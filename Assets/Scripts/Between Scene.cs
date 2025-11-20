using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BetweenScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    void OnTriggerEnter (Collider other)
        {
            Debug.Log ("A collider has entered the DoorObject trigger");
            SceneManager.LoadScene (sceneName:"Alley Test");
            
        }
        
    // Update is called once per frame
    void Update()
    {
        


    }
}
