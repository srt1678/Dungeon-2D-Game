using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class NormalTorch : MonoBehaviour
{
    public GameObject torch;
    private Light2D lightning;

    private float maxIntensity = 1.27f;
    private float minIntensity = 0.8f;
    private float flickerSpeed = 0.3f;
    private bool flickering = false;

    public ParticleSystem torchSmoke;
    public AudioSource torchAudio;

    
    // Start is called before the first frame update
    void Start()
    {
        lightning = torch.GetComponent<Light2D>();
        StartCoroutine(DoFlicker());
        torchSmoke.Play();

        torchAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DoFlicker()
    {
        flickering = true;
        while (flickering)
        {
            lightning.intensity = Random.Range(minIntensity, maxIntensity);
            yield return new WaitForSeconds(flickerSpeed);
        }
        flickering = false;
    }

    public void PauseAudio(string onOff)
    {
        if(onOff == "on")
        {
            torchAudio.UnPause();
        }else if(onOff == "off")
        {
            torchAudio.Pause();
        }
    }
}
