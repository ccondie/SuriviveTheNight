using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

	public class Roof : MonoBehaviour {

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
				Destroy (gameObject);
			}
		}
	}

}