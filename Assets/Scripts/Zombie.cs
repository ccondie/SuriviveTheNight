using System;
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

            if (!isMoving) {
                Vector2 playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
                if (navigatingPath && (!path.playerHasMoved(playerPosition))) {
                    if (path != null) {
                        ContinueAStar();
                    } else {
                        navigatingPath = false;
                    }
                } else {
                    path = null;
                    navigatingPath = false;
                    isMoving = false;
                    //abortMovement = true;
                    
                    AttemptMoveAStar<Wall>(playerPosition);
                }
            }

            if (isMoving)
            {
                if (walkStaminaDelay_Cur < 0)
                {
                    DecreaseStamina(walkStaminaLoss);
                    walkStaminaDelay_Cur = walkStaminaDelay;
                }
            }
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

    protected override void OnCantMove<T>(T component)
        {
            throw new NotImplementedException();
        }
    }
}

