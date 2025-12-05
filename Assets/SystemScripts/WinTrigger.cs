using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public GameObject winPanel;


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerStats>() != null)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0f;  // freeze game
        }
    }

  
}
