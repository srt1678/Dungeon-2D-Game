using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    //public GameObject pauseMenuUI;

    private NormalTorch normalTorch;
    private GameObject[] normalTorchArray;

    private CheckPointTorch checkPointTorch;
    private GameObject[] checkPointTorchArray;

    private SpinningBlade spinningBlade;
    private GameObject[] spinningBladeArray;


    public PauseMenu pauseMenu;
    public InGameControlsMenu controlsMenu;


    // Start is called before the first frame update
    void Start()
    {
        normalTorchArray = GameObject.FindGameObjectsWithTag("NormalTorch");
        checkPointTorchArray = GameObject.FindGameObjectsWithTag("CheckPointTorch");
        spinningBladeArray = GameObject.FindGameObjectsWithTag("SpinningBlade");

        pauseMenu = FindObjectOfType<PauseMenu>();
        controlsMenu = FindObjectOfType<InGameControlsMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                if (controlsMenu.controlsMenuOpening)
                {
                    controlsMenu.closeControlMenu();
                }
                else
                {
                    Resume();
                }
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenu.closePauseMenu();
        Time.timeScale = 1f;
        GameIsPaused = false;
        foreach (GameObject indivNormalTorch in normalTorchArray)
        {
            normalTorch = indivNormalTorch.GetComponent<NormalTorch>();
            normalTorch.PauseAudio("on");
        }
        foreach (GameObject indivCheckPointTorch in checkPointTorchArray)
        {
            checkPointTorch = indivCheckPointTorch.GetComponent<CheckPointTorch>();
            checkPointTorch.PauseAudio("on");
        }
        foreach (GameObject indivSpinningBlade in spinningBladeArray)
        {
            spinningBlade = indivSpinningBlade.GetComponent<SpinningBlade>();
            spinningBlade.PauseAudio("on");
        }
    }

    public void Pause()
    {
        pauseMenu.openPauseMenu();
        Time.timeScale = 0f;
        GameIsPaused = true;
        foreach (GameObject indivNormalTorch in normalTorchArray)
        {
            normalTorch = indivNormalTorch.GetComponent<NormalTorch>();
            normalTorch.PauseAudio("off");
        }
        foreach (GameObject indivCheckPointTorch in checkPointTorchArray)
        {
            checkPointTorch = indivCheckPointTorch.GetComponent<CheckPointTorch>();
            checkPointTorch.PauseAudio("off");
        }
        foreach (GameObject indivSpinningBlade in spinningBladeArray)
        {
            spinningBlade = indivSpinningBlade.GetComponent<SpinningBlade>();
            spinningBlade.PauseAudio("off");
        }
    }

}
