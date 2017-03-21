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

        public float sunUp = TimeManager.NormalizeTime(6, 0);
        public float sunDown = TimeManager.NormalizeTime(22, 0);

        public float transitionTimeDawnStart = TimeManager.NormalizeTime(0,30);
        public float transitionTimeDawnEnd = TimeManager.NormalizeTime(0,30);

        public float transitionTimeDuskStart = TimeManager.NormalizeTime(2,0);
        public float transitionTimeDuskEnd = TimeManager.NormalizeTime(0,30);

        public float ct;

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
			//sun_position = 2 * Mathf.PI * tm.getCurrentNormalizedTime();
			//sun.intensity = 0.5f - 0.5f*Mathf.Cos(sun_position);
			//lantern.intensity = 0.5f + 0.5f*Mathf.Cos(sun_position);

            ct = tm.getCurrentNormalizedTime();

            // sun could be in 5 places

            if(ct <= (sunUp - transitionTimeDawnStart))
            {
                sun.intensity = 0.0f;
                lantern.intensity = 1.0f;
            }

            if (ct > (sunUp - transitionTimeDawnStart) && ct <= (sunUp + transitionTimeDawnEnd))
            {
                // transition into max daylight
                sun.intensity = 0.0f + ((ct - (sunUp - transitionTimeDawnStart)) / (transitionTimeDawnStart + transitionTimeDawnEnd));
                lantern.intensity = 1.0f - ((ct - (sunUp - transitionTimeDawnStart)) / (transitionTimeDawnStart + transitionTimeDawnEnd));
            }

            if (ct > (sunUp + transitionTimeDawnEnd) && ct <= (sunDown - transitionTimeDuskStart))
            {
                // max daylight
                sun.intensity = 1.0f;
                lantern.intensity = 0.0f;

            }

            if (ct > (sunDown - transitionTimeDuskStart) && ct <= (sunDown + transitionTimeDuskEnd))
            {
                // transition into min daylight
                sun.intensity = 1.0f - ((ct - (sunDown - transitionTimeDuskStart)) / (transitionTimeDuskStart + transitionTimeDuskEnd));
                lantern.intensity = 0.0f + ((ct - (sunDown - transitionTimeDuskStart)) / (transitionTimeDuskStart + transitionTimeDuskEnd));

            }

            if (ct > (sunDown + transitionTimeDuskEnd))
            {
                // min daylight
                sun.intensity = 0.0f;
                lantern.intensity = 1.0f;
            }

            

        }
	}

}