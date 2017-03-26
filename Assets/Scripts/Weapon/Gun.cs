﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

	public abstract class Gun : Item {

        protected string ammoResource;
        protected int fullAmmo;
        protected int curAmmo;

        protected bool reloading;
        protected float reloadTime;
        protected float reloadTimeRemain;

        protected DateTime previousShot;
        protected double shotDelayMilSec;

        //returns whether the gun actually fired (if it had ammo, if the gun requires a wait time, etc.)
        void Update()
        {
            if (reloading)
            {
                if (reloadTimeRemain > 0)
                    reloadTimeRemain -= Time.deltaTime;
                else
                {
                    curAmmo = fullAmmo;
                    reloading = false;
                }
            }
        }


        public bool Fire(Vector2 target)
        {
            double gap = DateTime.UtcNow.Subtract(previousShot).TotalMilliseconds;
            if (gap >= shotDelayMilSec)
            {
                previousShot = DateTime.UtcNow;
            }
            else
            {
                return false;
            }

            if (curAmmo > 0)
            {
                // standard fire case
                curAmmo -= 1;
                Vector3 trans = (Vector3)target - transform.position;
                float angle = Mathf.Atan2(trans.y, trans.x) * Mathf.Rad2Deg - 90f;
                Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
                Instantiate((GameObject)Resources.Load(ammoResource), transform.position, rotation);

                if(curAmmo == 0)
                {
                    // if we hit zero ammo, initialize the reload cycle
                    reloading = true;
                    reloadTimeRemain = reloadTime;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public int getFullAmmo()
        {
            return fullAmmo;
        }

        public int getCurAmmo()
        {
            return curAmmo;
        }

        public bool isReloading()
        {
            return reloading;
        }

        public float getReloadTime()
        {
            return reloadTime;
        }

        public float getReloadTimeRemain()
        {
            return reloadTimeRemain;
        }
	}

}