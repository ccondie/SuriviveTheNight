using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    float maxSpeed = 3f;
    public float damage = 100f;
    float scale = .5f;
    int travel_frame_count = 0;

    // Update is called once per frame
    void Update() {
        Vector3 velocity = new Vector3(0, maxSpeed * Time.deltaTime, 0);
        transform.position += transform.rotation * velocity;
        if (travel_frame_count++ == 700 * (1 / maxSpeed))
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("Bullet vector on impact: " + transform.position + " - hit: " + other.tag);

        if (other.tag == "Enemy")
            Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other) {
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
