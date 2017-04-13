using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SurviveTheNight;

public abstract class Projectile : MonoBehaviour {

    private float scale = .5f;
    public float damage;
    public Player player;
    protected float startSpeed;
    protected float maxSpeed;
    protected float currentSpeed;
    protected int travel_frame_count = 0;
    public AudioClip impactSound;

    void Update() {
        if (currentSpeed < maxSpeed) {
            currentSpeed += (maxSpeed - startSpeed) / 50;
        }
        Vector3 velocity = new Vector3(0, currentSpeed * Time.deltaTime, 0);
        transform.position += transform.rotation * velocity;
        if (travel_frame_count++ == 700 * (1 / currentSpeed))
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("Bullet vector on impact: " + transform.position + " - hit: " + other.tag);

        if (other.tag == "Enemy")
            Destroy(gameObject);

		float volume = 0;
		if (squareDistToPlayer () <= 4) {
			volume = 1f;
		} else {
			double d = 1 - ((4 - squareDistToPlayer ()) * -1 / 100.0);
			if (d < 0) {
				d = 0;
			}
			volume = (float) d;
		}
        playSound(impactSound, volume);
		if (this is Rocket) {
			float shakeMag = 0;
			if (squareDistToPlayer () <= 4) {
				shakeMag = 1f;
			} else {
				double d = 1 - ((4 - squareDistToPlayer ()) * -1 / 50.0);
				if (d < 0) {
					d = 0;
				}
				shakeMag = (float) d;
			}
			GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraMovement> ().Shake (1f, shakeMag);
			if (shakeMag > 0) {
				double dist = squareDistToPlayer ();
				Player p = GameObject.FindWithTag ("Player").GetComponent<Player> ();
				print ("Distance " + dist);
				if (dist <= 1) {
					p.TakeDamage (30f, true);
				} else if (dist <= 2) {
					p.TakeDamage (10f, true);
				} else if (dist <= 3) {
					p.TakeDamage (5f, true);
				}
			}

		}
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

    public void playSound(AudioClip clip, float volume)
    {
        Debug.Log("IMPACT!");
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
        }
    }

    private double squareDist(Vector2 a, Vector2 b) {
        float xDist = a.x - b.x;
        float yDist = a.y - b.y;
        return xDist * xDist + yDist * yDist;
    }

	public double squareDistToPlayer() {
		GameObject playerPerson = GameObject.FindWithTag("Player");
		float xDist = playerPerson.transform.position.x - transform.position.x;
		float yDist = playerPerson.transform.position.y - transform.position.y;
		return xDist * xDist + yDist * yDist;
	}
}
