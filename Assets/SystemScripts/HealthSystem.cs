using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HealthSystem : MonoBehaviour
{
    public PlayerStats stats;
    public GameOverScreen GameOverScreen;

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
            GameOverScreen.Setup(stats.health);
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


    public void ApplyFallDamage(float amount)
    {
        StartCoroutine(DelayedFallDamage(amount));
    }

    public void gameOver(){
        GameOverScreen.Setup(stats.health);
    }

    private IEnumerator DelayedFallDamage(float amount)
    {
        yield return new WaitForSeconds(0.5f);  
        TakeDamage(amount);
        Debug.Log("Fall damage applied: " + amount);
    }
}