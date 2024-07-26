using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : Creature 
{
    protected GameObject player;
    protected bool playerDetected;

    Monster_Detection monsterDetection; 

    private float groundCheckDistance { set; get; }
    private float wallCheckDistance { set; get; }

    // [SerializeField]
    private bool isGround;
    // [SerializeField]
    private bool isWall; 

    private LayerMask groundLayer;
    private LayerMask wallLayer;
    private LayerMask playerLayer; 

    protected float attackRangeDistance { set; get; }
    protected float skillRangeDistance { set; get; }
    protected float attackDelay { set; get; }
    protected float skillCoolTime { set; get; }

    private bool skillEnable;
    private bool attackEnable;

    private bool damaged;

    protected Transform attackCheck;
    protected Transform skillCheck; 

    protected Vector2 hitRange { set; get; }
    protected Vector2 skillRange { set; get; }

    bool stateFlag;

    protected override void Init()
    {
        base.Init();

        player = null;
        playerDetected = false;

        monsterDetection = this.transform.GetChild(0).GetComponent<Monster_Detection>();
        attackCheck = this.transform.GetChild(1);
        skillCheck = this.transform.GetChild(2);

        monsterDetection.playerDetected.AddListener(Found); 
        monsterDetection.playerLost.AddListener(Lost);

        groundCheckDistance = 1f;
        wallCheckDistance = 0.1f;

        isGround = false; 
        isWall = false;

        groundLayer = 1 << LayerMask.NameToLayer("Ground");
        wallLayer = 1 << LayerMask.NameToLayer("Wall");
        playerLayer = 1 << LayerMask.NameToLayer("Player"); 

        attackEnable = true;
        skillEnable = true;

        stateFlag = false; 

    }

    #region State
    void ChangeState()
    {
        // This function is used for change between Idle and Move state. 
        if (!stateFlag)
        {
            stateFlag = true;
            
            int rand = Random.Range(0, 100);

            if (rand % 5 == 0)
                Flip(); 
    
            if (rand % 5 != 0)
            {
                if (creatureState == CreatureState.Idle)
                    creatureState = CreatureState.Move;

                else
                {
                    creatureState = CreatureState.Idle;
                    rb.velocity = new Vector2(0, rb.velocity.y);

                }
            }
            
            StartCoroutine(StateTimer(2)); 
        }
    }
    #endregion 

    #region Animation 
    protected override void UpdateAnimation()
    {
        if (damaged)
            return; 

        switch (creatureState)
        {
            case CreatureState.Idle:
                UpdateIdle();
                break;
            case CreatureState.Move:
                UpdateMove();
                break;
            case CreatureState.Attack:
                UpdateAttack();
                break;
            case CreatureState.Death:
                UpdateDeath();
                break;
        }
    }

    protected override void UpdateIdle()
    {
        anim.Play("Idle");
        ChangeState(); 
    }

    protected override void UpdateMove()
    { 
        if (playerDetected)
        {
            float direction = (player.transform.position.x - this.transform.position.x) > 0 ? 1 : -1;
            float distance = Mathf.Abs(player.transform.position.x - this.transform.position.x);

            if (distance <= skillRangeDistance)
            {
                if (skillEnable)
                {
                    creatureState = CreatureState.Attack;
                    rb.velocity = new Vector2(0, rb.velocity.y);


                    return;
                }
            }

            if (distance <= attackRangeDistance)
            {
                creatureState = CreatureState.Attack;
                rb.velocity = new Vector2(0, rb.velocity.y);

                return;
            }

            if (facingDir != direction)
                Flip();

            if (!isGround || isWall)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                anim.Play("Idle");
                return;
            }

        }

        anim.Play("Move"); 
        rb.velocity = new Vector2(speed * facingDir, rb.velocity.y);

        if(!playerDetected)
            ChangeState(); 
    }

    protected override void UpdateAttack()
    {
        if (!attackEnable)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                {
                    if (!isGround || isWall)
                        rb.velocity = new Vector2(speed * facingDir, rb.velocity.y);

                    return; 
                }
            }

            anim.Play("Idle");
            return;
        }

        if (!playerDetected)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                return;

            creatureState = CreatureState.Idle;
            return;
        }

        // Target is in the range and Monster is ready to Attack! 

        float direction = CheckDirection(player.transform.position);
        float distance = CheckDistance(player.transform.position);


        if (!skillEnable)
        {

            if (distance > attackRangeDistance)
            {
                creatureState = CreatureState.Move;
                return;
            }
        }

        else
        {
            if (distance > skillRangeDistance)
            {
                creatureState = CreatureState.Move;
                return;
            }
        }

        if (direction != facingDir)
            Flip();

        if (!isGround)
            return; 

        attackEnable = false;

        if (skillEnable)
        {
            skillEnable = false;
            anim.Play("Attack2");
            StartCoroutine(SkillDelay());
        }

        else
            anim.Play("Attack1");
        
        StartCoroutine(AttackDelay());

    }

    protected override void UpdateDeath()
    {
        anim.Play("Death");
        
    }
 

    #endregion

    #region Coroutine
    IEnumerator StateTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        stateFlag = false; 

    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        attackEnable = true;
    }

    IEnumerator SkillDelay()
    {
        yield return new WaitForSeconds(skillCoolTime);
        skillEnable = true;
    }

    IEnumerator DamagedDelay()
    {
        yield return new WaitForSeconds(0.5f);
        damaged = false; 
    }

    #endregion

    #region Check

    protected void IsGroundChecked()
    {
        float xPos = bc.bounds.center.x + ((bc.bounds.extents.x + 0.1f) * facingDir);
        float yPos = bc.bounds.center.y - bc.bounds.extents.y; 

        Vector2 pos = new Vector2(xPos, yPos);
        RaycastHit2D rayHit = Physics2D.Raycast(pos, Vector2.down, groundCheckDistance, groundLayer);

        if (!rayHit || rayHit.collider.tag != "Ground")
        {
            isGround = false;

            if (creatureState == CreatureState.Attack)
                rb.velocity = new Vector2(0, rb.velocity.y); 

            if (!playerDetected)
            {
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(bc.bounds.center.x, yPos), Vector2.down, groundCheckDistance, groundLayer); 

                if (hit == true)
                    Flip();
            }

        }

        else
        {
            isGround = true;
        }
    }

    protected void IsWallChecked()
    {
        Vector2 pos = bc.bounds.center + new Vector3((bc.bounds.extents.x * facingDir), 0, 0);
        
        RaycastHit2D rayHit = Physics2D.Raycast(pos, facingDir * Vector2.right, wallCheckDistance, wallLayer);

        if (rayHit)
        {
            if (rayHit.collider.tag == "Wall")
            {
                isWall = true;

                if (!playerDetected)
                    Flip();
            }
        }

        else
            isWall = false; 
    }

    void Found(GameObject _gameObject)
    {
        playerDetected = true;

        if (creatureState != CreatureState.Attack)
            creatureState = CreatureState.Move; 

        player = _gameObject; 
    }
    
    void Lost()
    {
        playerDetected = false;
        player = null; 
    }

    protected float CheckDistance(Vector2 targetPos)
    {
        float distance = Mathf.Abs(targetPos.x - this.transform.position.x);
        return distance; 
    }

    protected float CheckDirection(Vector2 targetPos)
    {
        float direction = (targetPos.x - this.transform.position.x) > 0 ? 1 : -1;
        return direction; 
    }

    #endregion

    void OnHit()
    {
        Vector2 pos;
        Vector2 size;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            pos = attackCheck.transform.position;
            size = hitRange; 
        }

        else
        {
            pos = skillCheck.transform.position;
            size = skillRange; 
        }


        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos, size, 0);
        foreach(Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Player")
            {
                Debug.Log("Player Hit!"); 
                collider.GetComponent<PlayerControl>().OnDamaged();
            }
        }

    }

    public override void OnDamaged()
    {
        if (damaged)
            return;

        base.OnDamaged();
        EnterDamaged(); 

    }
    private void EnterDamaged()
    {
        damaged = true;
        anim.Play("TakeHit");
        
        rb.velocity = new Vector2(0, rb.velocity.y);
        rb.AddForce(facingDir * -1 * Vector2.right, ForceMode2D.Impulse);
        rb.velocity = new Vector2(0, rb.velocity.y);

    }

    private void ExitDamaged()
    {
        anim.Play("Idle");
        StartCoroutine(DamagedDelay()); 
    }
}
