using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public Collider2D platForm;
    void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), platForm, true);
            Debug.Log("Good");
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            Physics2D.IgnoreCollision(collision, platForm, false);
        }
    }
}
