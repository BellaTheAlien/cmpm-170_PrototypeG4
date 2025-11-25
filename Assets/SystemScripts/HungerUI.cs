using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HungerUI : MonoBehaviour
{
    public PlayerStats stats;
    public Image fill;
    public TMP_Text hungerText;

    private void Update()
    {
        fill.fillAmount = stats.hunger / stats.maxHunger;
        hungerText.text = ((int)stats.hunger).ToString() + " / " + ((int)stats.maxHunger).ToString();
    }
}
