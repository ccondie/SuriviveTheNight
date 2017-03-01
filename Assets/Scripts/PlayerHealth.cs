using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	public float startingHealth = 100f;
	public float currentHealth;
	public Slider healthSlider;

    // *******************************************************************************************************
    //      STAMINA RELATED VARIABLES - maybe move to "player" or some new Stamina exclusive bar object
    // *******************************************************************************************************
    public float startingStamina = 100f;
	public float currentStamina;
	public Slider staminaSlider;
    private Image staminaFill;
    public Color staminaBlue = new Color((0f / 255f), (114f / 255f), (188f / 255f), 1.0f);
    public Color staminaRed = new Color((158f / 255f), (11f / 255), (15f / 255f), 1.0f);

    public float staminaGain = 0.4f;       
    public float staminaGainDelay = 0.03f;   // should update about 10 times a second
    private float staminaGainDelay_Cur;
    

    //public Image damageImage;
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
        staminaFill = staminaSlider.GetComponentsInChildren<Image>()[1];
        staminaGainDelay_Cur = staminaGainDelay;
    }
		
	// Update is called once per frame
	void Update () {
        //if (damaged) {
        //	damageImage.color = flashColor;
        //} else {
        //	damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        //}
        //damaged = false;

        // update current stamina on slider


        // if enough time has past (fractions of a second) to increase the stamina, increase it
        staminaGainDelay_Cur -= Time.deltaTime;
        if (staminaGainDelay_Cur < 0)
        {
            IncreaseStamina(staminaGain);
            staminaGainDelay_Cur = staminaGainDelay;
        }

        staminaSlider.value = currentStamina;
        if (currentStamina / startingStamina < 0.2f)
            staminaFill.color = staminaRed;
        else
            staminaFill.color = staminaBlue;
    }

	public void TakeDamage (float amount) {
		damaged = true;
		currentHealth -= amount;
		healthSlider.value = currentHealth;
		//playerAudio.Play ();
		if (currentHealth <= 0 && !isDead) {
			Death ();
		}
	}

	public void DecreaseStamina (float amount) {
        if(amount > currentStamina)
        {
            currentStamina = 0;
        }
        else
        {
            currentStamina -= amount;
        }
    }

    private void IncreaseStamina(float amount)
    {
        // only increase stamina if the stamina if it won't push the stamina above max
        float missingStamina = startingStamina - currentStamina;
        if(amount > missingStamina)
        {
            // if the amount would overflow to maxStamina, set to max stamina
            currentStamina = startingStamina;
        }
        else
        {
            // otherwise add the amount of stamina to the current stamina
            currentStamina += amount;
        }
        
    }

	void Death () {
		isDead = true;

		//anim.SetTrigger ("Die");

		//playerAudio.clip = deathClip;
		//playerAudio.Play ();
	}
}
