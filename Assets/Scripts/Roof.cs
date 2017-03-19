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
				if(spawnLocation 
					&& Random.Range (0, 20) == 13
					&& EnemyManager.Instance.GenerateEnemiesIndoors())
					EnemyManager.Instance.SpawnAtLocation (transform.position);
				Destroy (gameObject);
			}
		}
	}

}