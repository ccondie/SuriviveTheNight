using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SurviveTheNight {

	public class TimeManager : MonoBehaviour {

		//public Text timeUI = null;
		public static int frame_count = 0;
		private GameObject lantern;
		private GameObject sun;
		private Light lantern_light;
		private Light sun_light;
		private int frames_per_day = 10000;
		private float time_rate;
		private float sun_position = 0;

		// Use this for initialization
		void Awake()
		{
			time_rate = 2 * Mathf.PI / frames_per_day;
			lantern = GameObject.FindGameObjectWithTag("Lantern");
			sun = GameObject.FindGameObjectWithTag("Sun");
			lantern_light = (Light)lantern.GetComponent(typeof(Light));
			sun_light = (Light)sun.GetComponent(typeof(Light));
		}

		// Update is called once per frame
		void Update()
		{
			frame_count += 1;
			sun_position += time_rate;
			//timeUI.text = "[" + frame_count.ToString("D8") + "]";

			sun_light.intensity = 1 + Mathf.Cos(sun_position);
			lantern_light.intensity = 0.5f - 0.5f*Mathf.Cos(sun_position);
		}
	}

}