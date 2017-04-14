using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace SurviveTheNight {

	using System.Collections.Generic;
	using UnityEngine.UI;

	public class GameManager : MonoBehaviour {

		public static GameManager instance = null;

		AudioSource[] music;
		AudioSource gameMusic;
		AudioSource nightMusic;

		private int sunUpHour = 6;
		private int sunUpMin = 0;
		private int sunDownHour = 19;
		private int sunDownMin = 0;

		private float sunUp;
		private float sunDown;

		private float currentNormalTime;

		private TimeManager tm;

		public static class AudioFadeOut {

			public static IEnumerator FadeOut (AudioSource audioSource, float FadeTime) {
				float startVolume = audioSource.volume;

				while (audioSource.volume > 0) {
					audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

					yield return null;
				}

				audioSource.Stop ();
				audioSource.volume = startVolume;
			}

		}

		public static class AudioFadeIn {

			public static IEnumerator FadeIn (AudioSource audioSource, float FadeTime) {
				float startVolume = audioSource.volume;
				audioSource.volume = 0;
				audioSource.Play ();
				while (audioSource.volume < startVolume) {
					audioSource.volume += startVolume * Time.deltaTime / FadeTime;

					yield return null;
				}
					
				audioSource.volume = startVolume;
			}

		}

		// Use this for initialization
		void Awake () {
			music = GetComponents<AudioSource>();
			gameMusic = music[0];
			nightMusic = music[1];

			tm = TimeManager.Instance;

			Application.targetFrameRate = 60;
			if (instance == null)
				instance = this;
			else if (instance != this)
				Destroy(gameObject);	
			DontDestroyOnLoad(gameObject);
			InitGame ();



		}

		void InitGame() {
			BoardManager.Instance.SetupScene ();
		}
		bool sunWentDown = false;
		bool sunCameUp = false;
		// Update is called once per frame
		void Update () {

			currentNormalTime = tm.getCurrentNormalizedTime();

			if (sunUp == 0 || sunDown == 0) {
				sunUp = tm.NormalizeTime (sunUpHour, sunUpMin);
				sunDown = tm.NormalizeTime (sunDownHour, sunDownMin);
			}
				
			if(currentNormalTime < (sunUp) || currentNormalTime >= (sunDown))
			{
				if (!sunWentDown)
				{
					sunWentDown = true;
					sunCameUp = false;
					// nightMusic.Play();

					StartCoroutine(AudioFadeOut.FadeOut (gameMusic, 5f));

				}	

			} 
			else if (currentNormalTime >= (sunUp) && currentNormalTime < (sunDown))
			{
				if (!sunCameUp)
				{
					sunCameUp = true;
					sunWentDown = false;
					// StartCoroutine(AudioFadeIn.FadeIn (gameMusic, 5f));
					StartCoroutine(AudioFadeOut.FadeOut (nightMusic, 5f));
				}
			}
		}

        public int[,] getWallMap() {
			return BoardManager.Instance.getWallMap();
		}

		public Vector3 getRandomSpawnPosition() {
			return BoardManager.Instance.getRandomSpawnPosition();
		}

		public Vector3 getRandomIndoorPosition() {
			return BoardManager.Instance.getRandomIndoorPosition();
		}

        public List<GameObject> getEnemies() {
			return EnemyManager.Instance.enemies;
        }
    }
}
