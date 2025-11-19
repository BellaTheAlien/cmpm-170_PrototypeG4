using UnityEngine;
using UnityEngine.UI;

public class HungerUI : MonoBehaviour
{
    public PlayerStats stats;
    public Image fill;

    private void Update()
    {
        fill.fillAmount = stats.hunger / stats.maxHunger;
    }
}
