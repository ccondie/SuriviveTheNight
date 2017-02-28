using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

	public class Player : MovingObject {

		private PlayerHealth myHealth;

		// Use this for initialization
		protected override void Start () {
			base.Start ();
			myHealth = GetComponent <PlayerHealth> ();
		}

		private void OnDisable() {}

        private int worldToTile(float position) {
            return (int)((position + 0.3) / 0.6);
        }

        // Update is called once per frame
        void Update () {
			int horizontal = 0;
			int vertical = 0;

			if (!isMoving) {
				if (Input.GetMouseButtonDown (0)) {
					Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
					horizontal = worldToTile (mousePosition.x) - worldToTile (this.transform.position.x);
					vertical = worldToTile (mousePosition.y) - worldToTile (this.transform.position.y);
				} else {
					horizontal = (int)Input.GetAxisRaw ("Horizontal");
					vertical = (int)Input.GetAxisRaw ("Vertical");
				}

				if (!(0 == horizontal && 0 == vertical)) {
					AttemptMove<Wall> (horizontal, vertical);
				}
			} else {
				myHealth.DecreaseStamina (1);
			}
			
		}

		protected override void AttemptMove<T> (int xDir, int yDir) {
			//Debug.Log ("AttemptMove: xdir: " + xDir + "yDir: " + yDir);
			base.AttemptMove<T> (xDir, yDir);
			RaycastHit2D hit;
		}

		private void OnTriggerEnter2D(Collider2D other) {
			// Do something if you collide with something
			//Debug.Log("OnTriggerEnter2D");
		}

		protected override void OnCantMove<T>(T component) {
			Wall hitWall = component as Wall;
			// Do something to the wall
			//Debug.Log("OnCantMove");
		}
	}

}
