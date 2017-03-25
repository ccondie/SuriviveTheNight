using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

    public class InventoryController : MonoBehaviour {

        GameObject player;
        Player playerScript;

        // Use this for initialization
        void Start() {
            player = GameObject.FindGameObjectWithTag("Player");
            playerScript = (Player)player.GetComponent("Player");
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

        public void beltItemPressed(int button) {
            playerScript.setActiveBeltItem(button);
        }
    }
}
