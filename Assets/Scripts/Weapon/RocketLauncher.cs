using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SurviveTheNight {

    public class RocketLauncher : Gun {

        public GameObject rocket;

        // Use this for initialization
        /*protected override void Awake () {
			base.Awake ();
		}*/

        public override void Fire(Vector2 target) {
            //Debug.Log ("Fire rocket launcher!");
            Vector3 trans = (Vector3)target - transform.position;
            float angle = Mathf.Atan2(trans.y, trans.x) * Mathf.Rad2Deg - 90f;
            Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            Instantiate(rocket, transform.position, rotation);
        }
    }

}
