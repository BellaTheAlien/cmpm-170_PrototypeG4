using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public PlayerStats stats;
    public GameObject winTriggerPlane;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winTriggerPlane.SetActive(false);//starts inactive
    }

    // Update is called once per frame
    void Update()
    {
        if(stats.mood >= stats.maxMood)
        {
            winTriggerPlane.SetActive(true);//activate win trigger plane
        }
        
    }
}
