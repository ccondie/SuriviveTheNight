using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SurviveTheNight {

    public class RocketLauncher : Gun {

        void Start()
        {
            fullAmmo = 1;
            curAmmo = 1;
            reloading = false;
            reloadTime = 3f;
            reloadTimeRemain = 0;
            ammoResource = "Rocket";
            previousShot = DateTime.UtcNow;
            shotDelayMilSec = 0;
            shotSound = Resources.Load("shot-rocket-launcher") as AudioClip;
        }

        override public void weaponSpecificFire(UserInputController.Click c, Vector2 target) {
            if (c == UserInputController.Click.LEFT_DOWN || c == UserInputController.Click.LEFT_DOUBLE) {
                Fire(target);
                playSound(shotSound, .5f);
                playSound(casingSound, .5f);
				GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraMovement> ().Shake(.4f, .4f);
            }
        }

        public override float gunMovementRestriction() {
            return .6f;
        }
    }
}
