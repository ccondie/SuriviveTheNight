using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SurviveTheNight {

	public class TimeManager : Singleton<TimeManager> {

		public int secondsPerHour;
        private float secondsPerMin;
		private int secondsPerDay;

		// These variables exist so that the expensive division operations
		// only happen once on startup
		private float minsPerSecond;
		private float hoursPerSecond;
		private float daysPerSecond;

		private float currentTime;

		private Text timeUI;
        private string aORp;

		private bool night = false;

		protected TimeManager () {}

		// Use this for initialization
		void Awake()
		{
			secondsPerMin = secondsPerHour / 60f;
			secondsPerDay = secondsPerHour * 24;
			minsPerSecond = 1f / secondsPerMin;
			hoursPerSecond = 1f / secondsPerHour;
			daysPerSecond = 1f / secondsPerDay;
			currentTime = secondsPerHour * 6;
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
				currentTime -= secondsPerDay;

            // Update AM or PM status
            aORp = currentTime < (12 * secondsPerHour) ? "AM" : "PM";

            // Update the UI Text
            timeUI.text = getHour() + ":" + getMinute().ToString("d2") + " " + aORp;

            // Toggle Day/Night
			float normalTime = currentTime * daysPerSecond;
			if (!night && (normalTime >= NormalizeTime(22,0) || normalTime < NormalizeTime(6,0))) {
				night = true;
				NightFall ();
			} else if (night && normalTime >= NormalizeTime(6,0) && normalTime < NormalizeTime(22,0)) {
				night = false;
				DayBreak ();
			}
		}

        // converts from human recognizable time to a normalized time (0-1) adjusted for the current game settings
        public float NormalizeTime(int hour, int min)
        {
            int totalSeconds = hour * secondsPerHour + (int)(min * secondsPerMin);

            return ((float)totalSeconds * daysPerSecond);
        }

        // returns the time of day [from 12:00:01am to 11:59:59pm] as a range of 0 to 1
        public float getCurrentNormalizedTime()
        {
			return currentTime * daysPerSecond;
        }

        public int getHour()
        {
			return (int)(currentTime * hoursPerSecond);
        }

        public int getMinute()
        {
			return (int)((currentTime % secondsPerHour) * 60 * hoursPerSecond);
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