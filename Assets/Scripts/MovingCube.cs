using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : MonoBehaviour {
    public Transform pos1, pos2, pos3, pos4, pos5, pos6;
    public float speed;
    public Transform startPos;
    Vector3 nextPos;
    public bool startMoving;
    public LevelManager gameLevelManager;

    public bool moveToNext;
    public bool moveToNext2;

    public float orginalSpeed;

    public ParticleSystem cubeMovingDust1;
    public ParticleSystem cubeMovingDust2;
    public ParticleSystem cubeMovingDust3;
    public ParticleSystem cubeMovingDust4;
    public ParticleSystem cubeMovingDust5;
    public ParticleSystem cubeMovingDust6;
    public bool particle1;
    public bool particle2;
    public bool particle3;
    public bool particle4;
    public bool particle5;
    public bool particle6;


    public AudioSource movingCubeSound;
    public bool cubeNum1;



    // Start is called before the first frame update
    void Start()
    {
        nextPos = startPos.position;
        startMoving = false;
        gameLevelManager = FindObjectOfType<LevelManager>();
        moveToNext = false;
        moveToNext2 = false;
        orginalSpeed = speed;

        particle1 = false;
        particle2 = false;
        particle3 = false;
        particle4 = false;
        particle5 = false;
        particle6 = false;


        movingCubeSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startMoving)
        {
            playDust();
            if (transform.position == pos1.position)
            {
                particle1 = true;
                particle2 = true;
                nextPos = pos2.position;

                movingCubeSound.Play();
            }
            if ((transform.position == pos2.position) && !moveToNext)
            {
                particle1 = false;
                particle2 = false;
                freeze();


                movingCubeSound.Pause();
            }
            if (moveToNext)
            {
                nextPos = pos3.position;


                movingCubeSound.UnPause();
            }
            if (transform.position == pos3.position)
            {
                moveToNext = false;
                nextPos = pos4.position;
                particle1 = false;
                particle2 = false;
                particle3 = true;
                particle4 = true;
            }
            if (transform.position == pos4.position)
            {
                nextPos = pos5.position;
                particle3 = false;
                particle4 = false;
                particle5 = true;
                particle6 = true;
            }
            if ((transform.position == pos5.position) && !moveToNext2)
            {
                particle5 = false;
                particle6 = false;
                freeze();
            }
            if (moveToNext2)
            {
                nextPos = pos6.position;
            }
            if (transform.position == pos6.position)
            {
                moveToNext2 = false;
                particle5 = false;
                particle6 = false;
                
                movingCubeSound.Stop();
            }
            transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
        }
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pos1.position, pos2.position);
        Gizmos.DrawLine(pos2.position, pos3.position);
        Gizmos.DrawLine(pos3.position, pos4.position);
        Gizmos.DrawLine(pos4.position, pos5.position);
        Gizmos.DrawLine(pos5.position, pos6.position);
    }
    */

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.collider.transform.SetParent(transform);
            gameLevelManager.startChoicePlatform();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.collider.transform.SetParent(null);
        }
    }

    public void resetPlatform()
    {
        transform.position = pos1.position;
        startMoving = false;
    }

    private void freeze()
    {
        Invoke("unfreeze", 1.5f);
    }

    private void unfreeze()
    {
        if (transform.position == pos2.position)
        {
            moveToNext = true;
            particle1 = true;
            particle2 = true;
        }
        if (transform.position == pos5.position)
        {
            moveToNext2 = true;
            particle5 = true;
            particle6 = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SlowingPlatformTrigger")
        {
            speed = 2.95f;
        }
        if (collision.gameObject.tag == "SpeedUpPlatformTrigger")
        {
            speed = orginalSpeed;
        }
        if (collision.gameObject.tag == "CubeStop1")
        {
            particle1 = false;
            particle5 = false;
        }
        if (collision.gameObject.tag == "CubeStop2")
        {
            particle2 = false;
        }
        if (collision.gameObject.tag == "CubeStart1")
        {
            particle1 = true;
            particle2 = true;
        }
        if (collision.gameObject.tag == "CubeStart2")
        {
            particle5 = true;
        }
    }

    private void playDust()
    {
        if (particle1)
        {
            cubeMovingDust1.Play();
        }
        else if(!particle1)
        {
            cubeMovingDust1.Stop();
        }
        if (particle2)
        {
            cubeMovingDust2.Play();
        }
        else if(!particle2)
        {
            cubeMovingDust2.Stop();
        }
        if (particle3)
        {
            cubeMovingDust3.Play();
        }
        else if(!particle3)
        {
            cubeMovingDust3.Stop();
        }
        if (particle4)
        {
            cubeMovingDust4.Play();
        }
        else if (!particle4)
        {
            cubeMovingDust4.Stop();
        }
        if (particle5)
        {
            cubeMovingDust5.Play();
        }
        else if (!particle5)
        {
            cubeMovingDust5.Stop();
        }
        if (particle6)
        {
            cubeMovingDust6.Play();
        }
        else if (!particle6)
        {
            cubeMovingDust6.Stop();
        }
    }

    public void resetParticle()
    {
        particle1 = false;
        particle2 = false;
        particle3 = false;
        particle4 = false;
        particle5 = false;
        particle6 = false;
    }

}
