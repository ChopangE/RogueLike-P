using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Meteo : BaseObject
{
    [SerializeField]
    CircleCollider2D cc;

    [SerializeField]
    float speed;

    [SerializeField]
    GameObject collisionEffect;

    float timer; 
    
    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        speed = Random.Range(1, 4f);
        timer = 0; 

    }

    private void Update()
    {
        rb.velocity = new Vector2(-1f, -1f) * speed;

        timer += Time.deltaTime;

        if (timer >= 5f)
        {
            GameObject _go = Manager.Pool.Pop(collisionEffect);
            _go.transform.position = cc.bounds.center;

            Manager.Pool.Push(this.gameObject);
        }

    }

    protected override void Init()
    {
        base.Init();

        cc = this.gameObject.GetComponent<CircleCollider2D>();
        cc.isTrigger = true;

        speed = Random.Range(1, 4f); 
        rb.gravityScale = 0f;

        timer = 0f; 
    }

    public void StartMeteo(float xPos, float yPos)
    {
        this.gameObject.SetActive(true);
        this.transform.position = new Vector3(xPos, yPos, transform.position.z); 
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.GetComponent<PlayerControl>().OnDamaged();
            GameObject _go = Manager.Pool.Pop(collisionEffect);
            _go.transform.position = cc.bounds.center;

            Manager.Pool.Push(this.gameObject);
        }

        if (collision.gameObject.tag == "Ground")
        {
            GameObject _go = Manager.Pool.Pop(collisionEffect);
            _go.transform.position = cc.bounds.center; 
            Manager.Pool.Push(this.gameObject);
        }
    }
}
