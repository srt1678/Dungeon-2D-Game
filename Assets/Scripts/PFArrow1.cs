using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFArrow1 : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public GameObject impactEffect;
    public Sprite arrowTwo;
    private SpriteRenderer arrowSpriteRenderer;

    public AudioClip arrowAudio;

    // Start is called before the first frame update
    void Start()
    {
        //Setting arrow's flying speed
        rb.velocity = transform.up * speed;
        arrowSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if(hitInfo.tag == "Ground")
        {
            arrowSound();
            breakArrow();
        }else if (hitInfo.tag == "Player")
        {
            attackArrow();
        }
    }

    //Destroy arrow when attack
    public void attackArrow()
    {
        Destroy(gameObject);
    }

    public void breakArrow()
    {
        rb.velocity = -transform.right * speed / 4;
        arrowSpriteRenderer.sprite = arrowTwo;
        Invoke("attackArrow", 0.2f);
    }


    private void arrowSound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = 0.15f;
        audio.PlayOneShot(arrowAudio);
    }
}
