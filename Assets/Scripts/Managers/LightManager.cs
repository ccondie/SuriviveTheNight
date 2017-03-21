using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

	public class LightManager : Singleton<LightManager> {

		public Light sunPrefab;
		public Light lanternPrefab;
		private Light sun;
		private Light lantern;
		private float sun_position = 0;
		private TimeManager tm;

		protected LightManager () {}

		// Use this for initialization
		void Awake()
		{
			tm = TimeManager.Instance;
			sun = Instantiate(sunPrefab);
			lantern = Instantiate(lanternPrefab);
		}

		// Update is called once per frame
		void Update()
		{
			sun_position = 2 * Mathf.PI * tm.CurrentTime();
			sun.intensity = 0.5f - 0.5f*Mathf.Cos(sun_position);
			lantern.intensity = 0.5f + 0.5f*Mathf.Cos(sun_position);
		}
	}

}