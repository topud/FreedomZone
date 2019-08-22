using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using E.Tool;

public class SaveManager : SingletonPattern<SaveManager>
{
    public string CurrentSaveFile = "1.save";
    [SerializeField, ReadOnly] private string SaveFolder;
    [SerializeField, ReadOnly] private string CurrentSaveFilePath;
    
    private void Start()
    {
        Reset();
    }
    private void Reset()
    {
        SaveFolder = Application.persistentDataPath;
        CurrentSaveFilePath = SaveFolder + "/" + CurrentSaveFile;
    }
    private void OnValidate()
    {
        CurrentSaveFilePath = SaveFolder + "/" + CurrentSaveFile;
    }

    public void SaveGame()
    {
        if (!Directory.Exists(SaveFolder))
        {
            Directory.CreateDirectory(SaveFolder);
        }

        Save save = new Save
        {
            Time = DateTime.Now,
            PlayerDynamicData = Player.Myself.DynamicData,
            PlayerPosition = Player.Myself.transform.position,
        };
        string json = JsonUtility.ToJson(save);
        File.WriteAllText(CurrentSaveFilePath, json);

        AssetDatabase.Refresh();
        Debug.Log("存档成功：" + CurrentSaveFilePath + " 时间：" + save.Time);
        Debug.Log("存档内容：" + json);
    }
    public void LoadGame()
    {
        if (File.Exists(CurrentSaveFilePath))
        {
            string json = File.ReadAllText(CurrentSaveFilePath);
            Save save = JsonUtility.FromJson<Save>(json);

            Player.Myself.SetDynamicData(save.PlayerDynamicData);
            Player.Myself.transform.position = save.PlayerPosition;

            Debug.Log("读档成功：" + CurrentSaveFilePath);
            Debug.Log("读档内容" + json);
        }
        else
        {
            Debug.Log("读档失败，存档文件不存在：" + CurrentSaveFilePath);
        }
    }
}

[Serializable]
public struct Save
{
    public DateTime Time;
    public Vector2 PlayerPosition;
    public CharacterDynamicData PlayerDynamicData;
}
