using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    public Sprite caveOne;
    public Sprite caveTwo;
    private SpriteRenderer checkpointSpriteRenderer;
    public bool checkpointReached;
    public GameObject Torches1;
    public GameObject Torches2;
    private Animator anim;
    public PlayerMovement gamePlayer;
    public AudioClip checkPointSound;

    // Start is called before the first frame update
    void Start()
    {
        checkpointSpriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        gamePlayer = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //1 checkpoint = two torches, activate two torches when touch checkpoint
        //two torches will play the animation
        if(other.tag == "Player")
        {
            CPOpen();
            CheckPointTorch torch1 = Torches1.GetComponent<CheckPointTorch>();
            CheckPointTorch torch2 = Torches2.GetComponent<CheckPointTorch>();
            torch1.TorchAction();
            torch2.TorchAction();
            checkpointReached = true;
        }
    }   

    public void CPClose()
    {
        anim.SetBool("CPOpen", false);
        checkpointSpriteRenderer.sprite = caveOne;
    }

    public void CPOpen()
    {
        anim.SetBool("CPOpen", true);
        checkpointSpriteRenderer.sprite = caveTwo;
    }

    private void checkPointAudio()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = 0.5f;
        audio.PlayOneShot(checkPointSound);
    }
}
