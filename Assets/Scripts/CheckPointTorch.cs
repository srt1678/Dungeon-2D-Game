using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CheckPointTorch : MonoBehaviour
{
    private SpriteRenderer checkpointSpriteRenderer;
    private Animator anim;
    private float delayTorch = 1f;

    //Torch & the torch's lightning
    public GameObject torch;
    private Light2D lightning;
    //The lightning duration = torch lights on animation duration
    private float torchLightDuration = 0.4f;
    private float maxIntensity = 1.27f;
    private float minIntensity = 0.8f;
    private float flickerSpeed = 0.3f;
    private bool flickering = false;
    private bool changeTorchLight;

    public ParticleSystem torchSmoke;

    public AudioClip lightTorchSound;
    //public AudioClip keepTorchSound;


    // Start is called before the first frame update
    void Start()
    {
        checkpointSpriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        changeTorchLight = false;

        lightning = torch.GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (changeTorchLight)
        {
            if(lightning.intensity < maxIntensity)
            {
                torchLightDuration -= Time.deltaTime;
                lightning.intensity += 0.01f;
            }
        }
    }


    public void TorchAction()
    {
        //activate chaning torch animation
        //use Invoke to delay and slow down the animation
        anim.SetBool("LightTorch", true);
        Invoke("KeepTorch", delayTorch);
        changeTorchLight = true;
        torchSmoke.Play();
    }

    private void KeepTorch()
    {
        StartCoroutine(DoFlicker());
        anim.SetBool("KeepTorch", true);


        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        audio.volume = 0.25f;


        changeTorchLight = false;
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

    private void lightTorchAudio()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = 0.25f;
        audio.PlayOneShot(lightTorchSound);
    }

    public void PauseAudio(string onOff)
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (onOff == "on")
        {
            audio.volume = 0.5f;
        }
        else if (onOff == "off")
        {
            audio.volume = 0.0f;
        }
    }
}
