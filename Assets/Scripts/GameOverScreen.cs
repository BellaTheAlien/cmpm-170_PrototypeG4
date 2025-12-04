using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void Setup(float health){
        if (health <= 0){
            gameObject.SetActive(true);
        }
    }

    public void RestartButton(){
        SceneManager.LoadScene("Redone Bedroom");
    }
}
