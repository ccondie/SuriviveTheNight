using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace SurviveTheNight
{
    public class Zombie : MovingObject
    {
        public GameObject player;

		#region Health Related Variables
        public float startingHealth = 100f;
        public float currentHealth;
		public Slider healthSlider;
		public CanvasRenderer sliderColor;
		#endregion
		#region Staming Related Variables
		public float startingStamina = 100f;
        public float currentStamina;

        public float staminaGain = 0.4f;
        public float staminaGainDelay = 0.03f;   // should update about 10 times a second
        private float staminaGainDelay_Cur;
		#endregion
		#region Movement Variables
		private float moveSpeed = 1.35f;

        public float walkStaminaLoss = 0.72f;
        public float walkStaminaDelay = 0.03f;
        private float walkStaminaDelay_Cur;
		#endregion

        private float damage = 20f;
		private float t = 0f;

		private IEnumerator dtpCoroutine;

		public AudioClip attackSound;
		private AudioSource audioSource;
		private static float ATTACK_VOLUME = 1.0f;

		public AudioClip hurtSound;
		private AudioSource hurtSource;
		private static float HURT_VOLUME = 1.0f;

        void Awake()
        {
          player = GameObject.FindGameObjectWithTag("Player");
            currentHealth = startingHealth;
            currentStamina = startingStamina;
			healthSlider.value = currentHealth;
            staminaGainDelay_Cur = staminaGainDelay;
            walkStaminaDelay_Cur = walkStaminaDelay;
            InvokeRepeating("DecideNextAction", 0.0f, 4f);
            InvokeRepeating("TryAttackPlayer", 0.0f, 2f);
			audioSource = GetComponent<AudioSource>();
        }

        // Use this for initialization
        protected override void Start()
        {
            // Initialize the MoveableObject components
            base.Start();
        }

        // Update is called once per frame
        void Update()
        {
			if (isDead)
				Countdown ();
			else
            	UpdateMovement();
        }

        void UpdateMovement()
        {
            walkStaminaDelay_Cur -= Time.deltaTime;
            moveTime = moveSpeed;

			if (!isMoving && !isDead) {
				if (squareDistToPlayer() < 225f*scale*scale) {
                    if (navigatingPath && (!path.targetHasMoved(player.transform.position))) {
                        if (path != null) {
                            ContinueAStar();
                        } else {
                            navigatingPath = false;
                        }
                    } else {
                        Vector2 target = targetClosestToPlayer(player.transform.position);
                        if (hasLineOfSight(target)) {
							dtpCoroutine = directlyTrackPlayer ();
							StartCoroutine(dtpCoroutine);
                        } else {
                            path = null;
                            navigatingPath = false;
                            isMoving = false;

                            AttemptMoveAStar<Wall>(player.transform.position);
                        }
                    }
                }
			} else {
				if (walkStaminaDelay_Cur < 0 && !isDead)
                {
                    DecreaseStamina(walkStaminaLoss);
                    walkStaminaDelay_Cur = walkStaminaDelay;
                }
            }
		}

        private void TryAttackPlayer() {
			if (!player.GetComponent<Player>().isDead && !isDead && squareDistToPlayer() < 2 * scale * scale) {
				audioSource.PlayOneShot(attackSound, ATTACK_VOLUME);
                Player playerScript = (Player)player.GetComponent("Player");
                playerScript.TakeDamage(damage);
            }
        }

		public double distToPlayer() {
            return Math.Sqrt(
                Math.Pow((double) player.transform.position.x - (double) transform.position.x, 2)
                +
                Math.Pow((double) player.transform.position.y - (double) transform.position.y, 2)
            );
		}

		public double squareDistToPlayer() {
			float xDist = player.transform.position.x - transform.position.x;
			float yDist = player.transform.position.y - transform.position.y;
			return xDist * xDist + yDist * yDist;
		}

        private Vector2 randomTarget(float distance) {
            Vector2 target;
            do {
                target = new Vector2(
                    UnityEngine.Random.Range(
                        transform.position.x - distance,
                        transform.position.x + distance
                    ),
                    UnityEngine.Random.Range(
                        transform.position.y - distance,
                        transform.position.y + distance
                    )
                );
            } while (!hasLineOfSight(target));
            return target;
        }

        void DecideNextAction() {

            //Debug.Log("deciding next action");
            if (isMoving) {
                //if moving, don't interrupt
                //Debug.Log("\tmoving, don't interrupt");
                return;
            } else if (stayPut()) {
                //Debug.Log("\tstay put");
                return;
            }

            
			if (squareDistToPlayer() < 225f*scale*scale) {
                //Debug.Log("within range of player");
                Vector2 target = targetClosestToPlayer(player.transform.position);
                if (hasLineOfSight(target)) {
                    //Debug.Log("\tdirectly tracking player");
                    StartCoroutine(directlyTrackPlayer());
                } else {
                    //Debug.Log("\tgo to player");
                    AttemptMoveAStar<Wall>(target);
                }
            } else {
                //otherwise, a* to random location
                //Debug.Log("\tcalculating random target");
                AttemptMoveAStar<Wall>(randomTarget(5*scale));
                //Debug.Log("\tmoving to random target");
            }

        }

        private bool stayPut() {
            //if the zombie is adjacent to the player
            if (Math.Abs(BoardManager.worldToTile(player.transform.position.x) - BoardManager.worldToTile(transform.position.x)) <= 1.1) {
                if (Math.Abs(BoardManager.worldToTile(player.transform.position.y) - BoardManager.worldToTile(transform.position.y)) <= 1.1) {
                    return true;
                }
            }
            //if the zombie is trapped on all sides
            if (surrounded()) {
                return true;
            }
            return false;
        }

        protected override void AttemptMoveAStar<T>(Vector2 target)
        {
            base.AttemptMoveAStar<T>(target);
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

        private IEnumerator directlyTrackPlayer() {
            //Debug.Log("Directly tracking player");
            Vector3 target = targetClosestToPlayer(player.transform.position);
            float sqrRemainingDistance = (transform.position - target).sqrMagnitude;
            while (sqrRemainingDistance > float.Epsilon && hasLineOfSight(target) && !isDead) {
                isMoving = true;
                Vector3 newPosition = Vector3.MoveTowards(rb2D.position, target, moveTime * Time.deltaTime);
                rb2D.MovePosition(newPosition);
                target = targetClosestToPlayer(player.transform.position);
				defineAnimationState (target);
                sqrRemainingDistance = (transform.position - target).sqrMagnitude;
                yield return null;
                
            }
            isMoving = false;
            //Debug.Log("No longer tracking player");
        }

        private Vector2 targetClosestToPlayer(Vector2 playerLocation) {
            int xDir = BoardManager.worldToTile(playerLocation.x) - BoardManager.worldToTile(transform.position.x);
            int yDir = BoardManager.worldToTile(playerLocation.y) - BoardManager.worldToTile(transform.position.y);

            int absX = Mathf.Abs(xDir);
            int absY = Mathf.Abs(yDir);

            float deltaTargetX = 0f;
            float deltaTargetY = 0f;

            // Define animation state
            if (yDir > 0 && yDir > absX << 1) {
                //animator.SetTrigger("walk_north");
                deltaTargetY -= 0.6f;
            } else if (yDir < 0 && absY > absX << 1) {
                //animator.SetTrigger("walk_south");
                deltaTargetY += 0.6f;
            } else if (xDir < 0 && absX > absY << 1) {
                //animator.SetTrigger("walk_west");
                deltaTargetX += 0.6f;
            } else if (xDir > 0 && xDir > absY << 1) {
                //animator.SetTrigger("walk_east");
                deltaTargetX -= 0.6f;
            } else if (xDir < 0 && yDir > 0) {
                //animator.SetTrigger("walk_northwest");
                deltaTargetY -= 0.6f;
                deltaTargetX += 0.6f;
            } else if (xDir > 0 && yDir > 0) {
                //animator.SetTrigger("walk_northeast");
                deltaTargetY -= 0.6f;
                deltaTargetX -= 0.6f;
            } else if (xDir < 0 && yDir < 0) {
                //animator.SetTrigger("walk_southwest");
                deltaTargetY += 0.6f;
                deltaTargetX += 0.6f;
            } else if (xDir > 0 && yDir < 0) {
                //animator.SetTrigger("walk_southeast");
                deltaTargetY += 0.6f;
                deltaTargetX -= 0.6f;
            }
            //Debug.Assert(!(deltaTargetY == 0 && deltaTargetX == 0));

            return new Vector2(playerLocation.x + deltaTargetX, playerLocation.y + deltaTargetY);
        }

		void OnTriggerEnter2D(Collider2D collider) {
			if (collider.gameObject.tag == "Weapon" && !isDead) {
                //Debug.Log("Zombie hit by bullet");
                if (collider.GetComponent<Projectile>()) {
                    Projectile p = collider.GetComponent<Projectile>();
                    receiveDamage(p.damage, p.player);
                }
			} /*else {
                Debug.Log("Zombie hit by unknown object");
                receiveDamage(currentHealth);
            }*/
		}

        public void receiveDamage(float damage, Player dealer) {
			currentHealth -= damage;
			healthSlider.value = currentHealth;
			if (squareDistToPlayer () < 25) {
				audioSource.PlayOneShot (hurtSound, HURT_VOLUME);
			}
            if (currentHealth <= 0) {
                //Debug.Log("Zombie defeated!");
				Die(player.GetComponent<Player>());
            } else {
                //Debug.Log("Zombie health: " + currentHealth + " (" + damage + " damage)");
            }
        }

        protected override void OnCantMove<T>(T component)
        {
                throw new NotImplementedException();
        }

		private void Die(Player killer) {
			isDead = true;
			Color c = sliderColor.GetColor ();
			sliderColor.SetColor (new Color(c.r, c.g, c.b, 0f));
			//animator.SetTrigger ("die");
			animator.Play ("z_death");
			//Debug.Log ("Die");
			if(dtpCoroutine != null) StopCoroutine (dtpCoroutine);
			if(coroutine != null) StopCoroutine (coroutine);
			boxCollider.enabled = false;
			Destroy (rb2D);
			DropItem ();
            killer.score += 1000;
		}

		private void Countdown() {
			t += Time.deltaTime;
			if (t >= 25f)
				Destroy (gameObject);
		}

		private void DropItem() {
			int spawnItem = Random.Range (0, 20);
			switch (spawnItem) {
			case 1:
			case 3:
			case 7:
			case 11:
			case 15:
			case 17:
				EnemyManager.Instance.PowerUpAtLocation (transform.position);
				break;
			case 13:
				// spawn something else
				break;
			}
		}
    }

}

