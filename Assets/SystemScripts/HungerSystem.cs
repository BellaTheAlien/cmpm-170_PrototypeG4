using UnityEngine;

public class HungerSystem : MonoBehaviour
{
    public PlayerStats stats;
    public float drainPerSecond = 1f;

    private void Update()
    {
        stats.AddHunger(-drainPerSecond * Time.deltaTime);

        if (stats.hunger <= 0)
            stats.AddHealth(-2f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            stats.AddHunger(+100);
            Destroy(other.gameObject);
        }
    }
}
