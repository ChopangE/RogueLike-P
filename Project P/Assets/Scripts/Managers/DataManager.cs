using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public PlayerData data;
    public static DataManager Instance {
        get {
            if (instance == null) {
                var obj = FindObjectOfType<DataManager>();
                if (obj != null) {
                    instance = obj;
                }
            }
            else {
                var newObj = new GameObject().AddComponent<DataManager>();
                instance = newObj;
            }
            return instance;
        }

    }
    void Awake() {
        var objs = FindObjectsOfType<DataManager>();
        if (objs.Length != 1) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void LoadContinue() {
        loadDataFromJson();
    }
    public void SetInit() {
        data.level = 1;
        data.health = 3;
        data.atk = 3;
        data.speed = 3;
        data.jump = 3;
        saveDataToJson();
    }

    public void SetData(PlayerData data_) {
        data = data_;
        saveDataToJson();
    }
    void saveDataToJson() {
        string result = JsonUtility.ToJson(data);
        string path = Path.Combine(Application.dataPath, "playerData.json");
        File.WriteAllText(path, result);
        Debug.Log("Success");
    }

    void loadDataFromJson() {
        string path = Path.Combine(Application.dataPath, "playerData.json");
        string jsonData = File.ReadAllText(path);
        data = JsonUtility.FromJson<PlayerData>(jsonData);
    }

    [System.Serializable]
    public class PlayerData {
        public int level;
        public int health;
        public int atk;
        public float speed;
        public float jump;
        
    }
}
