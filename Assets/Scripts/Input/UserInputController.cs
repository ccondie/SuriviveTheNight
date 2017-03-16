using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

    public class UserInputController : MonoBehaviour {

        public enum Click { NONE, LEFT_DOWN, LEFT_HOLD, LEFT_UP, LEFT_DOUBLE, RIGHT_DOWN, RIGHT_HOLD, RIGHT_UP, RIGHT_DOUBLE };
        private int doubleClickThreshold = 300;
        private DateTime previousClickLeft = DateTime.UtcNow;
        private Vector2 previousClickLeftLocation;
        private bool waitingOnClickTypeLeft = false;
        private bool leftClickHolding = false;
        private DateTime previousClickRight = DateTime.UtcNow;
        private Vector2 previousClickRightLocation;
        private bool waitingOnClickTypeRight = false;
        private bool rightClickHolding = false;

        //Sub Controllers
        private MainMenuController mmc;
        private InventoryController ic;
        private MapController mc;

        // Use this for initialization
        void Start() {
            mmc = GetComponent<MainMenuController>();
            ic = GetComponent<InventoryController>();
            mc = GetComponent<MapController>();
        }

        // Update is called once per frame
        void Update() {
            //LEFT CLICKS
            if (Input.GetMouseButtonDown(0)) {
                //left click down
                if (!waitingOnClickTypeLeft) {
                    waitingOnClickTypeLeft = true;
                    previousClickLeft = DateTime.UtcNow;
                    previousClickLeftLocation = Input.mousePosition;
                } else {
                    double gap = DateTime.UtcNow.Subtract(previousClickLeft).TotalMilliseconds;
                    if (gap <= doubleClickThreshold) {
                        waitingOnClickTypeLeft = false;
                        assignClickToController(Click.LEFT_DOUBLE, previousClickLeftLocation);
                    }
                }
            } else if (leftClickHolding && Input.GetMouseButton(0)) {
                assignClickToController(Click.LEFT_HOLD, Input.mousePosition);
            } else if (Input.GetMouseButtonUp(0)) {
                //left click up
                if (leftClickHolding) {
                    leftClickHolding = false;
                    assignClickToController(Click.LEFT_UP, Input.mousePosition);
                }
            } else if (waitingOnClickTypeLeft) {
                double gap = DateTime.UtcNow.Subtract(previousClickLeft).TotalMilliseconds;
                if (gap > doubleClickThreshold) {
                    waitingOnClickTypeLeft = false;
                    if (Input.GetMouseButton(0)) {
                        leftClickHolding = true;
                        assignClickToController(Click.LEFT_HOLD, Input.mousePosition);
                    } else {
                        assignClickToController(Click.LEFT_DOWN, Input.mousePosition);
                    }
                }
            }

            //RIGHT CLICKS
            if (Input.GetMouseButtonDown(1)) {
                //left click down
                if (!waitingOnClickTypeRight) {
                    waitingOnClickTypeRight = true;
                    previousClickRight = DateTime.UtcNow;
                    previousClickRightLocation = Input.mousePosition;
                } else {
                    double gap = DateTime.UtcNow.Subtract(previousClickRight).TotalMilliseconds;
                    if (gap <= doubleClickThreshold) {
                        waitingOnClickTypeRight = false;
                        assignClickToController(Click.RIGHT_DOUBLE, previousClickRightLocation);
                    }
                }
            } else if (Input.GetMouseButtonUp(1)) {
                //left click up
                if (rightClickHolding) {
                    rightClickHolding = false;
                    assignClickToController(Click.RIGHT_UP, Input.mousePosition);
                }
            } else if (waitingOnClickTypeRight) {
                double gap = DateTime.UtcNow.Subtract(previousClickRight).TotalMilliseconds;
                if (gap > doubleClickThreshold) {
                    waitingOnClickTypeRight = false;
                    if (Input.GetMouseButton(1)) {
                        rightClickHolding = true;
                        assignClickToController(Click.RIGHT_HOLD, Input.mousePosition);
                    } else {
                        assignClickToController(Click.RIGHT_DOWN, Input.mousePosition);
                    }
                }
            }

            //TODO: checking key presses (as far as I know, only used for cycling through belt)
        }

        private void assignClickToController(Click c, Vector2 position) {
            if (mc.clickHit()) {
                mc.processClick(c, position);
            } else if (ic.clickHit()) {
                ic.processClick(c, position);
            } else if (mmc.clickHit()) {
                mmc.processClick(c, position);
            } 
        }
    }
}
