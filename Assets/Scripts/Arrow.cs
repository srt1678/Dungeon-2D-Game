using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform firePoint;
    private float nextActionTime = 0.0f;
    public float period = 0.1f;
    public GameObject arrowPrefab;
    public float delayTime = 0.0f;
    // Update is called once per frame

    void Start()
    {
        
        
    }

    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            Invoke("Shoot", delayTime);
        }
    }
    
    private void Shoot()
    {
        Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
    }

}
