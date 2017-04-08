using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

	public class Wall : Destructable {

		private bool[] damageLevel = new bool[5] {false,false,false,false,false};
		private float[] damagePercent = new float[5] {0.84f, 0.67f, 0.5f, 0.34f, 0.17f};
		
		private void OnTriggerEnter2D(Collider2D other) {
			// Do something if you collide with something
			if (other.gameObject.tag == "Enemy") {
				Damage (1);
			} else if (other.gameObject.tag == "Weapon") {
				if (other.GetComponent<Projectile>()) {
					Projectile p = other.GetComponent<Projectile>();
					Damage (p.damage);
					Destroy (other.gameObject);
				}
			}
		}

		protected override void UpdateSprite() {
			for (int i = 4; i >= 0; i--) {
				if (damageLevel [i])
					return;
				if (integrity <= startIntegrity * damagePercent [i]) {
					spriteRenderer.sprite = imageList [i];
					damageLevel [i] = true;
					return;
				}
			}
		}
	}
}
