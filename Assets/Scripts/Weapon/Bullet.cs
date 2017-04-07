using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile {

	float maxSpeed = 7f;
    float scale = .5f;
	int travel_frame_count = 0;

    void Awake() {
        damage = 34f;
    }
	
	// Update is called once per frame
	void Update () {
		Vector3 velocity = new Vector3 (0, maxSpeed * Time.deltaTime, 0);
		transform.position += transform.rotation * velocity;
		if(travel_frame_count++ == 700 * (1 / maxSpeed))
			Destroy (gameObject);
	}

	void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("Bullet vector on impact: " + transform.position + " - hit: " + other.tag);

        if (other.tag == "Enemy")
		    Destroy (gameObject);
	}

    private void OnTriggerStay2D(Collider2D other) {
        //This is probably not the best way to handle it, but
        //This function is to take into account the fact that without it,
        //Bullets get destroyed almost two tiles away from walls
        //We ignore everything except enemies in OnTriggerEnter2D
        //Then we keep checking the actual distance in OnTriggerStay2D, 
        //And if it gets close enough we destroy the bullet
        //Debug.Log("Sq distance: " + squareDist(transform.position, other.transform.position));
        //It seems to need an additional margin of error of about .15
        if (squareDist(transform.position, other.transform.position) <= (scale * scale - .15)) {
            //Debug.Log("Bullet vector on impact: " + transform.position + " - hit: " + other.transform.position);
            Destroy(gameObject);
        }
    }

    private double squareDist(Vector2 a, Vector2 b) {
        float xDist = a.x - b.x;
        float yDist = a.y - b.y;
        return xDist * xDist + yDist * yDist;
    }
}
