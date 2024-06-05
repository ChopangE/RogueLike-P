using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Switch;
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
    public Vector2 inputVec;
    int isRight;
    int jumpCount;
    int gravity;
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
    public BoxCollider2D ladderCheck;
    public float climbSpeed;
    bool isLadder;
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
        rb.gravityScale = gravity;    //�ӽ��ڵ�
        Debug.Log(PlayerState);
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
            case State.Ladding:
                UpdateLadding();
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
        gravity = 1;

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

        ladderCheck = GetComponentInChildren<BoxCollider2D>();

    }
    void StateCheck() {
        isWall = Physics2D.Raycast(wallCheck.position, Vector2.right * isRight, wallChkDistance, w_Layer);
        if (isWall) {
            PlayerState = State.Walling;
            isWallJump = false;
            return;
        }
        if (isLadder) {
            PlayerState = State.Ladding;
            return;
        }
        if (isDash) {
            PlayerState = State.Dashing;
            return;
        }
        if (isAttack || isDashAttack) {
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
        anim.SetBool("isLadding", isLadder);

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
                case State.Ladding:
                    isLadder = false;
                    rb.AddForce(inputVec * jumpPower, ForceMode2D.Impulse);
                    jumpCount++;

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
            switch (PlayerState) {
                case State.Running:
                    if(inputVec.y < 0) anim.SetTrigger("Slide");
                    else if(inputVec.y > 0) StartLadding();
                    break;
                case State.Idle:
                    if (inputVec.y < 0) DownLadding();
                    else if (inputVec.y > 0) StartLadding();
                    break;
                case State.Jumping:
                case State.Falling:
                    StartLadding();
                    break;
                
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
    void DownLadding() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up * inputVec.y, 1f, LayerMask.GetMask("Ladder"));
        if(hit) {
            isLadder = true;
            transform.position = new Vector2(hit.collider.transform.position.x, transform.position.y -2*ladderCheck.bounds.extents.y);
        }
    }
    void StartLadding() {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(ladderCheck.bounds.center, ladderCheck.bounds.extents, 0);
        foreach (Collider2D collider in colliders) {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Ladder")) {
                isLadder = true;
                BoxCollider2D colBc = collider.gameObject.GetComponent<BoxCollider2D>();
                float x_pos = colBc.bounds.center.x;
                float y_pos = Mathf.Clamp(transform.position.y, colBc.bounds.center.y - colBc.bounds.extents.y + 0.1f, colBc.bounds.center.y + colBc.bounds.extents.y - 0.1f);
                transform.position = new Vector2(x_pos, y_pos);
                rb.velocity = Vector2.zero;
                break;

            }
        }
    }
    void LadderCheck() {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(ladderCheck.bounds.center, ladderCheck.bounds.extents, 0);
        foreach (Collider2D collider in colliders) {
            isLadder = collider.gameObject.layer == LayerMask.NameToLayer("Ladder");
            if (isLadder) {
                break;
            }
        }
    }
    void UpdateLadding() {
        if (!isLadder) {
            EndLadding();
            return;
        }
        LadderCheck();
        if (!isLadder) {
            EndLadding();
            return;
        }

        float direction = inputVec.y;
        if(direction == 0) {
            StopLadding();
        }
        else {
            GoLadding();
            if(direction < 0) CheckGround();
        }

    }
    void GoLadding() {
        rb.velocity = new Vector2(0, inputVec.y * climbSpeed);
        gravity = 1;
        anim.speed = 1f;
    }
    void StopLadding() {
        rb.velocity = Vector2.zero;
        gravity = 0;
        anim.speed = 0f;
    }
    void EndLadding() {
        isLadder = false;
        gravity = 1;
        anim.speed = 1f;
    }
    void CheckGround() {
        Debug.Log("check");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up * -1, 0.58f, LayerMask.GetMask("Ground"));
        if (hit) {
            EndLadding();
            rb.velocity = Vector2.zero;
        }
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
        if (isAttack || isWallJump || isDash || isDashAttack || isLadder) return;
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


