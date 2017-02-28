using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace SurviveTheNight
{
    public class UIManager : MonoBehaviour
    {
        public Text timeUI = null;
        private static int time_count = 0;


        // Use this for initialization
        void Awake()
        {

        }

        // Update is called once per frame
        void Update()
        {
            time_count += 1;
            timeUI.text = "[" + time_count.ToString("D8") + "]";
        }
    }
}

