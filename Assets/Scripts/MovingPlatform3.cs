using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform3 : MonoBehaviour
{
    public Transform pos1, pos2;
    public float speed;
    public Transform startPos;
    Vector3 nextPos;
    private bool startMoving;

    public cameraFreeze vcam;
    private float originalSpeed;
    public bool firstStep;

    public AudioSource platformChargeSound;
    public AudioClip speedUpSound;
    private bool firstBurstSound;

    public ParticleSystem platformDust;
    public int triggerCount;



    // Start is called before the first frame update
    void Start()
    {
        nextPos = startPos.position;
        startMoving = false;

        vcam = FindObjectOfType<cameraFreeze>();
        originalSpeed = speed;

        firstStep = true;
        platformChargeSound = GetComponent<AudioSource>();
        firstBurstSound = true;

        triggerCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (startMoving)
        {
            if (transform.position == pos1.position)
            {
                nextPos = pos2.position;
            }
            transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
        }
        if(transform.position == pos2.position)
        {
            platformDust.Stop();
        }
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pos1.position, pos2.position);
    }
    */

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (firstStep)
            {
                if (firstBurstSound)
                {
                    speedUpAudio();
                }
                platformDust.Play();
                firstBurstSound = false;
                platformChargeSound.Play();
                collision.collider.transform.SetParent(transform);
                startMoving = true;
                vcam.uncheckLookAheadY();
                vcam.increaseYDamping();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.collider.transform.SetParent(null);
        }
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if(trigger.gameObject.tag == "SpeedUpPlatform")
        {
            if (triggerCount % 2 == 1)
            {
                speed += 18f;
                speedUpAudio();
                platformChargeSound.volume += 0.15f;
            }
            triggerCount += 1;
        }

        if (trigger.gameObject.tag == "CancelLookAhead")
        {
            fadeOutChargeSound();
            vcam.checkLookAheadY();
            resetSpeed();
            vcam.decreaseYDamping();
            firstStep = false;
        }
    }


    public void resetPlatform()
    {
        transform.position = pos1.position;
        startMoving = false;
    }

    public void resetSpeed()
    {
        speed = originalSpeed;
        firstBurstSound = true;
    }

    private void speedUpAudio()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(speedUpSound);
    }


    public void fadeOutChargeSound()
    {
        StartCoroutine(FadeOut(platformChargeSound, 3f));
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
