using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SurviveTheNight {

	public class TimeManager : Singleton<TimeManager> {

		//public Text timeUI = null;
		public int secondsPerHour;
		private int hoursPerDay = 24;
		private float dayTime;
		private float inverseDayTime;
		private float fractionOfDay;
		private float timeOfDay;
		private bool night = false;

		protected TimeManager () {}

		// Use this for initialization
		void Awake()
		{
			dayTime = secondsPerHour * hoursPerDay;
			inverseDayTime = 1.0f / dayTime;
			timeOfDay = secondsPerHour * 6;
		}

		// Update is called once per frame
		void Update()
		{
			timeOfDay += Time.deltaTime;
			timeOfDay -= timeOfDay >= dayTime ? dayTime : 0f;
			fractionOfDay = timeOfDay * inverseDayTime;

			if (!night && (timeOfDay >= 22 * secondsPerHour || timeOfDay < 6 * secondsPerHour)) {
				night = true;
				NightFall ();
			} else if (night && timeOfDay >= 6 * secondsPerHour && timeOfDay < 22 * secondsPerHour) {
				night = false;
				DayBreak ();
			}
		}

		public float CurrentTime() {
			return fractionOfDay;
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