using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Switch;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using static UnityEngine.UI.Image;

public class PlayerControl : MonoBehaviour
{
    [System.Serializable]
    public enum State {
        Running, Idle, Walling, Attacking, Jumping, Ladding, Falling, Dashing, Croush
    }
    [Header("# Player Move")]
    public float jumpPower = 5f;
    public float maxSpeed = 5f;
    public Vector2 inputVec;
    int isRight;
    int jumpCount;
    int gravity;
    bool isRun;
    bool isJump;

    [Header("# Dash")]
    public bool isDash;
    public bool dashEnable;
    public bool isDashAttack;
    public float dashSpeed;
    public State PlayerState { get; set; }
    public float dashCurTime;

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
    public Collider2D groundColl;
    public Transform GroundCheck;
    public PlatformEffector2D effector;
    public LayerMask playerMask;
    public LayerMask LadderMask;

    [Header("# Attack")]
    public bool isAttack;
    public bool attackEnable;
    public float AttackPower = 5f;
    float nowAttack;
    public float attackCurTime;

    [Header("# Attack Check")]
    public Transform attackCheck;

    [Header("# Slide")]
    public bool isSlide;

    [Header("# Cruosh")]
    public bool isCroush;

    [Header("# Damaged")]
    public bool isDamaged;

    [Header("# Health")]
    public int health;
    public int curHealth;

    [Header("# level")]
    public int level;

    [Header("#Dead")]
    public bool isDead;

    public Rigidbody2D rb;
    public Animator anim;
    SpriteRenderer sprite;

    
    void Start()
    {
        Init();    
    }

    void Update()
    {
        rb.gravityScale = gravity;
        if (isDead) return;
        //Debug.Log(isDamaged);
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
        if (isDead) return;
        StateCheck();
    }
    void FixedUpdate() {
        if (isDead) return;
        PlayerMove();
    }
    void Init() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        effector = FindObjectOfType<PlatformEffector2D>(false);
        gravity = 1;

        PlayerState = State.Idle;
        isRight = 1;
        
        wallCheck = transform.GetChild(0);
        w_Layer = LayerMask.GetMask("Wall");

        isRun = false;

        dashEnable = true;
        isDash = false;
        isDashAttack = false;
        dashCurTime = 5.0f;

        attackEnable = true;
        isAttack = false;
        attackCurTime = 3.0f;

        isSlide = false;

        isCroush = false;

        ladderCheck = GetComponentInChildren<BoxCollider2D>();

        isDamaged = false;

