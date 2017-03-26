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

        private static float staminaGain = 0.1f;

        private Dictionary<String, float> moveStaminaLossMap = new Dictionary<string, float>
        {
            {"none", 0.0f },
            {"walk", 0.15f },
            {"run", 0.5f },
            {"slug", 0.15f }
        };

        // *******************************************************************************************************
        // MOVEMENT VARIABLES
        // *******************************************************************************************************
		public GameObject moveRingPrefab;
		private GameObject moveRing;

        private Dictionary<String, float> moveSpeedMap = new Dictionary<string, float>()
        {
            {"none", 0.0f },
            {"walk", 1.8f },
            {"run", 2.6f },
            {"slug", 0.9f }
        };

        public String moveState = "none";

        private DateTime previousClick = DateTime.UtcNow;
        private bool doubleClick = false;

        // *******************************************************************************************************
        // OTHER
        // *******************************************************************************************************

        // This should be moved to the player's belt or inventory at some point
        public Belt belt;

        public GameOverScreen gameOverScreen;

        void Awake()
        {
			moveRing = Instantiate (moveRingPrefab);
            Reset();
            staminaFill = staminaSlider.GetComponentsInChildren<Image>()[1];
            belt = new Belt(this);
            belt.addItem(gameObject.AddComponent<Handgun>());
            belt.addItem(gameObject.AddComponent<RocketLauncher>());
        }

        public void Reset()
        {
            currentHealth = startingHealth;
            healthSlider.value = currentHealth;
            currentStamina = startingStamina;
            staminaSlider.value = currentStamina;
            isDead = false;
        }

        // Use this for initialization
        protected override void Start () {
            // Initialize the MoveableObject components
			base.Start ();

            // Kickstart the stamina managment functions
            InvokeRepeating("UpdateStamina",0.0f, 0.03f);
        }

		private void OnDisable() {}

        // Update is called once per frame
        void Update () {
            UpdateMovement();
		}

        public void moveTo(Vector2 target) {
            if (coroutine != null)
                StopCoroutine(coroutine);
            //path = null;
            //navigatingPath = false;
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
			//belt.gun.transform.position = this.transform.position;

            // set movement speed based on current stamina
            if (currentStamina / startingStamina < 0.2){
                moveState = "slug";
            }
            else if (doubleClick) {
                moveState = "run";
            } else {
                moveState = "walk";
            }

            this.moveTime = moveSpeedMap[moveState];

			if (!isMoving) {
                moveState = "none";
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
            float deltaStamina = staminaGain - moveStaminaLossMap[moveState];
            //Debug.Log(staminaGain + " - " + moveStaminaLossMap[moveState] + " = " + deltaStamina);

            DeltaStamina(deltaStamina);

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
			else if (other.gameObject.tag == "Collectable") {
				Heal(other.gameObject.GetComponent<FirstAidKit>().healFactor);
				Destroy(other.gameObject);
			}
			//Debug.Log("OnTriggerEnter2D");
		}

		protected override void OnCantMove<T>(T component) {
			Wall hitWall = component as Wall;
			// Do something to the wall
            // HERE BE DRAGONS
			//Debug.Log("OnCantMove");
		}


        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            healthSlider.value = currentHealth;
            //playerAudio.Play ();
            if (currentHealth <= 0 && !isDead)
            {
                Death();
            }
        }

		public void Heal(float amount)
		{
			currentHealth = currentHealth + amount > startingHealth ? startingHealth : currentHealth + amount;
			healthSlider.value = currentHealth;
		}

        void Death()
        {
            isDead = true;
            gameOverScreen.GetComponent<Canvas>().enabled = true;
        }

        private void DeltaStamina(float amount)
        {
            // Alter the current stamina by a delta
            currentStamina += amount;

            // If it breaks the stamina ceiling, set it to max stamina
            if(currentStamina > startingStamina)
            {
                currentStamina = startingStamina;
            }

            // If it breaks the stamina floor, set it to 0
            if(currentStamina < 0)
            {
                currentStamina = 0;
            }
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

        public void clickedOnMap(UserInputController.Click c, Vector2 target) {
            belt.clickedOnMap(c, target);
        }

        public void setActiveBeltItem(int index) {
            belt.activeBeltItem = index;
        }

        public void playFireAnimation(Vector2 target) {

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
