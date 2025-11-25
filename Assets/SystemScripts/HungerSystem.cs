using UnityEngine;

public class HungerSystem : MonoBehaviour
{
    public PlayerStats stats;
    public float drainPerSecond = 2f;

    public AudioSource eatSound;

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
            if (eatSound != null) eatSound.Play();

            Destroy(other.gameObject);
        }
    }
}
