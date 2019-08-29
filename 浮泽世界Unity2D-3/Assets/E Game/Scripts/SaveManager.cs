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
        RefreshNames();
    }
    private void Reset()
    {
        RefreshNames();
    }
    private void RefreshNames()
    {
        SaveFileNames.Clear();
        foreach (FileInfo item in GetSaveFiles(false))
        {
            SaveFileNames.Add(item.Name);
        }
    }

    /// <summary>
    /// 获取所有存档文件
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
        List<FileInfo> saveFiles = GetSaveFiles(false);
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
            Debug.Log("没有最近更改的存档");
            return null;
        }
    }
    /// <summary>
    /// 从所有存档文件中获取内容
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
    /// 从存档文件中获取内容
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <returns></returns>
    public static Save GetSave(FileInfo fileInfo)
    {
        string json = File.ReadAllText(fileInfo.FullName);
        Save save = JsonUtility.FromJson<Save>(json);
        return save;
    }

    /// <summary>
    /// 创建存档文件
    /// </summary>
    /// <returns></returns>
    public static FileInfo CreateSaveFile()
    {
        List<FileInfo> fls = GetSaveFiles(false);
        int n = 0;
        string path;
        do
        {
            path = Application.persistentDataPath + "/" + n + ".save";
            n++;
        }
        while (new FileInfo(path).Exists);
        FileInfo fileInfo = new FileInfo(path);
        fileInfo.Create();
        Debug.Log(string.Format("已创建存档文件 {0}", fileInfo.FullName));
        Singleton.RefreshNames();
        return fileInfo;
    }

    /// <summary>
    /// 删除存档文件
    /// </summary>
    public static void RemoveSaveFile(FileInfo fileInfo)
    {
        fileInfo.Delete();
        Singleton.RefreshNames();
        Debug.Log(string.Format("已删除存档文件 {0}", fileInfo.FullName));
    }

    /// <summary>
    /// 快捷保存游戏到最近存档
    /// </summary>
    public static void Save()
    {
        SaveTo(GetLatestSaveFile());
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
        Save save;
        if (fileInfo.Exists)
        {
            save = new Save
            {
                NodeID = StoryManager.Singleton.NodeID,
                PlayerPosition = Player.Myself.transform.position,
                PlayerDynamicData = Player.Myself.DynamicData,
            };
            string json = JsonUtility.ToJson(save);
            File.WriteAllText(fileInfo.FullName, json);

            AssetDatabase.Refresh();
            Debug.Log("存档成功：" + fileInfo.FullName + " 时间：" + save.NodeID);
            Debug.Log("存档内容：" + json);
        }
        else
        {
            Debug.LogError("存档失败，指定文件不存在：" + fileInfo.FullName);
        }
    }
    /// <summary>
    /// 载入游戏从指定文件
    /// </summary>
    public static void LoadFrom(FileInfo fileInfo)
    {
        if (fileInfo.Exists)
        {
            //获取存档信息
            Save save = GetSave(fileInfo);
            //配置对应游戏对象
            StoryManager.Singleton.NodeID = save.NodeID;
            Player.Myself.transform.position = save.PlayerPosition;
            Player.Myself.SetDynamicData(save.PlayerDynamicData);

            Debug.Log("读档成功：" + fileInfo.FullName);
        }
        else
        {
            Debug.LogError("读档失败，指定文件不存在：" + fileInfo.FullName);
        }
    }
    /// <summary>
    /// 快捷载入游戏从最近存档
    /// </summary>
    public static void Load()
    {
        LoadFrom(GetLatestSaveFile());
    }

    [MenuItem("Tools/E Save/打开存档文件夹")]
    public static void OpenSaveFolder()
    {
        string path = Application.persistentDataPath;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        Debug.Log("打开存档文件夹 " + path);
        path = path.Replace("/", "\\");
        System.Diagnostics.Process.Start("explorer.exe", path);
    }
}

[Serializable]
public class Save
{
    public NodeID NodeID = new NodeID(0,0,0,0,0);
    public Vector2 PlayerPosition = new Vector2(0,0);
    public CharacterDynamicData PlayerDynamicData = new CharacterDynamicData();
}
