using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager instance = null;
    public GameObject obj; 
    PoolManager poolManager = new PoolManager(); 

    public static PoolManager Pool
    {
        get { return instance?.poolManager; } 
    }

    void Awake()
    {
        if(null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); 
        }

        else
        {
            Destroy(this.gameObject); 
        }

    }

    public static Manager Instance
    {
        get
        {
            if(null == instance)
            {
                return null; 
            }

            return instance; 
        }
    }
}
