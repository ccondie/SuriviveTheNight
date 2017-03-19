using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

	public class Door : MonoBehaviour {

		public int doorIntegrity;
		private static bool ieg = false;

		private void OnTriggerEnter2D(Collider2D other) {
			// Do something if you collide with something
			if (other.gameObject.tag == "Player" && !ieg) {
				EnemyManager.Instance.BeginIndoorEnemyGeneration ();
				ieg = true;
			} else if (other.gameObject.tag == "Enemy") {
				Debug.Log ("Door Integrity: " + (doorIntegrity-1));
				if (--doorIntegrity <= 0)
					Destroy (gameObject);
			} else if (other.gameObject.tag == "Weapon") {
				Debug.Log ("Door Integrity: " + (doorIntegrity-50));
				if ((doorIntegrity -= 50) <= 0)
					Destroy (gameObject);
			}
		}
	}

}