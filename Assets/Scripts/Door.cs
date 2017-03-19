using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

	public class Door : MonoBehaviour {

		private static bool ieg = false;

		private void OnTriggerEnter2D(Collider2D other) {
			Debug.Log (other);
			// Do something if you collide with something
			if (other.gameObject.tag == "Player" && !ieg) {
				EnemyManager.Instance.BeginIndoorEnemyGeneration ();
				ieg = true;
			}
		}
	}

}