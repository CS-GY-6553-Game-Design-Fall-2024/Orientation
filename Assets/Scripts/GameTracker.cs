using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;

public class GameTracker : MonoBehaviour
{
    /*
    public JSONWriter<UserData> jsonWriter;

    [DllImport("__Internal")]
    public static extern void SyncFS();

    private void Start() {
        string pathToSaveData = System.IO.Path.Combine(Application.persistentDataPath,"data.json");
        jsonWriter = new JsonFileWriter<PlayerStatistics>(pathToSaveData);
    }

    public PlayerStatistics Load() {
        return _jsonFileWriter.DeserializeData();
    }

    public void Save(PlayerStatistics playerStatistics) {
        _jsonFileWriter.SerializeData(playerStatistics);

        #if UNITY_WEBGL && !UNITY_EDITOR
            SyncFS();
        #endif
    }
    */
}

[System.Serializable]
public class JSONWriter<JsonData> {
    public string filepath;
    public JSONWriter(string path) {
        this.filepath = path;
    }

    public void SerializeData(JsonData data) {
        string jsonDataString = JsonUtility.ToJson(data, true);
        File.WriteAllText(this.filepath, jsonDataString);
    }

    public JsonData DeserializeData() {
        if (File.Exists(this.filepath)) {
            string loadedJsonDataString = File.ReadAllText(this.filepath);
            return JsonUtility.FromJson<JsonData>(loadedJsonDataString);
        }
        return default(JsonData);
    }
}

[System.Serializable]
public class UserData {
    public int seed;
    public List<UserRow> positions;
    public UserData(int seed) {
        this.seed = seed;
        this.positions = new List<UserRow>();
    }
    public void AddRow(UserRow row) {
        this.positions.Add(row);
    }
}

[System.Serializable]
public class UserRow {
    public Vector3 position;
    public Vector3 forward;
    public Vector3 noisemapPosition;
    public int mapHeld;
    public UserRow(Vector3 position, Vector3 forward, Vector3 noisemapPosition, int mapHeld) {
        this.position = position;
        this.forward = forward;
        this.noisemapPosition = noisemapPosition;
        this.mapHeld = mapHeld;
    }
}
