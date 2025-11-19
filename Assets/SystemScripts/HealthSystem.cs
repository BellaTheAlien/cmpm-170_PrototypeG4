using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public PlayerStats stats;

    [Header("Regeneration")]
    public bool enableRegen = false;
    public float regenPerSecond = 0f;

    [Header("Death")]
    public bool destroyOnDeath = false;

    private void Update()
    {
        if (enableRegen && stats.health > 0)
        {
            stats.AddHealth(regenPerSecond * Time.deltaTime);
        }

        // Check death
        if (stats.health <= 0)
        {
            Debug.Log("Player died!");

            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            
        }
    }

    
    public void TakeDamage(float amount)
    {
        stats.AddHealth(-amount);
    }
    //healing maybe will be used after certain conditions? idk
    public void Heal(float amount)
    {
        stats.AddHealth(amount);
    }
}