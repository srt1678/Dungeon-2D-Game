using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public static AudioClip playerMovingSound;
    /*
    public AudioClip playerMovingSound, playerJumpingSound, playerDeadSound,
        playerSpinningSound, playerDashingSound, playerAttackingSound,
        playerWallJumpingSound, playerWallSlidingSound;
    */
    public static AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        playerMovingSound = Resources.Load<AudioClip>("playerMoving");
        /*
        playerJumpingSound = Resources.Load<AudioClip>("playerJumping");
        playerDeadSound = Resources.Load<AudioClip>("playerDead");
        playerSpinningSound = Resources.Load<AudioClip>("playerSpinning");
        playerDashingSound = Resources.Load<AudioClip>("playerDashing");
        playerAttackingSound = Resources.Load<AudioClip>("playerAttacking");
        playerWallJumpingSound = Resources.Load<AudioClip>("playerWallJumping");
        playerWallSlidingSound = Resources.Load<AudioClip>("playerWallSliding");
        */

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "playerMoving":
                audioSource.PlayOneShot(playerMovingSound);
                break;
                /*
            case "playerJumping":
                audioSource.PlayOneShot(playerJumpingSound);
                break;
            case "playerDead":
                audioSource.PlayOneShot(playerDeadSound);
                break;
            case "playerSpinning":
                audioSource.PlayOneShot(playerSpinningSound);
                break;
            case "playerDashing":
                audioSource.PlayOneShot(playerDashingSound);
                break;
            case "playerAttacking":
                audioSource.PlayOneShot(playerAttackingSound);
                break;
            case "playerWallJumping":
                audioSource.PlayOneShot(playerWallJumpingSound);
                break;
            case "playerWallSliding":
                audioSource.PlayOneShot(playerWallSlidingSound);
                break;
                */
        }
    }
}
