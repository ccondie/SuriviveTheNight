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
            reloadTime = 1f;
            reloadTimeRemain = 0;
            ammoResource = "Rocket";
            previousShot = DateTime.UtcNow;
            shotDelayMilSec = 1000;
        }

        override public void clickType(UserInputController.Click c, Vector2 target) {
            if (c == UserInputController.Click.LEFT_DOWN || c == UserInputController.Click.LEFT_DOUBLE) {
                Fire(target);
            }
        }
    }
}
