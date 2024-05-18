using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour {
    public enum State {
        Run, Idle, Walling, Attack, Jumping
    }
    [Header("# Player Move")]
    public float jumpPower = 5f;
    public int jumpCount = 0;
    public float maxSpeed = 5f;


    public State playerState { get; set; } = State.Idle;

    [Header("# Wall Check")]
    public Transform wallCheck;
    public LayerMask w_Layer;
    public float wallJumpPowerUp;
    public float wallJumpPowerSide;
    public float wallChkDistance;
    public float wallSildingSpeed;
    bool isWall;
    bool isWallJump;
    int isRight = 1;
    [Header("# Attack")]
    public bool isAttack;
    public float curAttack;
    float nowAttack;
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sprite;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    
    void Update() {
        nowAttack -= Time.deltaTime;
        
        isWall = Physics2D.Raycast(wallCheck.position, Vector2.right * isRight, wallChkDistance, w_Layer);
        anim.SetBool("isWall", isWall);
        
        //Walling
        if (isWall) {
            isWallJump = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * wallSildingSpeed);
            
            if (Input.GetButtonDown("Jump") && jumpCount < 2) {
                isWallJump = true;
                Invoke("FreezeMove", 0.2f);
                Vector2 WallJumpDir = new Vector2(-isRight * wallJumpPowerSide, wallJumpPowerUp);
                rb.velocity = WallJumpDir;
                FlipPlayer();
                jumpCount += 2;
                anim.SetBool("Jump", true);
                anim.SetBool("isFall", false);

            }
        }
        if (isAttack) {
            if (Input.GetKeyDown(KeyCode.Z)) {
                anim.SetTrigger("FollowingAttack");
            }
        }
        if (isWallJump || isAttack) return;
        //Attack
        if (Input.GetKeyDown(KeyCode.Z) && jumpCount == 0 && nowAttack < 0) {
            nowAttack = curAttack;
            anim.SetTrigger("Attack");
            rb.velocity = Vector2.zero;
        }
        //Jump
        if (Input.GetButtonDown("Jump") && jumpCount < 2) {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumpCount++;
            anim.SetBool("Jump", true);
            anim.SetBool("isFall", false);
        }
        else if (Input.GetButtonUp("Jump") && rb.velocity.y > 0) {
            rb.velocity = rb.velocity * 0.5f;
        }

        //Run and stop
        if (Input.GetButtonUp("Horizontal") && jumpCount == 0 && rb.velocity.y >= 0) {
            rb.velocity = Vector2.zero;
        }

        // Run or Idle
        if (Mathf.Abs(rb.velocity.x) < 0.3f) {
            anim.SetBool("Run", false);
        }
        else {
            anim.SetBool("Run", true);
        }

        //fall
        if (rb.velocity.y < 0) {
            anim.SetBool("isFall", true);

        }


    }
    
    void FixedUpdate() {
        if (isAttack || isWallJump) return;

        //Input Manager
        float inputX = Input.GetAxisRaw("Horizontal");
        if((inputX > 0 && isRight <0) || (inputX <0 && isRight > 0)) {
            FlipPlayer();
        }
        rb.AddForce(Vector2.right * inputX, ForceMode2D.Impulse);

        //limit speed
        if (rb.velocity.x > maxSpeed) {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }

        else if (rb.velocity.x < -maxSpeed) {
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
        }
        
    }
    void FlipPlayer() {
        transform.eulerAngles = new Vector3(0, Mathf.Abs(transform.eulerAngles.y - 180), 0);
        isRight *= -1;
    }
    void OnCollisionEnter2D(Collision2D collision) {

        if (collision.GetContact(0).normal.y > 0.6f) {

            anim.SetBool("Jump", false);
            anim.SetBool("isFall", false);
            if (jumpCount > 0) rb.velocity = Vector2.zero;

            jumpCount = 0;
            Debug.Log("Ground");
        }

        else {
            if (isWall) {
                jumpCount = 0;
            }
            rb.velocity = Vector2.zero;
        }

    }
    void FreezeMove() {
        isWallJump = false;
    }
}

