using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

	public abstract class Gun : Item {

		//public GameObject player;

		/*protected virtual void Awake() {
			player = GameObject.FindGameObjectWithTag("Player");
		}*/

		public abstract void Fire (Vector2 target);
	}

}