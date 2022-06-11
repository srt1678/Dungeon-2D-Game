using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour 
{
    public GameObject resumeButton;
    public GameObject controlsButton;
    public GameObject quitButton;

    private Animator anim;
    public InGameControlsMenu controlsMenu;

    void Start()
    {
        resumeButton.SetActive(false);
        controlsButton.SetActive(false);
        quitButton.SetActive(false);

        anim = GetComponent<Animator>();
        controlsMenu = FindObjectOfType<InGameControlsMenu>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void openPauseMenu()
    {
        resumeButton.SetActive(true);
        controlsButton.SetActive(true);
        quitButton.SetActive(true);
        anim.SetBool("PauseMenu_Show", true);
    }

    public void closePauseMenu()
    {
        anim.SetBool("PauseMenu_Show", false);
        resumeButton.SetActive(false);
        controlsButton.SetActive(false);
        quitButton.SetActive(false);
    }

    public void openControlsMenu()
    {
        closePauseMenu();
        controlsMenu.showControlMenu();
    }


    public void QuitGame()
    {
        Application.Quit();
    }

}
