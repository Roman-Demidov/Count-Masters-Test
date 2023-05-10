using UnityEngine;
using System.IO;

public static class DataHandler
{
    private static readonly string dataPath = Application.persistentDataPath + "/";

    public static void Save<T>(T data, string dataName) where T : class
    {
        string json = JsonUtility.ToJson(data, true);
        string filePath = dataPath + dataName + ".json";

        try
        {
            File.WriteAllText(filePath, json);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error saving data: " + e.Message);
        }
    }

    public static T Load<T>(string dataName) where T : class
    {
        string filePath = dataPath + dataName + ".json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(json);
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
            return null;
        }
    }
}