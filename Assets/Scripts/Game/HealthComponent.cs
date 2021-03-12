using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HealthComponent : NetworkBehaviour
{ 
    public const int maxHealth = 40;
    [SyncVar(hook = "OnHealthChange")]
    public int currentHealth = maxHealth;
    public Slider healthBar;

    void Start() {
        if (healthBar == null)
            return;
        healthBar.maxValue = maxHealth;
    }

    public void TakenDamage(int damage) {
        if (!isServer) {
            return;
        }
        currentHealth -= damage;
        if (currentHealth <= 0) {
            currentHealth = 0;
           Destroy(gameObject);
        }
    }

    void OnHealthChange(int currentHealth) {
        if (healthBar == null)
            return;
        healthBar.value = currentHealth;
    }
}
