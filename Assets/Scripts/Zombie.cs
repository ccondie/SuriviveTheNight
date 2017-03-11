using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SurviveTheNight
{
    public class Zombie : MovingObject
    {
        GameObject player;

        // *******************************************************************************************************
        // HEALTH RELATED VARIABLES
        // *******************************************************************************************************
        public float startingHealth = 100f;
        public float currentHealth;

        // *******************************************************************************************************
        //      STAMINA RELATED VARIABLES 
        // *******************************************************************************************************
        public float startingStamina = 100f;
        public float currentStamina;

        public float staminaGain = 0.4f;
        public float staminaGainDelay = 0.03f;   // should update about 10 times a second
        private float staminaGainDelay_Cur;

        // *******************************************************************************************************
        // MOVEMENT VARIABLES
        // *******************************************************************************************************
        private float moveSpeed = 1.35f;

        public float walkStaminaLoss = 0.72f;
        public float walkStaminaDelay = 0.03f;
        private float walkStaminaDelay_Cur;

        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            currentHealth = startingHealth;
            currentStamina = startingStamina;
            staminaGainDelay_Cur = staminaGainDelay;
            walkStaminaDelay_Cur = walkStaminaDelay;
            InvokeRepeating("DecideNextAction", 0f, 4f);
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
            UpdateMovement();
        }

        void UpdateMovement()
        {
            walkStaminaDelay_Cur -= Time.deltaTime;
            moveTime = moveSpeed;

            /*if (!isMoving) {
                
                if (stayPut()) {
                    //do nothing
                } else if (navigatingPath && (!path.targetHasMoved(player.transform.position))) {
                    if (path != null) {
                        ContinueAStar();
                    } else {
                        navigatingPath = false;
                    }
                } else {
                    Vector2 target = targetClosestToPlayer(player.transform.position);
                    if (hasLineOfSight(target)) {
                       StartCoroutine(directlyTrackPlayer());
                    } else {
                        path = null;
                        navigatingPath = false;
                        isMoving = false;

                        AttemptMoveAStar<Wall>(player.transform.position);
                    }
                }
            }*/

            if (isMoving)
            {
                if (walkStaminaDelay_Cur < 0)
                {
                    DecreaseStamina(walkStaminaLoss);
                    walkStaminaDelay_Cur = walkStaminaDelay;
                }
            }
        }

        private double distToPlayer() {
            return Math.Sqrt(
                Math.Pow((double) player.transform.position.x - (double) transform.position.x, 2)
                +
                Math.Pow((double) player.transform.position.y - (double) transform.position.y, 2)
            );
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

            Vector2 target = targetClosestToPlayer(player.transform.position);
            if (distToPlayer() < 15*scale) {
                Debug.Log("within range of player");
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
                AttemptMoveAStar<Wall>(randomTarget(3*scale));
                //Debug.Log("\tmoving to random target");
            }

        }

        private bool stayPut() {
            //if the zombie is adjacent to the player
            if (Math.Abs(worldToTile(player.transform.position.x) - worldToTile(transform.position.x)) <= 1) {
                if (Math.Abs(worldToTile(player.transform.position.y) - worldToTile(transform.position.y)) <= 1) {
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
            while (sqrRemainingDistance > float.Epsilon && hasLineOfSight(target)) {
                isMoving = true;
                Vector3 newPosition = Vector3.MoveTowards(rb2D.position, target, moveTime * Time.deltaTime);
                rb2D.MovePosition(newPosition);
                target = targetClosestToPlayer(player.transform.position);
                sqrRemainingDistance = (transform.position - target).sqrMagnitude;
                yield return null;
                
            }
            isMoving = false;
            animator.SetTrigger("stop");
            //Debug.Log("No longer tracking player");
        }

        private Vector2 targetClosestToPlayer(Vector2 playerLocation) {
            int xDir = worldToTile(playerLocation.x) - worldToTile(transform.position.x);
            int yDir = worldToTile(playerLocation.y) - worldToTile(transform.position.y);

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

        protected override void OnCantMove<T>(T component)
            {
                throw new NotImplementedException();
            }
        }
}

