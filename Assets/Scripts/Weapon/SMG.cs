using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace SurviveTheNight {
	public class SMG : Gun {
		void Start() {
			fullAmmo = 50;
			curAmmo = 50;
			reloading = false;
			reloadTime = 4f;
			reloadTimeRemain = 0;
			ammoResource = "Bullet-SMG";
			previousShot = DateTime.UtcNow;
			shotDelayMilSec = 50;

			shotSound = Resources.Load("shot-handgun") as AudioClip;
			casingSound = Resources.Load("casing") as AudioClip;
		}

		override public void weaponSpecificFire(UserInputController.Click c, Vector2 target) {
			if (c == UserInputController.Click.LEFT_HOLD) {
				target.x += Random.Range(-.1f, .1f);
				target.y += Random.Range(-.1f, .1f);
				Fire(target);
				playSound(shotSound, .3f);

				if (curAmmo == 1) playSound(casingSound, 1f);
			}
		}
	}
}