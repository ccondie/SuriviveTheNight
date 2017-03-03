﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

	public abstract class MovingObject : MonoBehaviour {

        // moveTime - smaller is slower, larger is faster
        public float moveTime = 2.0f;
		public LayerMask blockingLayer;
		private Animator animator;

		public bool isMoving = false;
        protected bool navigatingPath = false;
        protected Path path;

		private BoxCollider2D boxCollider;
		private Rigidbody2D rb2D;
		public float scale = 0.6f;

		// Use this for initialization
		protected virtual void Start () {
			animator = GetComponent<Animator>();
			boxCollider = GetComponent<BoxCollider2D> ();
			rb2D = GetComponent<Rigidbody2D> ();
		}

        protected int worldToTile(float position) {
            return (int)((position + (scale / 2)) / scale);
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
				Vector3 newPosition = Vector3.MoveTowards (rb2D.position, end, moveTime * Time.deltaTime);
				rb2D.MovePosition (newPosition);
				sqrRemainingDistance = (transform.position - end).sqrMagnitude;
				yield return null;
			}
			isMoving = false;
			animator.SetTrigger ("stop");
		}

		/*protected virtual void AttemptMove <T> (int xDir, int yDir) where T : Component {
			RaycastHit2D hit;
			int absX = Mathf.Abs (xDir);
			int absY = Mathf.Abs (yDir);

			// Define animation state
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
		}*/

        protected void ContinueAStar() {
            Vector2 nextStep = path.calcFirstStep(transform.position, boxCollider, blockingLayer);
            defineAnimationState(nextStep);
            StartCoroutine(SmoothMovement(nextStep));

            if (nextStep == path.steps[0]) {
                navigatingPath = false;
            } else {
                navigatingPath = true;
            }
        }

        protected virtual void AttemptMoveAStar<T>(Vector2 target) {
            RaycastHit2D hit;

            Vector2 start = transform.position;
            boxCollider.enabled = false;
            hit = Physics2D.Linecast(start, target, blockingLayer);
            boxCollider.enabled = true;

            if (hit.transform == null) {
                //there's a straight path
                defineAnimationState(target);
                StartCoroutine(SmoothMovement(target));
            } else {
                //try A* magic
                CalculatePathAStar(target);
            }
        }

        private void CalculatePathAStar(Vector2 target) {
            AStar algorithm = new SurviveTheNight.AStar(transform.position, target, scale);
            path = algorithm.calculatePath();
            Vector2 firstStep = path.calcFirstStep(transform.position, boxCollider, blockingLayer);
            defineAnimationState(firstStep);
            StartCoroutine(SmoothMovement(firstStep));

            if (firstStep == path.steps[0]) {
                navigatingPath = false;
            } else {
                navigatingPath = true;
            }
        }
        
        private void defineAnimationState(Vector2 target) {

            int xDir = worldToTile(target.x) - worldToTile(this.transform.position.x);
            int yDir = worldToTile(target.y) - worldToTile(this.transform.position.y);

            int absX = Mathf.Abs(xDir);
            int absY = Mathf.Abs(yDir);

            // Define animation state
            if (yDir > 0 && yDir > absX << 1)
                animator.SetTrigger("walk_north");
            else if (yDir < 0 && absY > absX << 1)
                animator.SetTrigger("walk_south");
            else if (xDir < 0 && absX > absY << 1)
                animator.SetTrigger("walk_west");
            else if (xDir > 0 && xDir > absY << 1)
                animator.SetTrigger("walk_east");
            else if (xDir < 0 && yDir > 0)
                animator.SetTrigger("walk_northwest");
            else if (xDir > 0 && yDir > 0)
                animator.SetTrigger("walk_northeast");
            else if (xDir < 0 && yDir < 0)
                animator.SetTrigger("walk_southwest");
            else if (xDir > 0 && yDir < 0)
                animator.SetTrigger("walk_southeast");
        }

        protected abstract void OnCantMove<T>(T component)
			where T : Component;
	}

}
