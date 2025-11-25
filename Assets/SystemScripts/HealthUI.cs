using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    public PlayerStats stats;
    public Image fill;
    public TMP_Text healthText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fill.fillAmount = stats.health / stats.maxHealth;

        healthText.text = ((int)stats.health).ToString() + " / " + ((int)stats.maxHealth).ToString();

    }
}
