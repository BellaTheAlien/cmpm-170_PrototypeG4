using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoodUI : MonoBehaviour
{
    public PlayerStats stats;
    public Image fill;
    

    void Update()
    {
        fill.fillAmount = stats.mood / stats.maxMood;
        
    }
}
