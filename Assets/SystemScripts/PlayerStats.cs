using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [Header("Max Values")]
    public float maxHealth = 100f;
    public float maxHunger = 100f;
    public float maxMood = 100f;

    [Header("Current Values")]
    public float health;
    public float hunger;
    public float mood = 0f;

    private void Awake()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        bool isStartingScene = currentScene == "Redone Bedroom";

        if (isStartingScene)
        {
            ResetStats();
        }
        else if (PlayerPrefs.HasKey("Health"))
        {
            LoadStats();
        }
        else
        {
            ResetStats();  
        }
        if (currentScene == "Alley Test")
        {
            // Apply fall damage once when entering this scene
            GetComponent<HealthSystem>().ApplyFallDamage(40f);
        }

        
    }
    public void Start()
    {
        PlayerPrefs.DeleteAll();
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

    private void ResetStats()
    {
        PlayerPrefs.DeleteKey("Health");
        PlayerPrefs.DeleteKey("Hunger");
        PlayerPrefs.DeleteKey("Mood");

        health = maxHealth;
        hunger = 50f;
        mood = 0f;

        SaveStats();
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
        mood = PlayerPrefs.GetFloat("Mood", 0f);
    }
}
