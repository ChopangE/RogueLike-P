using System.Collections;
using System.Collections.Generic;
using UGS;
using UnityEngine;

public class Testing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UnityGoogleSheet.Load<GameBalance.Player>();

        foreach(var element in GameBalance.Player.PlayerList) {
            Debug.Log(element.Attack+ " " + element.Speed + " "+ element.Health +" " + element.Jump);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
