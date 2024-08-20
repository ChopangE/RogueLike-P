using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManager_ : MonoBehaviour
{
    private static SceneManager_ instance;
    public static SceneManager_ Instance {
        get {
            if (instance == null) {
                var obj = FindObjectOfType<SceneManager_>();
                if (obj != null) {
                    instance = obj;
                }
            }
            else {
                var newObj = new GameObject().AddComponent<SceneManager_>();
                instance = newObj;
            }
            return instance;
        }
    }
    void Awake() {
        var objs = FindObjectsOfType<SceneManager_>();
        if (objs.Length != 1) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    
    public void CallScene(int idx) {
        SceneManager.LoadScene(idx);
    }
}
