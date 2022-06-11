using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

using Cinemachine;


public class PlayerMovement : MonoBehaviour
{
    //Player Info
    public Rigidbody2D rb;
    public Transform feetPos;
    public float groundCheckRadius;
    public BoxCollider2D playerCollider;

    //Walking
    private bool isWalking;
    private float inputDirection; //Direction you enter
    private float facingDirection = 1; //Character facing direction
    private bool facingRight = true;
    public float movingSpeed;

    //Jumping
    private bool canJump;
    public float jumpForce;

    private float ogJumpForce;
    private float movingPlatformJumpForce;

    public float amountOfJumps;
    private float amountOfJumpsLeft;
    private bool checkJumpMultiplier;
    public float variableJumpHeightMultiplier = 0.5f;


    //Sliding
    private bool isTouchingWall;
    public Transform rightPos;
    private bool isWallSliding;
    public float wallSlidingSpeed;

    //Wall Jump
    private bool wallJumping;
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;
    public float reverseX;

    //Dash
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    public float setGravityScaleTime;
    private int dashDirection;
    private bool isDashing;
    public float amountOfDash;
    private float amountOfDashLeft;

    //Attack
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    
    //Spinning
    public Transform spinPointFeet;
    public float spinRange = 0.5f;
    public float spinBounceForce;

    //Respawn/ Death
    public Vector3 respawnPoint;
    public Vector3[] checkPointArray;
    public int arrayIndex;
    public int arraySize = 20;
    public CheckPointController cpController;
    public bool existed = false;
    public bool canDie;
    
    //Level Manager
    public LevelManager gameLevelManager;

    //Surroundings
    private bool isOnGround;
    public LayerMask groundLayer;

    //Animation
    private Animator anim;

    //Special Camera
    public cameraFreeze specialCamera;

    //ParticleSystem
    public ParticleSystem RunningDust;
    public ParticleSystem WallJumpDust;
    public ParticleSystem WallSlideDust;
    public ParticleSystem DashDust;
    public ParticleSystem SpinDust;
    public ParticleSystem AttackDust;
    public ParticleSystem FallingDust;


    //Other
    public MainMenu sceneChanger;


    //Sound
    public AudioClip playerMoving;
    public AudioClip playerJumping;
    public AudioClip playerDashing;
    public AudioClip playerAttacking;
    //public AudioClip playerSpinning;
    public AudioClip playerDeath;
    public AudioClip playerSpinAttack;


