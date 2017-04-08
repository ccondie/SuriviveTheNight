using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurviveTheNight {

	public abstract class Destructable : MonoBehaviour {

		protected SpriteRenderer spriteRenderer;
		public float integrity;
		protected float startIntegrity;
		public Sprite[] imageList;
		private int x;
		private int y;

		void Awake () {
			startIntegrity = integrity;
			spriteRenderer = GetComponent<SpriteRenderer> ();
		}

		protected void Damage(float amount) {
			integrity -= amount;
			UpdateSprite();
			if (integrity <= 0f) {
				BoardManager.Instance.clearWall (x, y);
				Destroy (gameObject);
			}
		}

		protected abstract void UpdateSprite();

		public void SetCoords(int x, int y) {
			this.x = x;
			this.y = y;
		}
	}
}
