using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace SurviveTheNight {

	using System.Collections.Generic;
	using UnityEngine.UI;

	public class GameManager : MonoBehaviour {

		public static GameManager instance = null;

		// Use this for initialization
		void Awake () {
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
