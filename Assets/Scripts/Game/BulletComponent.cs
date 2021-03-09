using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    public int damage = 10;

    void OnCollisionEnter(Collision collision) {
        var hit = collision.gameObject;
        var health = hit.GetComponent<HealthComponent>();
        if (health != null) {
            health.TakenDamage(damage);
        }
        Destroy(gameObject);
    }
}
