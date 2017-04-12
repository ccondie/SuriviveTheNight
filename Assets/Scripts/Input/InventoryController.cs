using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SurviveTheNight {

    public class InventoryController : MonoBehaviour {

        GameObject player;
        Player playerScript;
		GameObject[] beltButtons;

        // Use this for initialization
        void Start() {
            player = GameObject.FindGameObjectWithTag("Player");
            playerScript = (Player)player.GetComponent("Player");
			beltButtons = GameObject.FindGameObjectsWithTag ("BeltButton");
			//The pistol is #2 I don't know why but oh well it works
			Button b = beltButtons[2].GetComponent<Button>();
				ColorBlock cb = b.colors;
				cb.normalColor = selectedColor;
				cb.highlightedColor = selectedColor;
				cb.pressedColor = selectedColor;
			b.colors = cb;
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
		Color notSelectedColor = new Color (1f, 1f, 1f, 1f);
		Color selectedColor = new Color(0.0f, 0.75f, 1f, 1f);
		public void beltItemPressed(Button b) {
			foreach (GameObject g in beltButtons) {
				Button btn = g.GetComponent<Button>();
				ColorBlock block = btn.colors;
				block.normalColor = notSelectedColor;
				block.highlightedColor = notSelectedColor;
				block.pressedColor = notSelectedColor;
				btn.colors = block;
			}
			ColorBlock cb = b.colors;
			cb.normalColor = selectedColor;
			cb.highlightedColor = selectedColor;
			cb.pressedColor = selectedColor;
			b.colors = cb;
			int button = int.Parse (b.name.Substring (10));
			if (button != playerScript.getActiveBeltItem ()) {
				playerScript.switchWeapons ();
			}
            playerScript.setActiveBeltItem(button);
        }
    }
}
