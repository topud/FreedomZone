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

    private void OnEnable()
    {
        SaveFolder = Application.persistentDataPath;
    }
    private void Start()
    {
        //选择当前存档文件
        string lastSaveFile = "";
        if (PlayerPrefs.HasKey("CurrentSaveFile")) lastSaveFile = PlayerPrefs.GetString("CurrentSaveFile");
        if (!IsSaveFileExists(lastSaveFile))
        {
            List<string> ss = GetSaveFiles();
            if (ss.Count > 0)
            {
                lastSaveFile = ss[0];
            }
            else
            {
                lastSaveFile = "1.save";
            }
        }
        CurrentSaveFile = lastSaveFile;
        OnValidate();
    }
    private void Reset()
    {
        CurrentSaveFile = "1.save";
        SaveFolder = Application.persistentDataPath;
        OnValidate();
    }
    private void OnValidate()
    {
        CurrentSaveFilePath = SaveFolder + "/" + CurrentSaveFile;
    }

    /// <summary>
    /// 获取所有存档的文件名
    /// </summary>
    /// <returns></returns>
    public List<string> GetSaveFiles()
    {
        List<string> saves = new List<string>();

        //设置当前存档
        string fullPath = SaveFolder + "/";
        if (Directory.Exists(fullPath))
        {
            //获取指定路径下面的所有资源文件
            DirectoryInfo direction = new DirectoryInfo(fullPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".save"))
                {
                    saves.Add(files[i].Name);
                }
            }
            Debug.Log(string.Format("已检测到 {0} 个存档", saves.Count));
        }
        return saves;
    }
    /// <summary>
    /// 检测存档是否存在
    /// </summary>
    /// <param name="save"></param>
    /// <returns></returns>
    public bool IsSaveFileExists(string save)
    {
        foreach (string item in GetSaveFiles())
        {
            if (item == save)
            {
                return true;
            }
        }
        Debug.Log(save + " 存档不存在");
        return false;
    }

    /// <summary>
    /// 保存游戏
    /// </summary>
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
    /// <summary>
    /// 载入游戏
    /// </summary>
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
