using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BetweenAlley : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    void OnTriggerEnter (Collider other)
        {
            Debug.Log ("A collider has entered the DoorObject trigger");
            FindFirstObjectByType<PlayerStats>().SaveStats();
            SceneManager.LoadScene (sceneName:"Alley Part 2");
            
        }
        
    // Update is called once per frame
    void Update()
    {
        


    }
}
