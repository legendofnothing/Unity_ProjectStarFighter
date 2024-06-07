using System.IO;
using UnityEngine;

public static class SaveSystem {
    public static T GetData<T>(string key) where T : new() {
        if (PlayerPrefs.HasKey(key)) {
            var obj = JsonUtility.FromJson<T>(PlayerPrefs.GetString(key));
            return obj;
        }

        Debug.Log($"No key found: {key}, generating default value");
        SaveData(new T(), key);
        return new T();
    } 
        
    public static void SaveData(object toSave, string key) {
        var jsonString = JsonUtility.ToJson(toSave);
        PlayerPrefs.SetString(key, jsonString);
    }

    public static void ClearKey(string key) {
        PlayerPrefs.DeleteKey(key);
    }

    public static void ClearAll() {
        PlayerPrefs.DeleteAll();
    }
}