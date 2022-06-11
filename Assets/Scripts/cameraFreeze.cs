using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class cameraFreeze : MonoBehaviour
{
    public CinemachineBrain vcam;
    public PlayerMovement gamePlayer;
    private bool startGame;

    public CinemachineVirtualCamera vcam2;


    // Start is called before the first frame update
    void Start()
    {
        gamePlayer = FindObjectOfType<PlayerMovement>();
        startGame = true;
        if (startGame)
        {
            vcam = FindObjectOfType<CinemachineBrain>();
            vcam.enabled = false;
            Invoke("enabledCamera", 1.3f);
        }

        vcam2 = FindObjectOfType<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void enabledCamera()
    {
        if (startGame)
        {
            startGame = false;
        }
        vcam.enabled = true;
    }

    public void disabledCamera()
    {
        vcam.enabled = false;


    }


    public void deathCamera()
    {
        vcam.enabled = false;
        Invoke("enabledCamera", 1f);
    }

    public void uncheckLookAheadY()
    {
        vcam2.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadIgnoreY = false;
    }

    public void checkLookAheadY()
    {
        vcam2.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadIgnoreY = true;
    }

    public void increaseYDamping()
    {
        vcam2.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 1.3f;
    }

    public void decreaseYDamping()
    {
        vcam2.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 1f;
    }
}
