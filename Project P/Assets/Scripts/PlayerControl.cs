using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerControl : MonoBehaviour
{
    [System.Serializable]
    public enum State {
        Running, Idle, Walling, Attacking, Jumping, Ladding, Falling, Dashing
    }
    [Header("# Player Move")]
    public float jumpPower = 5f;
    public float maxSpeed = 5f;
    int isRight;
    public Vector2 inputVec;
    int jumpCount;

    [Header("# Dash")]
    public bool isDash;
    public bool dashEnable;
    public bool isDashAttack;
    public float dashSpeed;
    public State PlayerState { get; set; }

    [Header("# Wall Check")]
    public Transform wallCheck;
    public LayerMask w_Layer;
    public float wallJumpPowerUp;
    public float wallJumpPowerSide;
    public float wallChkDistance;
    public float wallSildingSpeed;
    bool isWall;
    public bool isWallJump;

    [Header("# Ladder")]
    public float ladderSpeed;
    bool isLadder;
    bool isLadding;
    float initGravity;
    float ladderPosX;

    [Header("# Attack")]
    public bool isAttack;
    public bool attackEnable;
    float nowAttack;
    [Header("# Attack")]
    public bool isSlide;


    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sprite;

    
    void Start()
    {
        Init();    
    }

    void Update()
    {
        rb.gravityScale = 1;    //임시코드
        switch (PlayerState) {
            case State.Idle:
                break;
            case State.Running:
                break;
            case State.Walling:
                UpdateWalling();
                break;
            case State.Dashing:
                //UpdateDash();
                break;
        }
        AnimationUpdate();
    }
    void LateUpdate() {
        StateCheck();
    }
    void FixedUpdate() {
        PlayerMove();
    }
    void Init() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        PlayerState = State.Idle;
        isRight = 1;
        
        wallCheck = transform.GetChild(0);
        w_Layer = LayerMask.GetMask("Wall");

        dashEnable = true;
        isDash = false;
        isDashAttack = false;

        attackEnable = true;
        isAttack = false;

        isSlide = false;
    }
    void StateCheck() {
        isWall = Physics2D.Raycast(wallCheck.position, Vector2.right * isRight, wallChkDistance, w_Layer);
        if (isWall) {
            PlayerState = State.Walling;
            isWallJump = false;
            return;
        }
        if (isDash) {
            PlayerState = State.Dashing;
            return;
        }
        if (isAttack) {
            PlayerState = State.Attacking;
            return;
        }
        if(rb.velocity.y > 0.1f) {
            PlayerState = State.Jumping;
            return;
        }
        if(rb.velocity.y < -0.1f) {
            PlayerState = State.Falling;
            return;
        }
        if(Mathf.Abs(rb.velocity.x) > 0.3f) {
            PlayerState = State.Running;
        }
        else {
            PlayerState = State.Idle;
        }
    }

    void AnimationUpdate() {
        anim.SetFloat("Jumping", rb.velocity.y);
        anim.SetFloat("Running", Mathf.Abs(rb.velocity.x));
        anim.SetBool("isWall", isWall);
        anim.SetBool("isDash", isDash);
        anim.SetBool("isDashAttack", isDashAttack);

    }
    #region Input
    public void ActionMove(InputAction.CallbackContext context) {
        inputVec.x = context.ReadValue<float>();
        
        if (context.canceled) {
            if (PlayerState == State.Running) { rb.velocity = Vector2.zero; }
        }
    }
    public void ActionJump(InputAction.CallbackContext context) {
        if (context.started) {
            switch (PlayerState) {
                case State.Running:
                case State.Idle:
                case State.Jumping:
                case State.Falling:
                    if (jumpCount < 2) {
                        rb.velocity = Vector2.zero;
                        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                        jumpCount++;
                    }
                    break;
                case State.Walling:
                    isWallJump = true;
                    Invoke("FreezeMove", 0.3f);
                    Vector2 WallJumpDir = new Vector2(-isRight * wallJumpPowerSide, wallJumpPowerUp);
                    rb.velocity = WallJumpDir;
                    FlipPlayer();
                    jumpCount += 1;                  
                    break;
            }
            
        }
        else if (context.canceled) {
            if (rb.velocity.y > 0)
                rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y * 0.5f);
            
        }
    }
    public void ActionAttack(InputAction.CallbackContext context) {
        if (context.started) {
            switch (PlayerState) {
                case State.Idle:
                case State.Running:
                    if (attackEnable) {
                        attackEnable = false;
                        rb.velocity = Vector2.zero;
                        anim.SetTrigger("Attack");
                        isAttack = true;
                        StartCoroutine(attackCoolTime(3f));
                    }
                    break;
                case State.Attacking:
                    anim.SetTrigger("FollowingAttack");
                    break;
                case State.Dashing:
                    isDashAttack = true;
                    break;

            }
        }
    }
    public void ActionDash(InputAction.CallbackContext context) {
        if (context.started) {
            switch (PlayerState) {
                case State.Running:
                    if (dashEnable) {

                        rb.AddForce(Vector2.right * isRight * dashSpeed, ForceMode2D.Impulse);
                        isDash = true;
                        dashEnable = false;
                        StartCoroutine(dashCoolTime(5f));
                    }
                    break;
            }
        }
    }
    public void ActionUpDown(InputAction.CallbackContext context) {
        inputVec.y = context.ReadValue<float>();
        if (context.started) {
            if (inputVec.y < 0) {
                switch (PlayerState) {
                    case State.Running:
                        anim.SetTrigger("Slide");
                        break;
                }
            }
            else if(inputVec.y > 0) {
                //ladder
            }

        }

    }
        #endregion
        void isFlip() {
        switch (PlayerState) {
            case State.Idle:
            case State.Running:
            case State.Jumping:
            case State.Falling:
                if (inputVec.x > 0 && isRight < 0 || inputVec.x < 0 && isRight > 0) { FlipPlayer(); }
                break;

        }
    }
    void UpdateWalling() {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * wallSildingSpeed);
    }
    
    public void dashEnd() {
        isDash = false;
        rb.velocity = Vector2.zero;
    }
    public void dashAttackEnd() {
        isDashAttack = false;
    }
    void FlipPlayer() {
        transform.eulerAngles = new Vector3(0, Mathf.Abs(transform.eulerAngles.y - 180), 0);
        isRight *= -1;
    }

    void PlayerMove() {
        if (isAttack || isWallJump || isDash) return;
        isFlip();
        rb.AddForce(Vector2.right * inputVec.x, ForceMode2D.Impulse);
        if (rb.velocity.x > maxSpeed) {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }
        else if (rb.velocity.x < -maxSpeed) {
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
        }
    }

    void FreezeMove() {
        isWallJump = false;
    }
    #region Collision
    void OnCollisionEnter2D(Collision2D collision) {

        if (collision.GetContact(0).normal.y > 0.6f) {

            if (jumpCount > 0) rb.velocity = Vector2.zero;

            jumpCount = 0;
            Debug.Log("Ground");
        }

        else {
            if (isWall) {
                jumpCount = 1;
            }
        }

    }
    #endregion

    #region coolTime
    IEnumerator attackCoolTime(float time) {
        yield return new WaitForSeconds(time);
        attackEnable = true;
    }
    IEnumerator dashCoolTime(float time) {
        yield return new WaitForSeconds(time);
        dashEnable = true;
    }
    #endregion
}


