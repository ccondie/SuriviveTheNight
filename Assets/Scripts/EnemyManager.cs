using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

	public class EnemyManager : MonoBehaviour {

		public GameObject enemy;
        public List<GameObject> enemies = new List<GameObject>();

		public float MinX = 0;
		public float MaxX = 60;
		public float MinY = 0;
		public float MaxY = 60;

		// Use this for initialization
		void Start () {
			for (int i = 0; i < 20; i++) {
				Spawn ();
			}
		}

		void Spawn() {
			//float x = Random.Range(MinX,MaxX);
			//float y = Random.Range(MinY,MaxY);
			//float z = 0;
			enemies.Add(Instantiate (enemy, GameManager.instance.getRandomSpawnPosition(), Quaternion.identity));
		}


		
		// Update is called once per frame
		void Update () {
			
		}
	}

}