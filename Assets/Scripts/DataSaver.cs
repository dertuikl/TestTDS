using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class DataSaver
{
    private readonly string savePath = Application.persistentDataPath;
    private string FilePathForKey(string key) => Path.Combine(savePath, key + ".json");

    public void Save(string key, BoardSaveData saveData)
    {
        string filePath = FilePathForKey(key);
        string json = JsonUtility.ToJson(saveData);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
        File.Create(filePath).Write(bytes, 0, bytes.Length);
    }

    public BoardSaveData Load(string key)
    {
        string filePath = FilePathForKey(key);
        if (File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);
            BoardSaveData savedData = JsonUtility.FromJson<BoardSaveData>(json);
            return savedData;
        }
        
        return null;
    }
}