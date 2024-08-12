using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandOfDeath : BaseObject
{

    private void Awake()
    {
        Init(); 
    }

    private void OnEnable()
    {
        anim.Play("Idle"); 
    }

    protected override void Init()
    {
        base.Init(); 

        bc.isTrigger = true;
        rb.gravityScale = 0f; 
        anim.Play("Idle");
    }

    private void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 2f)
        {
            anim.Play("Attack"); 
        }
    }

    void OnHit()
    {
        Vector2 pos = new Vector2(bc.bounds.center.x, bc.bounds.center.y);
        Vector2 size = bc.bounds.size; 

        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos, size, 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Player")
            {
                collider.GetComponent<PlayerControl>().OnDamaged();
            }
        }
    }

    public void StartSpell(float Xpos)
    {
        this.transform.position = new Vector3(Xpos, this.transform.position.y, this.transform.position.z); 
        this.gameObject.SetActive(true); 
    }

    void EndSpell()
    {
        this.gameObject.SetActive(false);
    }
}
