using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public enum CreatureState
    {
        Idle,
        Move,
        Attack,
        Death
    }

    [SerializeField] 
    protected CreatureState creatureState;

    protected SpriteRenderer sr; 
    protected Animator anim;
    protected Rigidbody2D rb;
    protected BoxCollider2D bc;

    protected float hp { set; get; }

    protected float atk { set; get; }

    protected float def { set; get; }

    protected float speed { set; get; }

    protected float facingDir { set; get; }
   
    protected virtual void Init()
    {

        sr = this.gameObject.GetComponent<SpriteRenderer>(); 
        
        if (sr == null)
            sr = this.gameObject.AddComponent<SpriteRenderer>();

        anim = this.gameObject.AddComponent<Animator>();
        rb = this.gameObject.AddComponent<Rigidbody2D>();
        bc = this.gameObject.AddComponent<BoxCollider2D>();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    protected virtual void SetInfo(string monsterName)
    {
        anim.runtimeAnimatorController = Resources.Load(monsterName) as RuntimeAnimatorController;
        
        if (anim.runtimeAnimatorController == null)
            print("No such AnimatorController exists in Resources folder");

        creatureState = CreatureState.Idle;
    }

    protected void Flip(){

        this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z); 
        facingDir = facingDir * -1;
    }


    protected virtual void UpdateAnimation()
    {

    }

    protected virtual void UpdateIdle()
    {

    }

    protected virtual void UpdateMove()
    {

    }

    protected virtual void UpdateAttack()
    {

    }

    protected virtual void UpdateDeath()
    {

    }

    public virtual void OnDamaged()
    {

    }

}
