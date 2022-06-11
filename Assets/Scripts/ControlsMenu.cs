using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsMenu : MonoBehaviour
{
    private bool faded = true;
    public float duration = 0.4f;

    public GameObject returnButton;
    public MainMenu mainMenu;
    // Start is called before the first frame update
    void Start()
    {
        returnButton.SetActive(false);
        mainMenu = FindObjectOfType<MainMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeIn()
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(DoFade(canvasGroup, canvasGroup.alpha, faded ? 1 : 0));
        faded = !faded;
    }

    public void FadeOut()
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(DoFade(canvasGroup, canvasGroup.alpha, faded ? 1 : 0));
        faded = !faded;
        Invoke("buttonDisactive", duration);
    }

    public IEnumerator DoFade(CanvasGroup canvasGroup, float start, float end)
    {
        float counter = 0f;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, counter / duration);
            yield return null;
        }
    }

    private void buttonDisactive()
    {
        returnButton.SetActive(false);
        mainMenu.FadeIn();
        mainMenu.playButton.SetActive(true);
        mainMenu.controlsButton.SetActive(true);
        mainMenu.quitButton.SetActive(true);
    }
}
