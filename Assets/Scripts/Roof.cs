using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SurviveTheNight {

	public class Roof : MonoBehaviour {

		public bool spawnLocation;
		protected bool marked = false;

		void OnTriggerEnter2D(Collider2D other) {
			if (other.tag == "Player")
				LightRoom ();
		}

		void LightRoom() {
			if (!marked) {
				marked = true;
				GameObject[] neighbors = BoardManager.Instance.getRoofNeighbors (transform.position);
				for (int i = 0; i < neighbors.Length; i++)
					if (neighbors [i] != null)
						neighbors [i].GetComponent<Roof> ().LightRoom ();
				int spawnItem = Random.Range (0, 20);
				if (spawnLocation && EnemyManager.Instance.GenerateEnemiesIndoors ()) {
					switch (spawnItem) {
					case 7:
						EnemyManager.Instance.PowerUpAtLocation (transform.position);
						break;
					case 13:
						EnemyManager.Instance.SpawnAtLocation (transform.position);
						break;
					}
				}
				Destroy (gameObject);
			}
		}
	}

}