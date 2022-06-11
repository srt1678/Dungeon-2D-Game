using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class OpeningLight : MonoBehaviour
{
    public GameObject openingLight;
    private Light2D lights;

    private float maxIntensity = 1f;
    private float minIntensity = 0f;
    private float maxFlickerTimer = 10f;
    private float minFlickerTimer = 5f;
    public float currentFlickerSpeed;
    private bool flickering = false;
    private float flickerDuration = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        lights = openingLight.GetComponent<Light2D>();
        lightFlicker();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void lightFlicker()
    {
        currentFlickerSpeed = Random.Range(minFlickerTimer, maxFlickerTimer);
        Invoke("ActivateFlicker", currentFlickerSpeed);
    }

    private void ActivateFlicker()
    {
        StartCoroutine(DoFlicker());
        Invoke("StopFlicker", 1f);
    }

    private IEnumerator DoFlicker()
    {
        flickering = true;
        while (flickering)
        {
            lights.intensity = Random.Range(minIntensity, maxIntensity);
            yield return new WaitForSeconds(flickerDuration);
        }
        flickering = false;
    }

    private void StopFlicker()
    {
        flickering = false;
        lights.intensity = maxIntensity;
        lightFlicker();
    }
    
}
