using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Re_Monster : Creature
{
    protected enum EMonsterType
    {
        Normal,
        Boss
    }

    EMonsterType monsterType; 

    [SerializeField]
    protected GameObject player;

    protected float groundCheckDistance { set; get; }
    protected float wallCheckDistance { set; get; }

    [SerializeField]
    protected bool isGround;
    [SerializeField]
    protected bool isWall;

    protected LayerMask groundLayer;
    protected LayerMask wallLayer;

    [SerializeField]
    protected Vector2 searchDistance;

    [SerializeField]
    protected BaseSkill selectedSkill; 

    protected bool damaged;
    protected bool dead; 

    protected Coroutine coWait;

    
    protected override ECreatureState CreatureState
    {
        get {  return base.CreatureState; }
        set
        {
            base.CreatureState = value;
        }
    }

    protected EMonsterType MonsterType
    {
        get { return monsterType; }
        set
        {
            monsterType = value;
        }
    }

    #region Init
    protected override void Init()
    {
        base.Init();

        player = null; 

        groundCheckDistance = 0.1f;
        wallCheckDistance = 0.1f;

        isGround = false;
        isWall = false;

        groundLayer = 1 << LayerMask.NameToLayer("Ground");
        wallLayer = 1 << LayerMask.NameToLayer("Wall");

        CreatureState = ECreatureState.Idle;

        skillList = new List<BaseSkill>();
        enableSkillList = new List<BaseSkill>();

        selectedSkill = null;

        damaged = false;
        dead = false; 

        coWait = null; 

    }

    #endregion

    protected virtual void Update()
    {
        if (dead)
            return; 

        if (monsterType == EMonsterType.Normal)
        {
            if (damaged)
                return; 
        }

        switch (_creatureState)
        {
            case ECreatureState.Idle:
                UpdateIdle();
                break;
            case ECreatureState.Move:
                UpdateMove();
                break;
            case ECreatureState.Attack:
                UpdateAttack();
                break;
            case ECreatureState.Death:
                UpdateDeath();
                break;
        }

        IsWallChecked();
        IsGroundChecked(); 
    }

    #region AI 
    protected override void UpdateIdle()
    {
        // Find player 
        GameObject target = FindPlayerInRange();

        rb.velocity = new Vector2(0, rb.velocity.y); 

        // Chase, if player is in the range 
        if (target != null)
        {
            player = target;
            CreatureState = ECreatureState.Move;
            return;
        }
        
        
        // Patrol, if player is not in the detect range 
        if (coWait != null)
            return; 

        {
            float patrolPercent = 30;
            float rand = Random.Range(1, 100); 
            
            if (rand < patrolPercent)
            {
                CreatureState = ECreatureState.Move;
                return; 
            }

            StartWait(1f);
        }

    }

    protected override void UpdateMove()
    {
        if (player != null)
        {
            GameObject target = FindPlayerInRange(); 

            if(target == null)
            {
                player = target;
                CreatureState = ECreatureState.Move;
                PlayAnimation("Move");
                selectedSkill = null; 

                return; 
            }

            if(selectedSkill == null)
                selectedSkill = SelectSkill(); 

            float distance = CheckDistance(player.transform.position);
            float direction = CheckDirection(player.transform.position);

            if (selectedSkill.IsSkillReachable(distance))
            {
                CancelWait();
                CreatureState = ECreatureState.Attack;
                return;
            }

            else
            {
                if (direction != dir)
                    Flip();

                if (isWall || !isGround)
                {
                    PlayAnimation("Idle");

                    rb.velocity = new Vector2(0, rb.velocity.y); 

                    return; 
                }

                speed = 3f;
                rb.velocity = new Vector2(speed * dir, rb.velocity.y); 
            }
        }

        if (player == null)
        {
            GameObject target = FindPlayerInRange(); 

            if (target != null)
            {
                player = target;
                CreatureState = ECreatureState.Move;
                PlayAnimation("Move"); 
                return; 
            }

            speed = 2f;
            rb.velocity = new Vector2(1 * speed * dir, rb.velocity.y);

            if (coWait != null)
                return;

            StartWait(0.5f); 

            float stopPercent = 20;
            float turnAroundPercent = 20; 

            int stopRand = Random.Range(1, 100);
            int turnRand = Random.Range(1, 100); 

            if (turnAroundPercent > turnRand)
                Flip();

            if (stopPercent > stopRand)
            {
                CreatureState = ECreatureState.Idle;
                return; 
            }
        }
    }

    protected override void UpdateAttack()
    {
        if (coWait != null)
            return;

        // If the attack is end, it should select next skills to attack 

        if(selectedSkill == null)
        {
            selectedSkill = SelectSkill();
            
            float distance = CheckDistance(player.transform.position); 
            
            if (!selectedSkill.IsSkillReachable(distance))
            {
                if (isWall || !isGround)
                {
                    _creatureState = ECreatureState.Move; 
                }

                else
                {
                    CreatureState = ECreatureState.Move; 
                }

                return; 
            }
        }

        rb.velocity = new Vector2(0, rb.velocity.y);

        GameObject target = FindPlayerInRange(); 

        if(target == null)
        {
            player = target;

            selectedSkill = null; 
            CreatureState = ECreatureState.Idle;
            return; 
        }

        if (CheckDirection(player.transform.position) != dir)
            Flip();

        // In case of normal monster, if it is onDamaged during the attack, it should stop attack and loss its coolTime also;
        if (!selectedSkill.IsSkillEnable())
        {
            selectedSkill = SelectSkill();

            float distance = CheckDistance(player.transform.position);

            if (!selectedSkill.IsSkillReachable(distance))
            {
                CreatureState = ECreatureState.Move;
                return;
            }
        }

        selectedSkill.DoSkill();

        float delay = selectedSkill.GetSkillAnimationClip().length;
        
        StartWait(delay);

    }

    protected override void UpdateDeath()
    {
        dead = true;
        damaged = true;
    }
    #endregion

    #region Skill
    protected BaseSkill SelectSkill()
    {
        int rand = Random.Range(0, enableSkillList.Count);
        return enableSkillList[rand]; 
    }

    public void StartAttack()
    {

    }

    public void CancelAttack()
    {
        selectedSkill = null; 
    }

    public void EndAttack()
    {
        selectedSkill = null; 

        PlayAnimation("Idle");
        StartWait(1f);
    }

    #endregion 

    #region Animation 
    protected override void UpdateAnimation()
    {
        switch (_creatureState)
        {
            case ECreatureState.Idle:
                anim.Play("Idle");
                break;
            case ECreatureState.Move:
                anim.Play("Move");
                break;
            case ECreatureState.Attack:
                break;
            case ECreatureState.Death:
                anim.Play("Death"); 
                break;
        }
    }

    #endregion 

    #region Hit
    protected override void OnHit()
    {
        float direction;

        if (Mathf.Sign(transform.localScale.x) >= 0)
            direction = 1;

        else
            direction = -1;

        Vector2 pos = new Vector2(bc.bounds.center.x, bc.bounds.center.y) + new Vector2(direction * selectedSkill.skillPos.x, selectedSkill.skillPos.y);
        Vector2 size = selectedSkill.skillSize; 

        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos, size, 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Player")
            {
                collider.GetComponent<PlayerControl>().OnDamaged();
            }
        }
    }
    #endregion

    #region Damaged
    public override void OnDamaged(GameObject go)
    {
        if (damaged)
            return;

        PlayerControl player = go.GetComponent<PlayerControl>();
        
        float damage = player.AttackPower; 
        
        float direction = CheckDirection(go.transform.position);

        Vector2 pos = new Vector2(this.transform.position.x + direction * 0.5f, this.transform.position.y);

        creatureFX.CreatePopUpText(damage.ToString(), pos);

        currentHp -= damage;

        if (monsterType == EMonsterType.Normal)
        {

            if (direction != dir)
                Flip();

            if(currentHp <= 0)
            {
                CancelWait();
                CreatureState = ECreatureState.Death;
                return; 
            }

            damaged = true;

            PlayAnimation("TakeHit"); 

            rb.velocity = new Vector2(0, rb.velocity.y);
            rb.AddForce(CheckDirection(player.transform.position) * Vector2.right, ForceMode2D.Impulse);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void ExitDamaged()
    {
        PlayAnimation("Idle");
        StartWait(0.5f);
    }
    #endregion 

    #region Check
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

    protected GameObject FindPlayerInRange()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(bc.bounds.center, searchDistance, 0);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {

                player = collider.gameObject;
                return player;
            }
        }

        return null;
    }

    protected void IsGroundChecked()
    {
        float xPos = bc.bounds.center.x + ((bc.bounds.extents.x + 0.1f) * (dir));
        float yPos = bc.bounds.center.y - bc.bounds.extents.y;

        Vector2 pos = new Vector2(xPos, yPos);
        RaycastHit2D rayHit = Physics2D.Raycast(pos, Vector2.down, groundCheckDistance, groundLayer | wallLayer);


        if (!rayHit || !(rayHit.collider.tag == "Ground" || rayHit.collider.tag == "Wall"))
        {
            
            isGround = false;

            if (_creatureState == ECreatureState.Attack)
                rb.velocity = new Vector2(0, rb.velocity.y);

            
            if (player == null)
            {
                Flip();
            }
        }

        else
            isGround = true;
    }

    protected void IsWallChecked()
    {
        Vector2 pos = bc.bounds.center + new Vector3((bc.bounds.extents.x * dir), 0, 0);

        RaycastHit2D rayHit = Physics2D.Raycast(pos, (dir) * Vector2.right, wallCheckDistance, groundLayer | wallLayer);

        if (rayHit)
        {
            if (rayHit.collider.tag == "Wall" || rayHit.collider.tag == "Ground")
            {
                Debug.Log("Wall!");
                isWall = true;

                if (player == null)
                    Flip();
            }
        }

        else
            isWall = false;
    }
    #endregion

    #region Coroutine

    public void StartWait(float seconds)
    {
        CancelWait(); 
        coWait = StartCoroutine(CoWait(seconds)); 
    }

    IEnumerator CoWait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        coWait = null;

    }

    protected void CancelWait()
    {
        if (coWait != null)
            StopCoroutine(coWait);

        coWait = null;

        if (damaged)
            damaged = false; 

      }
    void Despawn(float seconds)
    {
        StartCoroutine("coDespawn", seconds); 
    }

    IEnumerator coDespawn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        this.gameObject.SetActive(false); 
    }

    #endregion

    #region DrawGizmos
    private void OnDrawGizmos()
    {
        UnityEngine.Color color;
        color = UnityEngine.Color.red;
        Gizmos.color = color;

        if (bc != null && selectedSkill != null)
        {
            float direction;

            if (Mathf.Sign(transform.localScale.x) >= 0)
                direction = 1;

            else
                direction = -1; 

            Vector2 pos = new Vector2(bc.bounds.center.x, bc.bounds.center.y) + new Vector2(direction * selectedSkill.skillPos.x, selectedSkill.skillPos.y);

            Vector2 size = selectedSkill.skillSize;

            Gizmos.color = UnityEngine.Color.red;
            color.a = 0.5f;
            Gizmos.DrawCube(pos,size);
        }

        /*
        if (bc != null)
        {
            Vector2 pos = bc.bounds.center + new Vector3((bc.bounds.extents.x * dir), 0, 0);

            color.a = 0.5f;
            Gizmos.DrawRay(pos, dir * Vector2.right);
        }
        */ 
    }
    #endregion 
}
