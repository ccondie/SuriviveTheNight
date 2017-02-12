using System.Collections;
using UnityEngine;

namespace SurviveTheNight {

	public class Loader : MonoBehaviour {

		public GameObject gameManager;

		void Awake () {
			if (GameManager.instance == null)

				//Instantiate gameManager prefab
				Instantiate(gameManager);
		}
	}

}