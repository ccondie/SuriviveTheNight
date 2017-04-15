using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

    public class MapController : MonoBehaviour {

        GameObject player;
        Player playerScript;

        // Use this for initialization
        void Start() {
            //player = GameObject.Find("Player");
			player = GameObject.FindGameObjectWithTag("Player");
            playerScript = (Player) player.GetComponent("Player");
        }

        // Update is called once per frame
        void Update() {

        }

        //Determine if user clicked on map
        public bool clickHit() {
            //TODO: implement
            return true;
        }

        public void processKey(KeyCode k) {
            if (k == KeyCode.Space) {
                playerScript.reloadActiveWeapon();
            }
        }

        public void processClick(UserInputController.Click c, Vector2 position) {
            //Debug.Log ("Click!");
            if (c >= UserInputController.Click.RIGHT_DOWN) {
                //MOVEMENT
                playerScript.setRun(c == UserInputController.Click.RIGHT_DOUBLE);
                if (c == UserInputController.Click.RIGHT_DOWN || c == UserInputController.Click.RIGHT_DOUBLE) {
                    playerScript.moveTo(Camera.main.ScreenToWorldPoint(position));
                } else if (c == UserInputController.Click.RIGHT_HOLD) {
                    //make a pop up menu appear
                }
            } else {
                //ATTACK
                if (c >= UserInputController.Click.LEFT_DOWN && c <= UserInputController.Click.LEFT_DOUBLE) {
                    playerScript.clickedOnMap(c, Camera.main.ScreenToWorldPoint(position));
                }
            }
        }
    }
}
