using Newtonsoft.Json;
using System.IO;

public static class JsonParser
{
    public static T LoadJsonFromFile<T>(string filePath)
    {
        string json = File.ReadAllText(filePath);
        T item = JsonConvert.DeserializeObject<T>(json);

        return item;
    }

    public static void SaveJsonToFile<T>(T objectToSave, string filePath)
    {
        string jsonString = JsonConvert.SerializeObject(objectToSave);
        File.WriteAllText(filePath, jsonString);
    }
}
