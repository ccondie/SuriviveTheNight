using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

	public class EnemyManager : Singleton<EnemyManager> {

		public GameObject powerup;
		public GameObject enemy;
        public List<GameObject> enemies = new List<GameObject>();
        public int ZombieCount = 20;

		public int maxEnemies;
		private bool indoorEnemyGeneration = false;

		protected EnemyManager () {}

		// Use this for initialization
		void Start () {
			for (int i = 0; i < ZombieCount; i++) {
				Spawn ();
			}
		}

		void Spawn() {
			if(enemies.Count < maxEnemies)
				enemies.Add(Instantiate (enemy, BoardManager.Instance.getRandomSpawnPosition(), Quaternion.identity));
		}

		public void SpawnAtEdge() {
			if(enemies.Count < maxEnemies)
				enemies.Add(Instantiate (enemy, BoardManager.Instance.getRandomEdgePosition(), Quaternion.identity));
		}

		public void SpawnAtLocation(Vector3 position) {
			enemies.Add(Instantiate (enemy, position, Quaternion.identity));
		}
			
		public void PowerUpAtLocation(Vector3 position) {
			Instantiate (powerup, position, Quaternion.identity);
		}
		
		// Update is called once per frame
		void Update () {
			enemies.RemoveAll(e => e == null);
		}

		public void BeginIndoorEnemyGeneration() {
			indoorEnemyGeneration = true;
		}

		public bool GenerateEnemiesIndoors() {
			return indoorEnemyGeneration;
		}
	}

}