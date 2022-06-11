using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    public float respawnDelay;
    public PlayerMovement gamePlayer;
    public CheckPointController checkPoint;
    public DeathTransition blackTransition;
    
    public BreakingPlatform breakingPlatform;
    public GameObject[] breakingPlatformArray;
    
    public WoodenSpike woodenSpike;
    public GameObject[] woodenSpikeArray;

    public MovingPlatform3 movingPlatform3;
    public GameObject[] movingPlatform3Array;

    public MovingPlatformChoice movingPlatformChoice;
    public GameObject[] movingPlatformChoiceArray;

    public MovingCube movingCube;
    public GameObject[] movingCubeArray;

    //Cinemachine
    public cameraFreeze vcam;


    void Start()
    {
        gamePlayer = FindObjectOfType<PlayerMovement>();
        checkPoint = FindObjectOfType<CheckPointController>();
        blackTransition = FindObjectOfType<DeathTransition>();

        breakingPlatformArray = GameObject.FindGameObjectsWithTag("BreakingPlatform");
        woodenSpikeArray = GameObject.FindGameObjectsWithTag("WoodenSpike");
        movingPlatform3Array = GameObject.FindGameObjectsWithTag("MovingPlatform3");
        movingPlatformChoiceArray = GameObject.FindGameObjectsWithTag("PlatformChoice");

        vcam = FindObjectOfType<cameraFreeze>();

        movingCubeArray = GameObject.FindGameObjectsWithTag("MovingCube");
    }

    void Update()
    {
    }

    public void Respawn()
    {
        StartCoroutine("RespawnCoroutine");
    }

    public IEnumerator RespawnCoroutine()
    {
        blackTransition.StartTransition();
        //to delay the respawn
        yield return new WaitForSeconds(respawnDelay);
        gamePlayer.gameObject.SetActive(false);
        //Respawn player based on the respawn point in the array
        if(gamePlayer.arrayIndex == 0)
        {
            gamePlayer.transform.position = new Vector3(
                gamePlayer.checkPointArray[gamePlayer.arrayIndex].x,
                gamePlayer.checkPointArray[gamePlayer.arrayIndex].y,
                gamePlayer.checkPointArray[gamePlayer.arrayIndex].z - 2.33f); ;
        }
        else
        {
            gamePlayer.transform.position = new Vector3(
                gamePlayer.checkPointArray[gamePlayer.arrayIndex].x,
                gamePlayer.checkPointArray[gamePlayer.arrayIndex].y - 0.6f, 
                gamePlayer.checkPointArray[gamePlayer.arrayIndex].z - 2.33f);
        }
        gamePlayer.gameObject.SetActive(true);
        //Bring back breakingPlaform
        foreach (GameObject indivPlatform in breakingPlatformArray)
        {
            breakingPlatform = indivPlatform.GetComponent<BreakingPlatform>();
            breakingPlatform.respawnPlatform();
        }
        //Bring back woodenPlatform
        foreach (GameObject indivWoodenSpike in woodenSpikeArray)
        {
            woodenSpike = indivWoodenSpike.GetComponent<WoodenSpike>();
            woodenSpike.respawnSpike();
        }
        //Ensure player go through objects when die
        gamePlayer.playerCollider.enabled = true;
        
        //Reset last moving platform
        foreach (GameObject indivMovingPlatform3 in movingPlatform3Array)
        {
            movingPlatform3 = indivMovingPlatform3.GetComponent<MovingPlatform3>();
            movingPlatform3.resetPlatform();
            movingPlatform3.firstStep = true;
            movingPlatform3.resetSpeed();
            movingPlatform3.platformChargeSound.Stop();
            movingPlatform3.platformChargeSound.volume = 0.5f;
            movingPlatform3.platformDust.Stop();
        }

        //Reset the choosing platform area
        foreach (GameObject indivPlatformChoice in movingPlatformChoiceArray)
        {
            movingPlatformChoice = indivPlatformChoice.GetComponent<MovingPlatformChoice>();
            movingPlatformChoice.resetPlatform();
            movingPlatformChoice.speed = movingPlatformChoice.orginalSpeed;
            movingPlatformChoice.startMoving = false;
            movingPlatformChoice.moveToNext = false;
            movingPlatformChoice.moveToNext2 = false;
        }

        //Rest the movingCube
        foreach (GameObject indivMovingCube in movingCubeArray)
        {
            movingCube = indivMovingCube.GetComponent<MovingCube>();
            movingCube.resetPlatform();
            movingCube.resetParticle();
            movingCube.speed = movingCube.orginalSpeed;
            movingCube.startMoving = false;
            movingCube.moveToNext = false;
            movingCube.moveToNext2 = false;
            movingCube.movingCubeSound.Stop();
        }
        Invoke("deathAnimationOff", 0.3f);
        vcam.checkLookAheadY();
        vcam.decreaseYDamping();
    }

    private void deathAnimationOff()
    {
        gamePlayer.rb.constraints = RigidbodyConstraints2D.None;
        gamePlayer.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        gamePlayer.canDie = true;

    }

    public void startChoicePlatform()
    {
        foreach (GameObject indivPlatformChoice in movingPlatformChoiceArray)
        {
            movingPlatformChoice = indivPlatformChoice.GetComponent<MovingPlatformChoice>();
            movingPlatformChoice.startMoving = true;
        }
        foreach (GameObject indivMovingCube in movingCubeArray)
        {
            movingCube = indivMovingCube.GetComponent<MovingCube>();
            movingCube.startMoving = true;
        }
    }
}
