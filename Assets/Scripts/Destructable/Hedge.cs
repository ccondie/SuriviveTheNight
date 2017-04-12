using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

	public class Hedge : Destructable {

		private void OnTriggerEnter2D(Collider2D other) {
			// Do something if you collide with something
			if (other.gameObject.tag == "Enemy") {
				AudioSource audioSource = other.GetComponent<AudioSource>();
				Zombie z = other.GetComponent<Zombie> ();
				if (!audioSource.isPlaying && !z.isDead && !z.player.GetComponent<Player>().isDead && z.squareDistToPlayer () < 10) {
						audioSource.Play ();
				}
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
