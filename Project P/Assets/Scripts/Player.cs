using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour {
    public enum State {
        Running, Idle, Walling, Attacking, Jumping, Ladding, Falling
    }
    [Header("# Player Move")]
    public float jumpPower = 5f;
    public int jumpCount = 0;
    public float maxSpeed = 5f;
    int isRight = 1;
    float inputX;


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

    [Header("# Ladder")]
    public float ladderSpeed;
    bool isLadder;
    bool isLadding;
    float initGravity;
    float ladderPosX;

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

    void Start() {
        initGravity = rb.gravityScale;
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
                jumpCount += 1;
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
        if (Input.GetKeyDown(KeyCode.Z) && jumpCount == 0 && nowAttack < 0 && !isLadding) {
            nowAttack = curAttack;
            anim.SetTrigger("Attack");
            rb.velocity = Vector2.zero;
        }
        //Jump
        if (Input.GetButtonDown("Jump") && jumpCount < 2 ) {
            if (isLadding) {
                if (Input.GetButtonDown("Jump")) {
                    if ((inputX > 0 && isRight < 0) || (inputX < 0 && isRight > 0)) {
                        FlipPlayer();
                    }
                    Vector2 ladderJump = new Vector2(inputX * 5f, 3f);
                    rb.AddForce(ladderJump, ForceMode2D.Impulse);
                }
            }
            else {
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                jumpCount++;
                //anim.SetBool("Jump", true);
                //anim.SetBool("isFall", false);
            }
        }
        else if (Input.GetButtonUp("Jump") && rb.velocity.y > 0 && !isLadding) {
            rb.velocity = rb.velocity * 0.5f;
        }

        //Run and stop
        if (Input.GetButtonUp("Horizontal") && jumpCount == 0 ) {
            rb.velocity = Vector2.zero;
        }

        // Run or Idle
        //if (Mathf.Abs(rb.velocity.x) < 0.3f) {
        //    anim.SetBool("Run", false);
        //}
        //else {
        //    anim.SetBool("Run", true);
        //}

        //fall
        //if (rb.velocity.y < 0) {
        //    anim.SetBool("isFall", true);

        //}
        anim.SetFloat("Jumping", rb.velocity.y);
        anim.SetFloat("Running", Mathf.Abs(rb.velocity.x));
        //ladder
        LadderUpdate();
    }
    
    void LadderUpdate() {
        if (isLadder && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))) {
            isLadder = false;
            rb.velocity = Vector2.zero;
            isLadding = true;
            rb.gravityScale = 0f;
            jumpCount = 1;
            transform.position = new Vector3(ladderPosX, transform.position.y, 0);
            //anim설정
        }

        if (isLadding && (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow))) {
            rb.velocity = Vector2.zero;
        }
    }

    void FixedUpdate() {
        if (isAttack || isWallJump) return;

        if (isLadding) {
            float ver = Input.GetAxisRaw("Vertical");
            if (ver != 0) {
                rb.velocity = new Vector2(rb.velocity.x, ver * ladderSpeed);
                //anim 실행
            }

        }
        //Input Manager
        inputX = Input.GetAxisRaw("Horizontal");
        //if (isLadding) {
        //    if (Input.GetButtonDown("Jump")) {
        //        if ((inputX > 0 && isRight < 0) || (inputX < 0 && isRight > 0)) {
        //            FlipPlayer();
        //        }
        //        Vector2 ladderJump = new Vector2(inputX * 5f, 3f);
        //        rb.AddForce(ladderJump, ForceMode2D.Impulse);
        //    }
            
        //}
        if (isLadding) return;

        if ((inputX > 0 && isRight <0) || (inputX <0 && isRight > 0)) {
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

            //anim.SetBool("Jump", false);
            //anim.SetBool("isFall", false);
            if (jumpCount > 0) rb.velocity = Vector2.zero;

            jumpCount = 0;
            Debug.Log("Ground");
        }

        else {
            if (isWall) {
                jumpCount = 1;
            }
        //    rb.velocity = Vector2.zero;
        }

    }
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Ladder")) {
            isLadder = true;
            ladderPosX = collision.transform.position.x;
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Ladder")) {
            isLadding = false;
            rb.gravityScale = initGravity;
        }
    }
    void FreezeMove() {
        isWallJump = false;
    }
}

