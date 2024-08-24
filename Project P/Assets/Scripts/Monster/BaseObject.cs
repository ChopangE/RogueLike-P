using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Creature;

public class BaseObject : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;

    protected float dir; 

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        anim = this.gameObject.GetComponent<Animator>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();

        if (anim == null)
            anim = this.gameObject.AddComponent<Animator>();
        if (rb == null)
            rb = this.gameObject.AddComponent<Rigidbody2D>();


        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    protected virtual void SetInfo(string animatorName)
    {
        anim.runtimeAnimatorController = Resources.Load(animatorName) as RuntimeAnimatorController;

        if (anim.runtimeAnimatorController == null)
            Debug.Log("No such AnimatorController exists in Resources folder");
    }

    protected void Flip()
    {

        this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
        dir *= -1;
    }

    public void PlayAnimation(string animationName)
    {
        anim.Play(animationName);
    }
}
