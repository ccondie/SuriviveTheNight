using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SurviveTheNight
{
    public class Handgun : Gun
    {
        void Start()
        {
            fullAmmo = 7;
            curAmmo = 7;
            reloading = false;
            reloadTime = 2.5f;
            reloadTimeRemain = 0;
            ammoResource = "Bullet";
            previousShot = DateTime.UtcNow;
            shotDelayMilSec = 0;
        }
    }
}
