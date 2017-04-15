using System;
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

        public Player player;

        public AudioClip shotSound;
        public AudioClip casingSound;
        public AudioClip emptySound;
        public AudioClip cockingSound;

        void Awake() {
            emptySound = Resources.Load("empty") as AudioClip;
            cockingSound = Resources.Load("cocking") as AudioClip;
        }

        //returns whether the gun actually fired (if it had ammo, if the gun requires a wait time, etc.)
        void Update()
        {
            if (reloading)
            {
                if (reloadTimeRemain > 0)
                    reloadTimeRemain -= Time.deltaTime;
                else
                {
                    playSound(cockingSound, 1f);
                    curAmmo = fullAmmo;
                    reloading = false;
                }
            }
        }


        public bool clickedOnMap(UserInputController.Click c, Vector2 target)
        {
            double gap = DateTime.UtcNow.Subtract(previousShot).TotalMilliseconds;
            if (gap >= shotDelayMilSec)
            {
                previousShot = DateTime.UtcNow;
            }
            else
            {
                if (c == UserInputController.Click.LEFT_UP) {
                    weaponSpecificFire(c, target);
                }
                return false;
            }

            if (curAmmo > 0)
            {
                // standard fire case
                weaponSpecificFire(c, target);

                if(curAmmo == 0)
                {
					GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraMovement> ().EndShake ();
                    // if we hit zero ammo, initialize the reload cycle
                    reloading = true;
                    reloadTimeRemain = reloadTime;
                }

                return true;
            }
            else
            {
                playSound(emptySound, .5f);
                return false;
            }
        }

        public void Fire(Vector2 target) {
            curAmmo -= 1;
            Vector3 trans = (Vector3)target - transform.position;
            float angle = Mathf.Atan2(trans.y, trans.x) * Mathf.Rad2Deg - 90f;
            Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            GameObject projectileGameObject = Instantiate((GameObject)Resources.Load(ammoResource), transform.position, rotation);
            Projectile projectileScript = projectileGameObject.GetComponent<Projectile>();
            projectileScript.player = this.player;
        }

        public void playSound(AudioClip clip, float volume) {
            if (clip != null) {
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
            }
        }

        abstract public void weaponSpecificFire(UserInputController.Click c, Vector2 target);

        abstract public float gunMovementRestriction();

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