using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

	public class LightManager : Singleton<LightManager> {

		public Light sunPrefab;
		public Light lanternPrefab;

		private Light sun;
		private Light lantern;
		private TimeManager tm;

		private int sunUpHour = 6;
		private int sunUpMin = 0;
		private int sunDownHour = 20;
		private int sunDownMin = 0;

        private float sunUp;
		private float sunDown;

		private float transitionTimeDawnStart;
		private float transitionTimeDawnEnd;
		private float inverseTransitionTimeDawn;

		private float transitionTimeDuskStart;
		private float transitionTimeDuskEnd;
		private float inverseTransitionTimeDusk;

		private float currentNormalTime;

		protected LightManager () {}

		// Use this for initialization
		void Awake()
		{
			tm = TimeManager.Instance;

			sun = Instantiate(sunPrefab);
			lantern = Instantiate(lanternPrefab);

			sunUp = tm.NormalizeTime(sunUpHour, sunUpMin);
			sunDown = tm.NormalizeTime(sunDownHour, sunDownMin);

			transitionTimeDawnStart = tm.NormalizeTime(0,30);
			transitionTimeDawnEnd = tm.NormalizeTime(0,30);
			inverseTransitionTimeDawn = 1f / (transitionTimeDawnStart + transitionTimeDawnEnd);

			transitionTimeDuskStart = tm.NormalizeTime(2,0);
			transitionTimeDuskEnd = tm.NormalizeTime(0,30);
			inverseTransitionTimeDusk = 1f / (transitionTimeDuskStart + transitionTimeDuskEnd);
		}

		// Update is called once per frame
		void Update()
		{
			//sun_position = 2 * Mathf.PI * tm.getCurrentNormalizedTime();
			//sun.intensity = 0.5f - 0.5f*Mathf.Cos(sun_position);

			currentNormalTime = tm.getCurrentNormalizedTime();

            // sun could be in 4 places

			if(currentNormalTime <= (sunUp - transitionTimeDawnStart) || currentNormalTime > (sunDown + transitionTimeDuskEnd))
            {
                sun.intensity = 0.0f;
            } 
			else if (currentNormalTime > (sunUp - transitionTimeDawnStart) && currentNormalTime <= (sunUp + transitionTimeDawnEnd))
            {
                // transition into max daylight
				sun.intensity = 0.0f + ((currentNormalTime - (sunUp - transitionTimeDawnStart)) * inverseTransitionTimeDawn);
			}
			else if (currentNormalTime > (sunUp + transitionTimeDawnEnd) && currentNormalTime <= (sunDown - transitionTimeDuskStart))
            {
                // max daylight
                sun.intensity = 1.0f;
            }
			else if (currentNormalTime > (sunDown - transitionTimeDuskStart) && currentNormalTime <= (sunDown + transitionTimeDuskEnd))
            {
                // transition into min daylight
				sun.intensity = 1.0f - ((currentNormalTime - (sunDown - transitionTimeDuskStart)) * inverseTransitionTimeDusk);
			}

			lantern.intensity = 1.0f - sun.intensity;
        }

		public float GetSunBrightness() {
			return sun.intensity;
		}
	}
}