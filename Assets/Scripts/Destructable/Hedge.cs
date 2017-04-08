using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

	public class Hedge : Destructable {

		private void OnTriggerEnter2D(Collider2D other) {
			// Do something if you collide with something
			if (other.gameObject.tag == "Enemy") {
				Damage (1);
			} else if (other.gameObject.tag == "Weapon") {
				if (other.GetComponent<Projectile>()) {
					Projectile p = other.GetComponent<Projectile>();
					Damage (p.damage);
				}
			}
		}

		protected override void UpdateSprite() {}
	}
}
