using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

    public class Path {
        //first step (0) is the target - last step (size-1) is current
        public List<Vector2> steps { get; set; }
        public int intermediateTarget { get; set; }

        public Path() {
            steps = new List<Vector2>();
        }

        public Vector2 calcFirstStep(Vector3 current, BoxCollider2D boxCollider, LayerMask blockingLayer) {
            RaycastHit2D hit;
            Vector2 start = current;

            Vector2 target = new Vector2();
            for (int i = 0; i < steps.Count; i++) {
                target = steps[i];

                boxCollider.enabled = false;
                hit = Physics2D.Linecast(start, target, blockingLayer);
                boxCollider.enabled = true;

                if (hit.transform == null) {
                    //nothing stopping you - go for it!
                    return target;
                }
            }

            Debug.Log("Didn't match any of the intermediate steps");
            return steps[steps.Count - 1];
        }
    }

}
