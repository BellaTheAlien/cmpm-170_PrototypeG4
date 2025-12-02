using UnityEngine;
using System.Collections;

public class meowLogic : MonoBehaviour
{
    public AudioSource meowAudio;
    // Update is called once per frame
    // Player can press "e" to make the cat meow
    // "e" key will trigger "cutMeow.mp3" sound effect
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)){
            //meowAudio = Resources.Load ("Assets/music/cutMeows.mp3") as AudioSource;
            meowAudio.Play();
        }
    }
}
