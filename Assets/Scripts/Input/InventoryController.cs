using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

    public class InventoryController : MonoBehaviour {

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        //Determine if user clicked on inventory
        public bool clickHit() {
            //TODO: implement
            return false;
        }

        public void processClick(UserInputController.Click c, Vector2 position) {

        }

        public void processKeyPress() {
            if (Input.GetKeyDown(KeyCode.A)) {
                //
            } //etc.
        }
    }
}
