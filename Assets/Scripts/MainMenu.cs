using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 0.5f;
    private bool faded = false;
    public float duration = 0.4f;
    public GameObject playButton;
    public GameObject controlsButton;
    public GameObject quitButton;
    public ControlsMenu controlsMenu;


    void Start()
    {
        controlsMenu = FindObjectOfType<ControlsMenu>();
    }

    public void PlayGame()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("SceneTransition");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
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
        while(counter < duration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, counter / duration);
            yield return null;
        }
    }

    private void buttonDisactive()
    {
        playButton.SetActive(false);
        controlsButton.SetActive(false);
        quitButton.SetActive(false);
        controlsMenu.FadeIn();
        controlsMenu.returnButton.SetActive(true);
    }
}
