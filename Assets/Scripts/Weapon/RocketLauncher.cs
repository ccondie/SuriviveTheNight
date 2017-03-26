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
    }
}
