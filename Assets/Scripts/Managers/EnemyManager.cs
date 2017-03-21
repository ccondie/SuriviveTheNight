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

		// This is initially false to prevent spawning enemies in the same room as you
		// at the start of the game
		private bool indoorEnemyGeneration = false;

		private int spawnCheck = 0;
		public int spawnDelay;

		private TimeManager tm;
		private BoardManager bm;

		protected EnemyManager () {}

		// Use this for initialization
		void Start () {
			tm = TimeManager.Instance;
			bm = BoardManager.Instance;
			SpawnZombiesAtStartUp ();
		}

		// Update is called once per frame
		void Update () {
			// This removes all destroyed enemies from the list of active enemies
			enemies.RemoveAll(e => e == null);

			SpawnZombiesAtNight ();
		}

		void SpawnZombiesAtStartUp() {
			for (int i = 0; i < ZombieCount; i++)
				Spawn ();
		}

		void SpawnZombiesAtNight() {
			if (tm.IsNight () && spawnCheck++ % spawnDelay == 0)
				SpawnAtEdge ();
		}

		void Spawn() {
			if(enemies.Count < maxEnemies)
				enemies.Add(Instantiate (enemy, bm.getRandomSpawnPosition(), Quaternion.identity));
		}

		public void SpawnAtEdge() {
			if(enemies.Count < maxEnemies)
				enemies.Add(Instantiate (enemy, bm.getRandomEdgePosition(), Quaternion.identity));
		}

		public void SpawnAtLocation(Vector3 position) {
			enemies.Add(Instantiate (enemy, position, Quaternion.identity));
		}
			
		public void PowerUpAtLocation(Vector3 position) {
			Instantiate (powerup, position, Quaternion.identity);
		}
			
		public void BeginIndoorEnemyGeneration() {
			indoorEnemyGeneration = true;
		}

		public bool GenerateEnemiesIndoors() {
			return indoorEnemyGeneration;
		}
	}

}