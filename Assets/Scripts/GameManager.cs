using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace SurviveTheNight {

	using System.Collections.Generic;
	using UnityEngine.UI;

	public class GameManager : MonoBehaviour {

		public static GameManager instance = null;
        private BoardManager boardScript;
		private EnemyManager enemyScript;
		private TimeManager timeScript;


		// Use this for initialization
		void Awake () {
			Application.targetFrameRate = 60;
			if (instance == null)
				instance = this;
			else if (instance != this)
				Destroy(gameObject);	
			DontDestroyOnLoad(gameObject);
			boardScript = GetComponent<BoardManager> ();
			enemyScript = GetComponent<EnemyManager> ();
			timeScript = GetComponent<TimeManager> ();
			InitGame ();
		}

		void InitGame() {
			boardScript.SetupScene ();
		}
		
		// Update is called once per frame
		void Update () {
			
		}

        public int[,] getWallMap() {
            return boardScript.getWallMap();
		}

		public Vector3 getRandomSpawnPosition() {
			return boardScript.getRandomSpawnPosition();
		}

		public Vector3 getRandomIndoorPosition() {
			return boardScript.getRandomIndoorPosition();
		}
    }
}
