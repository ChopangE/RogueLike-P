using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Monster_Detection : MonoBehaviour
{
    public UnityEvent<GameObject> playerDetected;
    public UnityEvent playerLost; 

    void Start()
    {
        Init();
    }

    void Init()
    {
        BoxCollider2D bc = this.AddComponent<BoxCollider2D>();
        bc.isTrigger = true;
        bc.size = new Vector2(7, 2); 
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Player Found!"); 
            playerDetected.Invoke(collision.gameObject); 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerLost.Invoke(); 
        }
    }
}
