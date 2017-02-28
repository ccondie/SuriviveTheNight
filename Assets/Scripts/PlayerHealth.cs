using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	public int startingHealth = 100;
	public int currentHealth;
	public Slider healthSlider;
	public int startingStamina = 100;
	public int currentStamina;
	public Slider staminaSlider;
	public Image damageImage;
	//public AudioClip deathClip;
	public float flashSpeed = 5f;
	public Color flashColor = new Color(1f, 0f, 0f, 0.1f); //RED

	//Animator anim;
	//AudioSource playerAudio;
	bool isDead;
	bool damaged;

	void Awake() {
		//anim = GetComponent <Animator> ();
		//playerAudio = GetComponent <AudioSource> ();
		currentHealth = startingHealth;
		currentStamina = startingStamina;
	}
		
	// Update is called once per frame
	void Update () {
		if (damaged) {
			damageImage.color = flashColor;
		} else {
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}
		damaged = false;
	}

	public void TakeDamage (int amount) {
		damaged = true;
		currentHealth -= amount;
		healthSlider.value = currentHealth;
		//playerAudio.Play ();
		if (currentHealth <= 0 && !isDead) {
			Death ();
		}
	}

	public void DecreaseStamina (int amount) {
		currentStamina -= amount;
		staminaSlider.value = currentStamina;
	}

	void Death () {
		isDead = true;

		//anim.SetTrigger ("Die");

		//playerAudio.clip = deathClip;
		//playerAudio.Play ();
	}
}
