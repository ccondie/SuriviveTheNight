using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

	public abstract class MovingObject : MonoBehaviour {

		public float moveTime = 0.1f;
		public LayerMask blockingLayer;
		private Animator animator;

		public bool isMoving = false;

		private BoxCollider2D boxCollider;
		private Rigidbody2D rb2D;
		private float inverseMoveTime;
		private float scale = 0.6f;

		// Use this for initialization
		protected virtual void Start () {
			animator = GetComponent<Animator>();
			boxCollider = GetComponent<BoxCollider2D> ();
			rb2D = GetComponent<Rigidbody2D> ();
			inverseMoveTime = 1f / moveTime;
		}

		protected bool Move(int xDir, int yDir, out RaycastHit2D hit) {
			Vector2 start = transform.position;
			Vector2 end = start + new Vector2 (xDir*scale, yDir*scale);
			boxCollider.enabled = false;
			hit = Physics2D.Linecast (start, end, blockingLayer);
			boxCollider.enabled = true;
			if (hit.transform == null) {
				StartCoroutine (SmoothMovement (end));
				return true;
			}
			return false;
		}

		protected IEnumerator SmoothMovement (Vector3 end) {
			float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			while (sqrRemainingDistance > float.Epsilon) {
				isMoving = true;
				Vector3 newPosition = Vector3.MoveTowards (rb2D.position, end, inverseMoveTime * Time.deltaTime);
				rb2D.MovePosition (newPosition);
				sqrRemainingDistance = (transform.position - end).sqrMagnitude;
				yield return null;
			}
			isMoving = false;
			animator.SetTrigger ("stop");
		}

		protected virtual void AttemptMove <T> (int xDir, int yDir) where T : Component {
			RaycastHit2D hit;
			int absX = Mathf.Abs (xDir);
			int absY = Mathf.Abs (yDir);
			if(yDir > 0 && yDir > absX<<1)
				animator.SetTrigger ("walk_north");
			else if(yDir < 0 && absY > absX<<1)
				animator.SetTrigger ("walk_south");
			else if(xDir < 0 && absX > absY<<1)
				animator.SetTrigger ("walk_west");
			else if(xDir > 0 && xDir > absY<<1)
				animator.SetTrigger ("walk_east");
			else if(xDir < 0 && yDir > 0)
				animator.SetTrigger ("walk_northwest");
			else if(xDir > 0 && yDir > 0)
				animator.SetTrigger ("walk_northeast");
			else if(xDir < 0 && yDir < 0)
				animator.SetTrigger ("walk_southwest");
			else if(xDir > 0 && yDir < 0)
				animator.SetTrigger ("walk_southeast");
			bool canMove = Move (xDir, yDir, out hit);
			if (hit.transform == null)
				return;
			T hitComponent = hit.transform.GetComponent <T> ();
			if (!canMove && hitComponent != null)
				OnCantMove (hitComponent);
		}
		
		protected abstract void OnCantMove<T>(T component)
			where T : Component;
	}

}
