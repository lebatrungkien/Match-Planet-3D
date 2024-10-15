using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Slider healthBar;
    public float maxHealth = 100f;
    public float currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        if(healthBar != null){
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakenDamage(float damageAmount){
        currentHealth -= damageAmount;
        
        if(healthBar != null){
            healthBar.value = currentHealth;
        }
        if(currentHealth <= 0){
            Destroy(gameObject);
        }
    }
}
