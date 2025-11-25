using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Max Values")]
    public float maxHealth = 100f;
    public float maxHunger = 100f;
    public float maxMood = 100f;

    [Header("Current Values")]
    public float health;
    public float hunger;
    public float mood;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Health"))
        {
            LoadStats();
        }
        else
        {
            // First time starting game
            health = maxHealth;
            hunger = 50f;
            mood = maxMood;
        }

    }
    
    
    public void AddHealth(float amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }

    public void AddHunger(float amount)
    {
        hunger = Mathf.Clamp(hunger + amount, 0, maxHunger);
    }

    public void AddMood(float amount)
    {
        mood = Mathf.Clamp(mood + amount, 0, maxMood);
    }



    public void SaveStats()
    {
        PlayerPrefs.SetFloat("Health", health);
        PlayerPrefs.SetFloat("Hunger", hunger);
        PlayerPrefs.SetFloat("Mood", mood);
        PlayerPrefs.Save();
    }

    public void LoadStats()
    {
        health = PlayerPrefs.GetFloat("Health", maxHealth);
        hunger = PlayerPrefs.GetFloat("Hunger", maxHunger);
        mood = PlayerPrefs.GetFloat("Mood", maxMood);
    }
}