    public FadeOutMusic bgmSetting;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        amountOfJumpsLeft = amountOfJumps;
        amountOfDashLeft = amountOfDash;
        dashTime = startDashTime;
        anim = GetComponent<Animator>();
        respawnPoint = transform.position;
        gameLevelManager = FindObjectOfType<LevelManager>();
        checkPointArray = new Vector3[arraySize];
        arrayIndex = 0;
        checkPointArray[arrayIndex] = respawnPoint;
        cpController = FindObjectOfType<CheckPointController>();
        canDie = true;
        startOffFreeze();
        specialCamera = FindObjectOfType<cameraFreeze>();
        playerCollider = GetComponent<BoxCollider2D>();
        ogJumpForce = jumpForce;
        movingPlatformJumpForce = jumpForce + 5f;
        sceneChanger = FindObjectOfType<MainMenu>();
        bgmSetting = FindObjectOfType<FadeOutMusic>();
    }

    void Update()
    {
        //check if input is walking or jumping
        //Walking -> isWalking = true
        //Jumping -> Jump()
        CheckInput();
        //canJump = true
        CheckIfCanJump();
        //isWallSliding = true
        CheckIfWallSliding();
    }

    private void FixedUpdate()
    {
        //Moving and fliping character
        //(Flip will call flip function)
        Movement();
        WallJumping();
        CheckAnimation();
        //check isOnGround
        CheckSurroundings();
    }


    private void CheckAnimation()
    {
        if (isWallSliding)
        {
            anim.SetBool("WallSliding", true);
            anim.SetBool("Moving", false);
        }
        else
        {
            anim.SetBool("WallSliding", false);
        }

        if (inputDirection == 0 || isWallSliding)
        {
            anim.SetBool("Moving", false);
        }
        else
        {
            anim.SetBool("Moving", true);
        }

        if (isOnGround)
        {
            anim.SetBool("Jumping", false);
            anim.SetBool("Falling", false);
        }
        else if (rb.velocity.y <= 0)
        {
            anim.SetBool("Falling", true);
            anim.SetBool("Jumping", false);


            if (amountOfJumpsLeft != 0 && Input.GetKeyDown(KeyCode.Space))
            {
                CreateDust(1);
                anim.SetBool("Jumping", true);
                anim.SetBool("Falling", false);


            }
        }
        else
        {
            anim.SetBool("Jumping", true);
        }
    }

    private void CheckInput()
    {
        inputDirection = Input.GetAxisRaw("Horizontal");
        //Walking right or left
        if (inputDirection > 0)
        {
            isWalking = true;
        }else if(inputDirection < 0)
        {
            isWalking = true;
        }


        //Check if button is jump
        //If true, check whether the character is on ground or touching wall
        //To determine a normal jump or wall jump
        if (Input.GetButtonDown("Jump"))
        {
            if(isOnGround || !isTouchingWall)
            {
                NormalJumping();
            }else if (isWallSliding || isTouchingWall)
            {
                if (inputDirection == facingDirection)
                {
                    wallJumping = true;
                    Invoke("SetWallJumpingToFalse", wallJumpTime);
                }else
                {
                    ReverseJump();
                }
            }
        }
        //Jump Higher when pushing harder
        if (checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }


        //Dashing
        if (dashDirection == 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                CreateDust(4);
                anim.SetTrigger("Dashing");
                if (amountOfDashLeft > 0)
                {
                    isDashing = true;
                    if(inputDirection < 0)
                    {
                        dashDirection = 1;
                    }else if(inputDirection > 0)
                    {
                        dashDirection = 2;
                    }
                }
            }
        }
        else
        {
            if (dashTime <= 0)
            {
                dashDirection = 0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
                isDashing = false;
                amountOfDashLeft--;
            }
            else
            {
                dashTime -= Time.deltaTime;
                
                if (dashDirection == 1)
                {
                    //rb.velocity = Vector2.left * dashSpeed;
                    rb.gravityScale = 0;
                    rb.AddForce(new Vector2(facingDirection * dashSpeed, 0));
                }
                else if (dashDirection == 2)
                {
                    rb.gravityScale = 0;
                    //rb.velocity = Vector2.right * dashSpeed;
                    rb.AddForce(new Vector2(facingDirection * dashSpeed, 0));
                }
                
                Invoke("SetGravityScale", setGravityScaleTime);
                    
            }
        }

        //check attack && spin input
        if(Input.GetKeyDown(KeyCode.J) && isOnGround)
        {
            Attack();
        }else if((Input.GetKeyDown(KeyCode.J) && !isOnGround) && (!isWallSliding && !isDashing))
        {
            Spin();
        }
    }


    //check if can jump
    //check the amount of jumps left to determine if can jump
    private void CheckIfCanJump()
    {
        //restore the jumps (if want double jump)
        if (isOnGround && rb.velocity.y <= 0.01f)
        {
            amountOfJumpsLeft = amountOfJumps;
            amountOfDashLeft = amountOfDash;
        }
        if (wallJumping || isWallSliding)
        {
            amountOfJumpsLeft = 1;
            amountOfDashLeft = 1;
        }
        //only can jump if there are jumps left
        if(amountOfJumpsLeft > 0 && (!isWallSliding || !isTouchingWall))
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    }

    
    //if touching wall, not on ground, moving into wall -> wall sliding = true
    private void CheckIfWallSliding()
    {
        //check if player is on wall
        if(isTouchingWall && !isOnGround)
        {
            //check if player is pushing towards wall or not putting any input
           // if(facingDirection == inputDirection || inputDirection == 0 && !Input.GetKey(KeyCode.Space))
            if (facingDirection == inputDirection || inputDirection == 0)
            {
                isWallSliding = true;
            }
        }
        else
        {
            isWallSliding = false;
        }
    }
    

    private void NormalJumping()
    {
        if (canJump && !isWallSliding)
        {
            if(isOnGround && Input.GetKeyDown(KeyCode.Space))
            {
                CreateDust(1);
            }
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
            if(jumpForce == movingPlatformJumpForce)
            {
                jumpForce = ogJumpForce;
            }
            checkJumpMultiplier = true;
        }
    }

    private void WallJumping()
    {
        if (wallJumping)
        {
            //isWallSliding = false;
            CreateDust(2);
            rb.velocity = new Vector2(xWallForce * -inputDirection, yWallForce);
        }
    }

    private void ReverseJump()
    {
        if(Input.GetAxisRaw("Horizontal") != facingDirection && Input.GetAxisRaw("Horizontal") != 0)
        {
            rb.velocity = new Vector2(-inputDirection, yWallForce);
        }
    }

    private void Movement()
    {
        //Only walk if isWalking is true
        if (isWalking)
        {
            if(!isTouchingWall || isOnGround)
            {
                rb.velocity = new Vector2(movingSpeed * inputDirection, rb.velocity.y);

            }
        }
        //Flip the character
        if(inputDirection < 0 && facingRight)
        {
            Flip();
        }else if(inputDirection > 0 && !facingRight)
        {
            Flip();
        }
        //CheckIfWallSliding -> wall Sliding
        if (isWallSliding)
        {
            CreateDust(3);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            if (Input.GetButton("Jump"))
            {
                ClearDust();
            }
        }
        
    }
   

    //flip the character
    private void Flip()
    {
        facingDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
        if (isOnGround)
        {
            CreateDust(1);
        }
    }

    private void SetWallJumpingToFalse()
    {
        wallJumping = false;
    }

    private void SetGravityScale()
    {
        rb.gravityScale = 3;
    }

    //check if character is on ground
    private void CheckSurroundings()
    {
        isOnGround = Physics2D.OverlapCircle(feetPos.position, groundCheckRadius, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(rightPos.position, groundCheckRadius, groundLayer);
    }


    //Attack on ground
    private void Attack()
    {
        anim.SetTrigger("Attack");
        CreateDust(6);
        Collider2D [] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in hitEnemies)
        {
            if(enemy.tag == "Arrow")
            {
                //if attack arrow, arrow destroyed
                PFArrow1 arrowDestroy = enemy.GetComponent<PFArrow1>();
                arrowDestroy.breakArrow();
            }
            if (enemy.tag == "WoodenSpike")
            {
                WoodenSpike spikeDestroy = enemy.GetComponent<WoodenSpike>();
                spikeDestroy.breakSpike();
            }
        }
    }

    //Attack in air, spinning
    private void Spin()
    {
        anim.SetTrigger("Spin");
        CreateDust(5);
        Collider2D [] spinningFeet = Physics2D.OverlapCircleAll(spinPointFeet.position, spinRange, enemyLayers);
        foreach (Collider2D enemy in spinningFeet)
        {
            if (enemy.tag == "Arrow")
            {
                //if attack arrow, arrow destroyed
                PFArrow1 arrowDestroy = enemy.GetComponent<PFArrow1>();
                arrowDestroy.breakArrow();
            }
            if (enemy.tag == "WoodenSpike")
            {
                WoodenSpike spikeDestroy = enemy.GetComponent<WoodenSpike>();
                spikeDestroy.breakSpike();
            }
            if (enemy.tag != "WoodenSpike"){
                playerSpinningAttackAudio();
            }
            rb.velocity = new Vector2(rb.velocity.x, spinBounceForce);
            checkJumpMultiplier = true;
            amountOfJumpsLeft = 1;
            anim.SetTrigger("Spin");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Use array to check if already reached this checkpoint
        //if so, don't respawn/add this checkpoint
        //if not, add this new checkpoint into array
        if(collision.tag == "Checkpoint")
        {
            if (arrayIndex == 0)
            {
                arrayIndex += 1;
                respawnPoint = collision.transform.position;
                checkPointArray[arrayIndex] = respawnPoint;
            }
            else
            {
                for(int i = 0; i <= arrayIndex; i++)
                {
                    if(checkPointArray[i].x == collision.transform.position.x)
                    {
                        existed = true;
                        break;
                    }
                    else
                    {
                        existed = false;
                    }
                }
                if (!existed)
                {
                    arrayIndex++;
                    respawnPoint = collision.transform.position;
                    checkPointArray[arrayIndex] = respawnPoint;
                }
            }
        }
        if ((collision.tag == "Enemy" || collision.tag == "Arrow" || collision.tag == "WoodenSpike" 
            || collision.tag == "DeathZone" || collision.tag == "SpinningBlade") && canDie)
        {
            canDie = false;
            playerCollider.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            anim.SetTrigger("Death");
            gameLevelManager.Invoke("Respawn", 0.2f);
        }
        if(collision.tag == "DeathZoneCamera")
        {
            specialCamera.deathCamera();
        }
        if(collision.tag == "MovingPlatform2")
        {
            jumpForce = movingPlatformJumpForce;
        }
        if(collision.tag == "endGame")
        {
            sceneChanger.PlayGame();
            bgmSetting.fadeOutBGM();
        }
    }

    private void startOffFreeze()
    {
        anim.SetBool("OpeningFalling", true);
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        Invoke("startOffUnfreeze", 1f);
    }

    private void startOffUnfreeze()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        anim.SetBool("OpeningFalling", false);
    }

    //draw for feetpos
    /*
    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(feetPos.position, groundCheckRadius);
        //Gizmos.DrawWireSphere(rightPos.position, groundCheckRadius);
        //Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        //Gizmos.DrawWireSphere(spinPointFeet.position, spinRange);
    }
    */

    private void CreateDust(int index)
    {
        if(index == 1)
        {
            RunningDust.Play();
        }else if(index == 2)
        {
            WallJumpDust.Play();
        }else if(index == 3)
        {
            WallSlideDust.Play();
        }else if(index == 4)
        {
            DashDust.Play();
        }else if(index == 5)
        {
            SpinDust.Play();
        }else if(index == 6)
        {
            AttackDust.Play();
        }else if(index == 7)
        {
            FallingDust.Play();
        }
    }

    private void ClearDust()
    {
        WallSlideDust.Stop();
    }

    private void playerMovingAudio()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = 0.4f;
        audio.PlayOneShot(playerMoving);
    }

    private void playerJumpingAudio()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = 0.6f;
        audio.PlayOneShot(playerJumping);
    }

    private void playerDashingAudio()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = 0.5f;
        audio.PlayOneShot(playerDashing);
    }

    private void playerAttackingAudio()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = 0.3f;
        audio.PlayOneShot(playerAttacking);
    }

    /*
    private void playerSpinningAudio()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = 0.3f;
        audio.PlayOneShot(playerSpinning);
    }
    */

    private void playerDeathAudio()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = 0.7f;
        audio.PlayOneShot(playerDeath);
    }


    private void playerSpinningAttackAudio()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = 1f;
        audio.PlayOneShot(playerSpinAttack);
    }
}
