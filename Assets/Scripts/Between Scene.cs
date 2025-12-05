using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BetweenScene : MonoBehaviour
{
    void OnTriggerEnter (Collider other)
        {
            Debug.Log ("A collider has entered the DoorObject trigger");
            FindFirstObjectByType<PlayerStats>().SaveStats();
            SceneManager.LoadScene (sceneName:"Alley Test");
            
        }
        

}
