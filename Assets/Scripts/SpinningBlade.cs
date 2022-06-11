using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningBlade : MonoBehaviour
{
    public AudioSource spinningBladeAudio;
    // Start is called before the first frame update
    void Start()
    {
        spinningBladeAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseAudio(string onOff)
    {
        if (onOff == "on")
        {
            spinningBladeAudio.UnPause();
        }
        else if (onOff == "off")
        {
            spinningBladeAudio.Pause();
        }
    }
}
