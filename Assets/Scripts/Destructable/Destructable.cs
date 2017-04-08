using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

	public abstract class Destructable : MonoBehaviour {

		protected SpriteRenderer spriteRenderer;
		public float integrity;
		protected float startIntegrity;
		public Sprite[] imageList;

		void Awake () {
			startIntegrity = integrity;
			spriteRenderer = GetComponent<SpriteRenderer> ();
		}

		protected void Damage(float amount) {
			integrity -= amount;
			UpdateSprite();
			if (integrity <= 0f)
				Destroy (gameObject);
		}

		protected abstract void UpdateSprite();
	}
}
