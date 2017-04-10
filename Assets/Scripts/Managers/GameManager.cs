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
		private int sunDownHour = 22;
		private int sunDownMin = 0;

		private float sunUp;
		private float sunDown;

		private float currentNormalTime;

		private TimeManager tm;


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
		
		// Update is called once per frame
		void Update () {

			currentNormalTime = tm.getCurrentNormalizedTime();

			if (sunUp == 0 || sunDown == 0) {
				sunUp = tm.NormalizeTime (sunUpHour, sunUpMin);
				sunDown = tm.NormalizeTime (sunDownHour, sunDownMin);
			}
				
			if(currentNormalTime < (sunUp) || currentNormalTime >= (sunDown))
			{
				if (!nightMusic.isPlaying)
				{

					gameMusic.Stop();

					nightMusic.Play();

				}	

			} 
			else if (currentNormalTime >= (sunUp) && currentNormalTime < (sunDown))
			{
				if (!gameMusic.isPlaying)
				{

					nightMusic.Stop();

					gameMusic.Play();

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
