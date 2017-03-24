using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SurviveTheNight {

    public class RocketLauncher : Gun {

        private DateTime previousShot = DateTime.UtcNow;
        private static double shotFrequency = 1000;

        public override void Fire(Vector2 target) {
            double gap = DateTime.UtcNow.Subtract(previousShot).TotalMilliseconds;
            if (gap >= shotFrequency) {
                previousShot = DateTime.UtcNow;
            } else {
                Debug.Log("Wait one second between shots from the rocket launcher");
                return;
            }
            //Debug.Log ("Fire rocket launcher!");
            Vector3 trans = (Vector3)target - transform.position;
            float angle = Mathf.Atan2(trans.y, trans.x) * Mathf.Rad2Deg - 90f;
            Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            Instantiate((GameObject)Resources.Load("Rocket"), transform.position, rotation);
        }
    }

}
