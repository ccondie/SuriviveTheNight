using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

	public class Door : Destructable {

		private static bool ieg = false;

		private void OnTriggerEnter2D(Collider2D other) {
			// Do something if you collide with something
			if (other.gameObject.tag == "Player" && !ieg) {
				EnemyManager.Instance.BeginIndoorEnemyGeneration ();
				ieg = true;
			} else if (other.gameObject.tag == "Enemy") {
				AudioSource audioSource = other.GetComponent<Zombie>().GetComponent<AudioSource>();
				Zombie z = other.GetComponent<Zombie> ();
				if (!audioSource.isPlaying && !z.isDead && !z.player.GetComponent<Player>().isDead) {
						audioSource.Play ();
				}
				Damage (1);
			} else if (other.gameObject.tag == "Weapon") {
				if (other.GetComponent<Projectile>()) {
					Projectile p = other.GetComponent<Projectile>();
					Damage (p.damage);
					Destroy (other.gameObject);
				}
			}
		}

		protected override void UpdateSprite() {}
	}

}