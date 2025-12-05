using UnityEngine;

public class StarPickup : MonoBehaviour
{
    public float moodValue = 1f;
    private void OnTriggerEnter(Collider other)
    {
        PlayerStats stats = other.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.AddMood(moodValue);
            stats.SaveStats(); 
            Destroy(gameObject);  
        }
    }
}
