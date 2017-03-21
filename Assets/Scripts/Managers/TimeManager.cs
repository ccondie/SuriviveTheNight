using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SurviveTheNight {

	public class TimeManager : Singleton<TimeManager> {

        public Text timeUI;
		private static int secondsPerHour = 10;
        private static float secondsPerMin = (float)secondsPerHour / 60;
        private static int secondsPerDay = secondsPerHour * 24;
        private string aORp;

		private float currentTime;

		private bool night = false;

		protected TimeManager () {}

		// Use this for initialization
		void Awake()
		{
			currentTime = secondsPerHour * 19;
		}

        void Start()
        {
            timeUI = GameObject.Find("ClockText").GetComponent<Text>();
        }

		// Update is called once per frame
		void Update()
		{
            // Count current time
			currentTime += Time.deltaTime;
            if (currentTime > secondsPerDay)
                currentTime = 0;

            // Update AM or PM status
            if (currentTime <= (12 * secondsPerHour))
                aORp = "AM";
            else
                aORp = "PM";

            // Update the UI Text
            timeUI.text = getHour() + ":" + getMinute().ToString("d2") + " " + aORp;

            // Toggle Day/Night
			if (!night && (currentTime >= NormalizeTime(22,0) || currentTime < NormalizeTime(6,0))) {
				night = true;
				NightFall ();
			} else if (night && currentTime >= NormalizeTime(6,0) && currentTime < NormalizeTime(22,0)) {
				night = false;
				DayBreak ();
			}
		}

        // converts from human recognizable time to a normalized time (0-1) adjusted for the current game settings
        public static float NormalizeTime(int hour, int min)
        {
            int totalSeconds = hour * secondsPerHour + (int)(min * secondsPerMin);

            Debug.Log("NormalizeTime: " + hour + ':' + min + " - " + totalSeconds + " / " + secondsPerDay + " - " +  ((float)totalSeconds/ secondsPerDay));
            return ((float)totalSeconds / secondsPerDay);
        }

        // returns the time of day [from 12:00:01am to 11:59:59pm] as a range of 0 to 1
        public float getCurrentNormalizedTime()
        {
            return currentTime / secondsPerDay;
        }

        public int getHour()
        {
            return (int)(currentTime / secondsPerHour);
        }

        public int getMinute()
        {
            return (int)((currentTime % secondsPerHour) * 60 / secondsPerHour);
        }


		public bool IsNight() {
			return night;
		}

		void NightFall() {
			Debug.Log ("NightFall");
		}

		void DayBreak() {
			Debug.Log ("DayBreak");
			/*if(frames_per_spawn>10)
				frames_per_spawn--;*/
		}
	}

}