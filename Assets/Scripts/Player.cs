using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SurviveTheNight {

	public class Player : MovingObject {

		//private PlayerHealth myHealth;

        // *******************************************************************************************************
        // HEALTH RELATED VARIABLES
        // *******************************************************************************************************
        public float startingHealth = 100f;
        public float currentHealth;
        public Slider healthSlider;

        // *******************************************************************************************************
        //      STAMINA RELATED VARIABLES 
        // *******************************************************************************************************
        public float startingStamina = 100f;
        public float currentStamina;
        public Slider staminaSlider;
        private Image staminaFill;
        public Color staminaBlue = new Color((0f / 255f), (114f / 255f), (188f / 255f), 1.0f);
        public Color staminaRed = new Color((158f / 255f), (11f / 255), (15f / 255f), 1.0f);

        public float staminaGain = 0.4f;
        public float staminaGainDelay = 0.03f;   // should update about 10 times a second
        private float staminaGainDelay_Cur;

        // *******************************************************************************************************
        // MOVEMENT VARIABLES
		// *******************************************************************************************************
		public GameObject moveRing;

        private float walkSpeed = 2.8f;
        private float runSpeed = 4.5f;
		private float slugSpeed = 1.35f;

        public float walkStaminaLoss = 0.72f;
        public float walkStaminaDelay = 0.03f;
        private float walkStaminaDelay_Cur;

        private DateTime previousClick = DateTime.UtcNow;
        private bool doubleClick = false;

        // *******************************************************************************************************
        // OTHER
        // *******************************************************************************************************

		// This should be moved to the player's belt or inventory at some point
		public Gun gun;

        void Awake()
        {
            currentHealth = startingHealth;
            currentStamina = startingStamina;
            staminaFill = staminaSlider.GetComponentsInChildren<Image>()[1];
            staminaGainDelay_Cur = staminaGainDelay;
            walkStaminaDelay_Cur = walkStaminaDelay;
			//gun = new Handgun ();
        }

        // Use this for initialization
        protected override void Start () {
            // Initialize the MoveableObject components
			base.Start ();
        }

		private void OnDisable() {}

        // Update is called once per frame
        void Update () {
            UpdateMovement();
            UpdateStamina();
		}

        public void moveTo(Vector2 target) {
            AttemptMoveAStar<Wall>(target);
        }

        public void setRun(bool run) {
            doubleClick = run;
        }

        protected override void AttemptMoveAStar<T> (Vector2 target) {
            moveRing.transform.position = target;
            moveRing.GetComponent<Renderer>().enabled = true;
            base.AttemptMoveAStar<T> (target);
        }

        // A subroutine of the Update function that handles player movement per update
        void UpdateMovement()
        {
			gun.transform.position = this.transform.position;

            // for tracking stamina drain off of time, not frames
            walkStaminaDelay_Cur -= Time.deltaTime;

            // set movement speed based on current stamina
            if (currentStamina / startingStamina < 0.2)
            {
                this.moveTime = slugSpeed;
            }
            else if (doubleClick) {
                this.moveTime = runSpeed;
            } else {
                this.moveTime = walkSpeed;
            }

			if (!isMoving) {
				if (navigatingPath) {
                    if (path != null) {
						ContinueAStar ();
						return;
					} else {
						navigatingPath = false;
					}
				} else {
                    moveRing.GetComponent<Renderer>().enabled = false;
                }
			} else if (isPlayerStanding()) {
				defineAnimationState(dest);
			}
        }

		private bool isPlayerStanding() {
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo (0);
			return stateInfo.IsName ("pm_face_north") ||
				stateInfo.IsName ("pm_face_south") ||
				stateInfo.IsName ("pm_face_west") ||
				stateInfo.IsName ("pm_face_east") ||
				stateInfo.IsName ("pm_face_northwest") ||
				stateInfo.IsName ("pm_face_northeast") ||
				stateInfo.IsName ("pm_face_southwest") ||
				stateInfo.IsName ("pm_face_southeast");
		}

        void UpdateStamina()
        {
            if (isMoving) {
                if (walkStaminaDelay_Cur < 0) {
                    DecreaseStamina(walkStaminaLoss);
                    walkStaminaDelay_Cur = walkStaminaDelay;
                }
            }

            // if enough time has past (fractions of a second) to increase the stamina, increase it
            staminaGainDelay_Cur -= Time.deltaTime;
            if (staminaGainDelay_Cur < 0)
            {
                IncreaseStamina(staminaGain);
                staminaGainDelay_Cur = staminaGainDelay;
            }

            staminaSlider.value = currentStamina;
            if (currentStamina / startingStamina < 0.2f)
                staminaFill.color = staminaRed;
            else
                staminaFill.color = staminaBlue;
        }

		private void OnTriggerEnter2D(Collider2D other) {
			// Do something if you collide with something
			if (other.gameObject.tag == "MoveRing")
				moveRing.GetComponent<Renderer> ().enabled = false;
			//Debug.Log("OnTriggerEnter2D");
		}

		protected override void OnCantMove<T>(T component) {
			Wall hitWall = component as Wall;
			// Do something to the wall
            // HERE BE DRAGONS
			//Debug.Log("OnCantMove");
		}


        private void TakeDamage(float amount)
        {
            currentHealth -= amount;
            healthSlider.value = currentHealth;
            //playerAudio.Play ();
            if (currentHealth <= 0 && !isDead)
            {
                Death();
            }
        }

        void Death()
        {
            isDead = true;
        }

        private void DecreaseStamina(float amount)
        {
            if (amount > currentStamina)
            {
                currentStamina = 0;
            }
            else
            {
                currentStamina -= amount;
            }
        }

        private void IncreaseStamina(float amount)
        {
            // only increase stamina if the stamina if it won't push the stamina above max
            float missingStamina = startingStamina - currentStamina;
            if (amount > missingStamina)
            {
                // if the amount would overflow to maxStamina, set to max stamina
                currentStamina = startingStamina;
            }
            else
            {
                // otherwise add the amount of stamina to the current stamina
                currentStamina += amount;
            }
        }

		public void fireWeapon(Vector2 target) {
			playFireAnimation(target);
			gun.Fire(target);
		}

		private void playFireAnimation(Vector2 target) {

			int xDir = BoardManager.worldToTile(target.x) - BoardManager.worldToTile(this.transform.position.x);
			int yDir = BoardManager.worldToTile(target.y) - BoardManager.worldToTile(this.transform.position.y);

			int absX = Mathf.Abs(xDir);
			int absY = Mathf.Abs(yDir);

			// choose animation state
			if (yDir > 0 && yDir > absX << 1)
				animator.SetTrigger("fire_north");
			else if (yDir < 0 && absY > absX << 1)
				animator.SetTrigger("fire_south");
			else if (xDir < 0 && absX > absY << 1)
				animator.SetTrigger("fire_west");
			else if (xDir > 0 && xDir > absY << 1)
				animator.SetTrigger("fire_east");
			else if (xDir < 0 && yDir > 0)
				animator.SetTrigger("fire_northwest");
			else if (xDir > 0 && yDir > 0)
				animator.SetTrigger("fire_northeast");
			else if (xDir < 0 && yDir < 0)
				animator.SetTrigger("fire_southwest");
			else if (xDir > 0 && yDir < 0)
				animator.SetTrigger("fire_southeast");
		}
    }

}
