using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trap : BaseObject
{

    Collider2D _collider; 

    protected override void Init()
    {
        base.Init();

        rb.gravityScale = 0f;
        _collider = this.gameObject.GetComponent<Collider2D>();
        _collider.isTrigger= true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerControl>().OnDamaged(); 
            }
        }
    }
}
