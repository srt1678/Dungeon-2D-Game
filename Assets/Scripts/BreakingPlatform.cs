using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    public Sprite breakPlatformOne;
    public Sprite breakPlatformTwo;
    private SpriteRenderer breakPlatformSpriteRenderer;
    public Rigidbody2D rb;
    private float amount = 6f;
    private float speed = 6f;
    private bool startShaking;
    private bool firstTime = true;
    Vector3 startOffPos;
    public AudioSource breakingPlatformSound;
    PolygonCollider2D objectCollider;

    // Start is called before the first frame update
    void Start()
    {
        breakPlatformSpriteRenderer = GetComponent<SpriteRenderer>();
        startShaking = false;
        if (firstTime)
        {
            startOffPos = transform.position;
        }
        breakingPlatformSound = GetComponent<AudioSource>();
        objectCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startShaking)
        {
            Vector3 newPos = Random.insideUnitSphere * (Time.deltaTime * amount);
            newPos.y = newPos.y + transform.position.y;
            newPos.x = transform.position.x;
            newPos.z = transform.position.z;
            transform.position = newPos;
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.tag == "Player")
        {
            if (objectCollider.isTrigger)
            {
                objectCollider.isTrigger = false;
                Shaking();
                Invoke("changeBreakSprite", 1f);
            }
        }
    }


    public void Shaking()
    {
        StartCoroutine("ShakeNow");
    }

    IEnumerator ShakeNow()
    {
        Vector3 originalPos = transform.position;
        if(startShaking == false)
        {
            startShaking = true;
        }
        yield return new WaitForSeconds(3f);
        startShaking = false;
        transform.position = originalPos;
    }

    private void changeBreakSprite()
    {
        rb.velocity = -transform.up * speed;
        breakingPlatformSound.Play();
        breakPlatformSpriteRenderer.sprite = breakPlatformTwo;
        Invoke("destroyPlatform", 0.2f);
    }

    private void destroyPlatform()
    {
        gameObject.SetActive(false);
        firstTime = false;
        Invoke("resetCollider", 0.5f);
    }
    
    public void respawnPlatform()
    {
        breakPlatformSpriteRenderer.sprite = breakPlatformOne;
        gameObject.SetActive(true);
        transform.position = startOffPos;
        startShaking = false;
    }

    private void resetCollider()
    {
        objectCollider.isTrigger = true;
    }
}
