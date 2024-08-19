using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandOfDeath : BaseObject
{
    private BoxCollider2D bc;
    public LightController lc; 

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
        
        bc = this.gameObject.GetComponent<BoxCollider2D>();
        bc.isTrigger = true;

        lc = GameObject.Find("Main Camera").transform.GetChild(0).GetComponent<LightController>(); 

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
                lc.StartSpotLight(10f); 
                collider.GetComponent<PlayerControl>().OnDamaged();
                
            }
        }
    }

    public void StartSpell(float xPos, float yPos)
    {
        this.transform.position = new Vector3(xPos, yPos, this.transform.position.z); 
        this.gameObject.SetActive(true); 
    }

    void EndSpell()
    {
        Manager.Pool.Push(this.gameObject);
    }
}
