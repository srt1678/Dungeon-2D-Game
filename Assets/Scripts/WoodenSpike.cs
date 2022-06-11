using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenSpike : MonoBehaviour
{
    public Sprite woodenSpikeOne;
    public Sprite woodenSpikeTwo;
    private SpriteRenderer woodenSpikeRenderer;
    public Rigidbody2D rb;
    private float speed = 2f;
    private bool firstTime = true;
    Vector3 startOffPos;
    public AudioSource woodenSpikesSound;

    // Start is called before the first frame update
    void Start()
    {
        woodenSpikeRenderer = GetComponent<SpriteRenderer>();
        if (firstTime)
        {
            startOffPos = transform.position;
        }
        woodenSpikesSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void breakSpike()
    {
        rb.velocity = -transform.up * speed;
        woodenSpikeRenderer.sprite = woodenSpikeTwo;
        woodenSpikesSound.Play();
        Invoke("destroySpike", 0.2f);
    }

    private void destroySpike()
    {
        gameObject.SetActive(false);
        firstTime = false;
    }

    public void respawnSpike()
    {
        woodenSpikeRenderer.sprite = woodenSpikeOne;
        gameObject.SetActive(true);
        transform.position = startOffPos;
    }
}
