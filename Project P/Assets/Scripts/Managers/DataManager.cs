using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class DataManager
{

    public PlayerData data = new PlayerData();
    public void LoadData() {
        loadDataFromJson();
    }
    public PlayerData GetData() {
        loadDataFromJson();
        return data;
    }
    public void SetInit() {
        data.curStage = 0;
        data.level = 1;
        data.health = 3;
        data.curhealth = data.health;
        data.atk = 5;
        data.speed = 5;
        data.jump = 5;
        data.statPoint = 5;
        saveDataToJson();
    }

    public void SetDataAndSave(PlayerData data_) {
        data = data_;
        saveDataToJson();
    }
    public void tmpSave() {
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
        public int curStage;
        public int level;
        public int health;
        public int curhealth;
        public int atk;
        public float speed;
        public float jump;
        public int statPoint;
    }
}
