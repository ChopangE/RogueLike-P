using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TempCamera : MonoBehaviour
{

    public GameObject player;

    public GameObject currentMap;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public float halfHeight; 
    public float halfWidth;

    // Start is called before the first frame update
    void Start()
    {
        halfHeight = this.GetComponent<Camera>().orthographicSize;
        halfWidth = halfHeight * this.GetComponent<Camera>().aspect; 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z); 
        
        if (currentMap == null)
        {
            this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
            Debug.Log("Null");
        }

        else
        {
            minX = currentMap.GetComponent<BoxCollider2D>().bounds.min.x;
            maxX = currentMap.GetComponent<BoxCollider2D>().bounds.max.x;
            minY = currentMap.GetComponent<BoxCollider2D>().bounds.min.y; 
            maxY = currentMap.GetComponent<BoxCollider2D>().bounds.max.y;

            Vector3 boundPosition = new Vector3(Mathf.Clamp(targetPos.x, minX + halfWidth, maxX - halfWidth), Mathf.Clamp(targetPos.y, minY + halfHeight, maxY - halfHeight), this.transform.position.z);

            this.transform.position = boundPosition; 
        }
    }

}