        isDead = false;
        SetStatus();

    }
    public void SetStatus() {
        jumpPower = GameManager.instance.pd.jump;
        maxSpeed = GameManager.instance.pd.speed;
        AttackPower = GameManager.instance.pd.atk;
        health = GameManager.instance.pd.health;
        curHealth = GameManager.instance.pd.curhealth;
        level = GameManager.instance.pd.level;
    }
    void StateCheck() {
        if (isDead) return;
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
        if (isCroush) {
            PlayerState = State.Croush;
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
        anim.SetBool("Jump", isJump);
        anim.SetFloat("Running", Mathf.Abs(rb.velocity.x));
        anim.SetBool("Run", isRun);
        anim.SetBool("isWall", isWall);
        anim.SetBool("isDash", isDash);
        anim.SetBool("isDashAttack", isDashAttack);
        anim.SetBool("isLadding", isLadder);
        anim.SetBool("isCroush", isCroush);
    }
    #region Input
    public void ActionMove(InputAction.CallbackContext context) {
        if (isDead) return;
        inputVec.x = context.ReadValue<float>();
        if (context.started) {
            isRun = true;
            if (PlayerState == State.Croush) { isCroush = false; }
        }
        if (context.canceled) {
            isRun = false;
            if (PlayerState == State.Running) { rb.velocity = Vector2.zero; }
        }
    }
    public void ActionJump(InputAction.CallbackContext context) {
        if (isDead) return;
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
                    if (inputVec.y == 0) {
                        isLadder = false;
                        rb.AddForce(inputVec * jumpPower, ForceMode2D.Impulse);
                        jumpCount++;
                    }
                    break;
            }
            
        }
        else if (context.canceled) {
            if (rb.velocity.y > 0)
                rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y * 0.5f);
            
        }
    }
    public void ActionAttack(InputAction.CallbackContext context) {
        if (isDead) return;
        if (context.started) {
            switch (PlayerState) {
                case State.Idle:
                case State.Running:
                    if (attackEnable) {
                        attackEnable = false;
                        rb.velocity = Vector2.zero;
                        anim.SetTrigger("Attack");
                        isAttack = true;
                        StartCoroutine(attackCoolTime(attackCurTime));
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
        if (isDead) return;
        if (context.started) {
            switch (PlayerState) {
                case State.Running:
                    if (dashEnable) {
                        rb.AddForce(Vector2.right * isRight * dashSpeed, ForceMode2D.Impulse);
                        isDash = true;
                        dashEnable = false;
                        StartCoroutine(dashCoolTime(dashCurTime));
                    }
                    break;
            }
        }
    }
    public void ActionUpDown(InputAction.CallbackContext context) {
        if (isDead) return;
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
                case State.Ladding:
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
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y - 2*ladderCheck.bounds.extents.y, 0);
        //RaycastHit2D hit = Physics2D.Raycast(newPos, Vector2.up * inputVec.y, 0.5f, LayerMask.GetMask("Ladder"));
        RaycastHit2D hit2 = Physics2D.Raycast(newPos, Vector2.up * inputVec.y, 0.5f, LayerMask.NameToLayer("Ground"));
        RaycastHit2D hit = Physics2D.BoxCast(GroundCheck.transform.position, new Vector2(0.3f, 0.5f), 0, new Vector2(0, -1f), 0.5f,LayerMask.GetMask("Ladder"));
        //foreach (Collider2D c in colliders) {
        //    if (c.gameObject.tag == "Ground") {
        //        groundColl = c;
        //        Debug.Log("찾음!!");
        //        //groundColl.enabled = false;
        //    }
        //}
        if (hit) {
            isLadder = true;
            //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground"), true);
            if(effector == null) {
                effector = FindObjectOfType<PlatformEffector2D>();
            }
            effector.colliderMask &= ~playerMask;
            //groundColl = hit2.collider.GetComponent<Collider2D>();
            transform.position += Vector3.down * 0.5f;
            StartLadding();
        }
        else {
            isCroush = true;
        }
    }
    void StartLadding() {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(ladderCheck.bounds.center, ladderCheck.bounds.extents, 0);
        foreach (Collider2D collider in colliders) {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Ladder")) {
                isLadder = true;
                Collider2D colBc = collider.gameObject.GetComponent<Collider2D>();
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
            if (!groundColl && (collider.gameObject.layer == LayerMask.NameToLayer("Ground"))) {    //���׼���
                groundColl = collider;
                break;
            }
        }

        foreach (Collider2D collider in colliders) {
            isLadder = collider.gameObject.layer == LayerMask.NameToLayer("Ladder");
            //isLadder = true;
            if (isLadder) {
                break;
            }
        }
    }
    //void RunningAndJumpingLadderCheck() {
    //    Collider2D[] colliders = Physics2D.OverlapBoxAll(ladderCheck.bounds.center, ladderCheck.bounds.extents, 0);
    //    foreach (Collider2D collider in colliders) {
    //        isLadder = collider.gameObject.layer == LayerMask.NameToLayer("Ladder");
    //        if (isLadder) {
    //            break;
    //        }
    //    }
    //}
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
        if (direction < 0 && CheckGround()) return;
        if(direction == 0) {    
            StopLadding();
        }
        else {
            GoLadding();
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
        if (groundColl) {
            groundColl.enabled = true;
            groundColl = null;
        }
        //if (groundColl) groundColl = null;
        //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground"), false);
        if (effector == null) {
            effector = FindObjectOfType<PlatformEffector2D>();
        }
        effector.colliderMask |= playerMask;
        isLadder = false;
        gravity = 1;
        anim.speed = 1f;
    }

    bool CheckGround() {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up * -1, 0.58f, LayerMask.GetMask("Ground"));
        Collider2D[] colliders = Physics2D.OverlapBoxAll(GroundCheck.transform.position, new Vector2(0.3f, 0.2f), 0);
        bool isLadderOn = false;
        bool isGroundOn = false;
        
        foreach (var coll in colliders) {
            if(coll.gameObject.tag == "Ground" ) {
                isGroundOn = true;
               
            }
            else if(coll.gameObject.tag == "Ladder") {
                isLadderOn = true;
            }
        }
        if (!isLadderOn && isGroundOn) {
            EndLadding();
            rb.velocity = Vector2.zero;
            return true;
        }
        return false;
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
        if(inputVec.y > 0 && !isLadder)
        StartLadding();
    }

    void FreezeMove() {
        isWallJump = false;
    }

    #region Damaged
    public void OnDamaged()
    {
        if (isDamaged)
            return;

        EnterDamaged(); 
    }

    void EnterDamaged()
    {
        StartDamageEffect();
        isDamaged = true;
        curHealth -= 1;
        GameManager.instance.pd.curhealth = curHealth;
        GameManager.instance.SetStatus();
        if(curHealth <= 0) {
            isDead = true;
            anim.SetBool("isDead",isDead);
            return;
        }
        anim.SetBool("isDamaged", true); 
    }

    void ExitDamaged()
    {
        anim.SetBool("isDamaged", false); 
        StartCoroutine(damagedCoolTime());     
    }

    public GameObject postProcessing;
    public Volume volume;
    public Vignette vignette;

    Coroutine coEffect; 

    void StartDamageEffect()
    {
        if (postProcessing == null)
            return;

        CancelCoEffect();
        StartCoroutine(DamageEffect()); 
    }

    void CancelCoEffect()
    {
        if(coEffect == null)
            return;

        StopCoroutine(coEffect); 
        coEffect = null; 
    }

    IEnumerator DamageEffect()
    {
        volume = postProcessing.GetComponent<Volume>();

        if (volume.profile.TryGet<Vignette>(out vignette))
        {
            vignette.intensity.value = 0.4f; 
        }

        while(vignette.intensity.value > 0)
        {
            vignette.intensity.value -= 0.01f;
            yield return new WaitForSeconds(0.1f); 
        }

        yield return null; 
    }

    #endregion

    #region Hit
    void OnHit()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(attackCheck.position, new Vector2(2, 2), 0);

        foreach (Collider2D collider in colliders)
        {
            if(collider.gameObject.tag == "Monster")
                collider.GetComponent<Monster>().OnDamaged(this.gameObject);
        }
    }
    #endregion 

    #region Collision
    void OnCollisionEnter2D(Collision2D collision) {
       
        if (collision.GetContact(0).normal.y > 0.6f) {
            //if (jumpCount > 0) rb.velocity = Vector2.zero;
            rb.velocity = Vector2.zero;
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

    IEnumerator damagedCoolTime()
    {
        yield return new WaitForSeconds(0.5f);
        isDamaged = false; 
    }
    #endregion
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(GroundCheck.transform.position, new Vector2(0.3f, 0.2f));
    }
}



