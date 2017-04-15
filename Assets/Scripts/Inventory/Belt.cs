using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SurviveTheNight {
    public class Belt {
        public int activeBeltItem = 0;
        Item[] items = new Item[6];
        Player playerScript;

        public Belt(Player p) {
            this.playerScript = p;
        }

        public void clickedOnMap(UserInputController.Click c, Vector2 target) {
            if (items[activeBeltItem] != null) {
                if (items[activeBeltItem] is Gun) {
                    if (((Gun)items[activeBeltItem]).clickedOnMap(c, target)) {
                        playerScript.playFireAnimation(target);
                    }
                }
            }
        }

        public void addItem(Item item) {
            for (int i = 0; i < items.Length; i++) {
                if (items[i] == null) {
                    items[i] = item;
                    return;
                }
            }
            Debug.Log("Belt full");
        }

        public Item getActiveItem()
        {
            return items[activeBeltItem];
        }

        public float gunMovementRestriction() {
            //the float returned by this function is the percent of how fast the player should move
            //i.e. 1f = full speed
            // .7f = 70% speed
            // 0f = can't move
            if (items[activeBeltItem] != null) {
                if (items[activeBeltItem] is Gun) {
                    return ((Gun)items[activeBeltItem]).gunMovementRestriction();
                }
            }
            return 1f;
        }
    }
}
