using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTransition : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTransition()
    {
        anim.SetTrigger("BlackTransition");
        Invoke("endTransition", 0.7f);
    }

    private void endTransition()
    {
        anim.SetTrigger("EndTransition");
    }
}
