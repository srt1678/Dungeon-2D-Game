using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameControlsMenu : MonoBehaviour
{
    private Animator anim;
    public GameObject returnButton;
    public PauseMenu pauseMenu;

    public bool controlsMenuOpening;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        returnButton.SetActive(false);
        pauseMenu = FindObjectOfType<PauseMenu>();
        controlsMenuOpening = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showControlMenu()
    {
        controlsMenuOpening = true;
        anim.SetBool("Control_Menu_Show", true);
        returnButton.SetActive(true);
    }

    public void closeControlMenu()
    {
        anim.SetBool("Control_Menu_Show", false);
        returnButton.SetActive(false);
        pauseMenu.openPauseMenu();
        controlsMenuOpening = false;
    }
}
