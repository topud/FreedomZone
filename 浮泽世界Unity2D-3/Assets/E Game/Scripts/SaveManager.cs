// ========================================================
// 作者：E Star
// 创建时间：2019-01-27 01:41:08
// 当前版本：1.0
// 作用描述：
// 挂载目标：
// ========================================================
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
    [SerializeField, ReadOnly] private string SaveFolder;
    [SerializeField, ReadOnly] string CurrentSaveFilePath;
    public string CurrentSaveFile = "1.save";


    private void Start()
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

            Player.Myself.SetData(save.PlayerDynamicData);
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
