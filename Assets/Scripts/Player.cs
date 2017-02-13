using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

	public class Player : MovingObject {

		private int input_lag_def = 5;
		private int input_lag_count = 0;

		// Use this for initialization
		protected override void Start () {
			base.Start ();
		}

		private void OnDisable() {}
		
		// Update is called once per frame
		void Update () {
			int horizontal = 0;
			int vertical = 0;

			if (!isMoving && input_lag_count < 0) {
				horizontal = (int)Input.GetAxisRaw ("Horizontal");
				vertical = (int)Input.GetAxisRaw ("Vertical");
				//if (horizontal != 0)
				//	vertical = 0;
				//if (horizontal != 0 || vertical != 0)
				AttemptMove<Wall> (horizontal, vertical);
				input_lag_count = input_lag_def;
			} else
				input_lag_count--;
			
		}

		protected override void AttemptMove<T> (int xDir, int yDir) {
			Debug.Log ("AttemptMove");
			base.AttemptMove<T> (xDir, yDir);
			RaycastHit2D hit;
		}

		private void OnTriggerEnter2D(Collider2D other) {
			// Do something if you collide with something
			Debug.Log("OnTriggerEnter2D");
		}

		protected override void OnCantMove<T>(T component) {
			Wall hitWall = component as Wall;
			// Do something to the wall
			Debug.Log("OnCantMove");
		}
	}

}
