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
    [SerializeField, ReadOnly, Tooltip("存档文件名列表")] List<string> SaveFileNames = new List<string>();
    private void Start()
    {
        Refresh();
    }
    private void Reset()
    {
        Refresh();
    }
    private void Refresh()
    {
        SaveFileNames.Clear();
        foreach (FileInfo item in GetSaveFiles())
        {
            SaveFileNames.Add(item.Name);
        }
    }

    /// <summary>
    /// 获取所有存档的文件名
    /// </summary>
    /// <returns></returns>
    public static List<FileInfo> GetSaveFiles(bool showLog = true)
    {
        List<FileInfo> saveFiles = new List<FileInfo>();

        //设置当前存档
        string fullPath = Application.persistentDataPath;
        if (Directory.Exists(fullPath))
        {
            //获取指定路径下面的所有资源文件
            DirectoryInfo direction = new DirectoryInfo(fullPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".save"))
                {
                    saveFiles.Add(files[i]);
                }
            }
            if (showLog) Debug.Log(string.Format("已检测到 {0} 个存档", saveFiles.Count));
        }
        else
        {
            if (showLog) Debug.LogError("目录不存在 " + fullPath);
        }
        return saveFiles;
    }
    /// <summary>
    /// 获取最近更改的存档文件
    /// </summary>
    /// <returns></returns>
    public static FileInfo GetLatestSaveFile()
    {
        List<FileInfo> saveFiles = GetSaveFiles();
        if (saveFiles.Count > 0)
        {
            List<DateTime> times = new List<DateTime>();
            for (int i = 0; i < saveFiles.Count; i++)
            {
                times.Add(saveFiles[i].LastWriteTime);
            }
            FileInfo fileInfo = saveFiles[Utility.IndexLatest(times)];
            Debug.Log(string.Format("已获取存档 {0}", fileInfo.FullName));
            return fileInfo;
        }
        else
        {
            Debug.Log("未检测到任何存档");
            return null;
        }
    }
    /// <summary>
    /// 获取所有存档
    /// </summary>
    /// <returns></returns>
    public static List<Save> GetSaves()
    {
        List<Save> saves = new List<Save>();
        foreach (FileInfo item in GetSaveFiles(false))
        {
            saves.Add(GetSave(item));
        }
        return saves;
    }
    /// <summary>
    /// 获取存档
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <returns></returns>
    public static Save GetSave(FileInfo fileInfo)
    {
        string json = File.ReadAllText(fileInfo.FullName);
        return JsonUtility.FromJson<Save>(json);
    }

    /// <summary>
    /// 创建存档文件
    /// </summary>
    /// <returns></returns>
    public static FileInfo CreateSaveFile()
    {
        List<FileInfo> fls = GetSaveFiles(false);
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/" + fls.Count + ".save");
        fileInfo.Create();
        Debug.Log(string.Format("已创建存档文件 {0}", fileInfo.FullName));
        Singleton.Refresh();
        return fileInfo;
    }
    /// <summary>
    /// 删除存档文件
    /// </summary>
    public static void RemoveSaveFile(FileInfo fileInfo)
    {
        fileInfo.Delete();
        Singleton.Refresh();
        Debug.Log(string.Format("已删除存档文件 {0}", fileInfo.FullName));
    }

    /// <summary>
    /// 保存游戏到指定文件
    /// </summary>
    public static void SaveTo(FileInfo fileInfo)
    {
        //if (!Directory.Exists(Application.persistentDataPath))
        //{
        //    Directory.CreateDirectory(Application.persistentDataPath);
        //}
        if (!fileInfo.Exists) fileInfo.Create();
        Save save = new Save
        {
            Time = DateTime.Now,
            PlayerDynamicData = Player.Myself.DynamicData,
            PlayerPosition = Player.Myself.transform.position,
        };
        string json = JsonUtility.ToJson(save);
        File.WriteAllText(fileInfo.FullName, json);

        AssetDatabase.Refresh();
        Debug.Log("存档成功：" + fileInfo.FullName + " 时间：" + save.Time);
        Debug.Log("存档内容：" + json);
    }
    /// <summary>
    /// 从指定文件载入游戏
    /// </summary>
    public static void LoadFrom(FileInfo fileInfo)
    {
        if (fileInfo.Exists)
        {
            //获取存档信息
            Save save = GetSave(fileInfo);
            //配置对应游戏对象
            Player.Myself.SetDynamicData(save.PlayerDynamicData);
            Player.Myself.transform.position = save.PlayerPosition;

            Debug.Log("读档成功：" + fileInfo.FullName);
        }
        else
        {
            Debug.Log("读档失败，存档文件不存在：" + fileInfo.FullName);
        }
    }
    /// <summary>
    /// 保存游戏
    /// </summary>
    public static void Save()
    {
        SaveTo(GetLatestSaveFile());
    }
    /// <summary>
    /// 载入游戏
    /// </summary>
    public static void Load()
    {
        LoadFrom(GetLatestSaveFile());
    }
}

[Serializable]
public struct Save
{
    public DateTime Time;
    public Vector2 PlayerPosition;
    public CharacterDynamicData PlayerDynamicData;
}
