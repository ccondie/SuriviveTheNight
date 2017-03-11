using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

	public class Handgun : Gun {

		public override void Fire(Vector2 target) {
			Debug.Log ("Fire handgun!");
		}
	}

}