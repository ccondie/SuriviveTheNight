using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

	public class Player : MovingObject {

		private PlayerHealth myHealth;

        public float walkStaminaLoss = 0.72f;
        public float walkStaminaDelay = 0.03f;
        private float walkStaminaDelay_Cur;

        // 
        private float walkSpeed = 2.8f;
        private float runSpeed = 4.5f;
        private float slugSpeed = 1.35f;

        // Use this for initialization
        protected override void Start () {
			base.Start ();
			myHealth = GetComponent <PlayerHealth> ();
            walkStaminaDelay_Cur = walkStaminaDelay;
        }

		private void OnDisable() {}

        // Update is called once per frame
        void Update () {
			int horizontal = 0;
			int vertical = 0;

            // for tracking stamina drain off of time, not frames
            walkStaminaDelay_Cur -= Time.deltaTime;

            // set movement speed based on current stamina
            if ((float)myHealth.currentStamina / myHealth.startingStamina < 0.2)
            {
                this.moveTime = slugSpeed;
            }
            else
            {
                this.moveTime = walkSpeed;
            }

			if (!isMoving) {
                if(navigatingPath) {
                    if (path != null) {
                        ContinueAStar();
                    } else {
                        navigatingPath = false;
                    }
                }

                if (Input.GetMouseButtonDown (0)) {
                    Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);

					horizontal = worldToTile (mousePosition.x) - worldToTile (this.transform.position.x);
					vertical = worldToTile (mousePosition.y) - worldToTile (this.transform.position.y);

                    if (!(0 == horizontal && 0 == vertical)) {
                        //AttemptMove<Wall>(horizontal, vertical);
                        AttemptAStar<Wall>(mousePosition);
                    }
                } 
			}

            if(isMoving) {
                if(walkStaminaDelay_Cur < 0)
                {
                    myHealth.DecreaseStamina(walkStaminaLoss);
                    walkStaminaDelay_Cur = walkStaminaDelay;
                }
            }
			
		}

        protected override void AttemptAStar<T> (Vector2 target) {
            base.AttemptAStar<T> (target);
        }


        protected override void AttemptMove<T> (int xDir, int yDir) {
			//Debug.Log ("AttemptMove: xdir: " + xDir + "yDir: " + yDir);
			base.AttemptMove<T> (xDir, yDir);
			//RaycastHit2D hit;
		}

		private void OnTriggerEnter2D(Collider2D other) {
			// Do something if you collide with something
			//Debug.Log("OnTriggerEnter2D");
		}

		protected override void OnCantMove<T>(T component) {
			Wall hitWall = component as Wall;
			// Do something to the wall
            // HERE BE DRAGONS
			//Debug.Log("OnCantMove");
		}
	}

}
