using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	float maxSpeed = 5f;
	
	// Update is called once per frame
	void Update () {
		Vector3 velocity = new Vector3 (0, maxSpeed * Time.deltaTime, 0);
		transform.position += transform.rotation * velocity;
	}

	void OnTriggerEnter2D(Collider2D other) {
		Destroy (gameObject);
	}
}
