using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformChoice : MonoBehaviour
{
    public Transform pos1, pos2, pos3, pos4, pos5, pos6;
    public float speed;
    public Transform startPos;
    Vector3 nextPos;
    public bool startMoving;
    public LevelManager gameLevelManager;

    public bool moveToNext;
    public bool moveToNext2;

    public float orginalSpeed;

    

    // Start is called before the first frame update
    void Start()
    {
        nextPos = startPos.position;
        startMoving = false;
        gameLevelManager = FindObjectOfType<LevelManager>();
        moveToNext = false;
        moveToNext2 = false;
        orginalSpeed = speed;
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
            if(transform.position == pos2.position)
            {
                freeze();
            }
            if (moveToNext)
            {
                nextPos = pos3.position;
            }
            if (transform.position == pos3.position)
            {
                moveToNext = false;
                nextPos = pos4.position;
            }
            if (transform.position == pos4.position)
            {
                nextPos = pos5.position;
            }
            if (transform.position == pos5.position)
            {
                freeze();
            }
            if (moveToNext2)
            {
                nextPos = pos6.position;
            }
            if (transform.position == pos6.position)
            {
                moveToNext2 = false;
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
        }
        if (transform.position == pos5.position)
        {
            moveToNext2 = true;
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
    }
}
