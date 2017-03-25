using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

	public abstract class Gun : Item {

		//public GameObject player;

		/*protected virtual void Awake() {
			player = GameObject.FindGameObjectWithTag("Player");
		}*/

        //returns whether the gun actually fired (if it had ammo, if the gun requires a wait time, etc.)
		public abstract bool Fire (Vector2 target);
	}

}