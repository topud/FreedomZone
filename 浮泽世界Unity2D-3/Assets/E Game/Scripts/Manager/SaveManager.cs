using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using E.Tool;

public class SaveManager : MonoBehaviour
{
    [ReadOnly, Tooltip("当前存档")] public FileInfo currentSave;
    [SerializeField, ReadOnly, Tooltip("存档文件名列表")] List<string> saveNames = new List<string>();

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
        saveNames.Clear();
        foreach (FileInfo item in GetSaveFiles())
        {
            saveNames.Add(item.Name);
        }
    }

    /// <summary>
    /// 获取所有存档文件
    /// </summary>
    /// <returns></returns>
    public List<FileInfo> GetSaveFiles()
    {
        List<FileInfo> saveFiles = new List<FileInfo>();
        
        string fullPath = Application.persistentDataPath;
        if (Directory.Exists(fullPath))
        {
            //获取指定路径下面的所有资源文件
            DirectoryInfo direction = new DirectoryInfo(fullPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".json"))
                {
                    saveFiles.Add(files[i]);
                }
            }
        }
        else
        {
            Debug.LogError("目录不存在 " + fullPath);
        }
        return saveFiles;
    }
    /// <summary>
    /// 获取最近更改的存档文件
    /// </summary>
    /// <returns></returns>
    public FileInfo GetLatestSaveFile()
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
    public List<Save> GetSaves()
    {
        List<Save> saves = new List<Save>();
        foreach (FileInfo item in GetSaveFiles())
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
    public Save GetSave(FileInfo fileInfo)
    {
        if (!fileInfo.Exists)
        {
            Debug.LogError("存档文件不存在 " + fileInfo.FullName);
            return null;
        }
        string json = File.ReadAllText(fileInfo.FullName);
        Save save = JsonUtility.FromJson<Save>(json);
        return save;
    }

    /// <summary>
    /// 创建存档文件
    /// </summary>
    /// <returns></returns>
    public FileInfo CreateSaveFile()
    {
        int n = 0;
        string path;
        do
        {
            path = Application.persistentDataPath + "/" + n + ".json";
            n++;
        }
        while (new FileInfo(path).Exists);

        FileInfo fileInfo = new FileInfo(path);
        fileInfo.Create().Dispose();
        RefreshNames();

        Debug.Log(string.Format("已创建存档文件 {0}", fileInfo.FullName));
        return fileInfo;
    }
    /// <summary>
    /// 创建并保存
    /// </summary>
    public void CreateSaveFileAndSave()
    {
        currentSave = GameManager.Save.CreateSaveFile();
        SaveTo(currentSave);
    }

    /// <summary>
    /// 删除存档文件
    /// </summary>
    public void RemoveSaveFile(FileInfo fileInfo)
    {
        fileInfo.Delete();
        RefreshNames();

        Debug.Log(string.Format("已删除存档文件 {0}", fileInfo.FullName));
    }
    /// <summary>
    /// 删除所有存档文件
    /// </summary>
    public void RemoveAllSaveFile()
    {
        List<FileInfo> saveFiles = new List<FileInfo>();
        foreach (FileInfo item in saveFiles)
        {
            item.Delete();
        }
        RefreshNames();

        Debug.Log(string.Format("已删除所有存档文件，共计 {0} 个", saveFiles.Count));
    }

    /// <summary>
    /// 快捷保存游戏到最近存档
    /// </summary>
    public void Save()
    {
        SaveTo(GetLatestSaveFile());
    }
    /// <summary>
    /// 保存游戏到指定文件
    /// </summary>
    public void SaveTo(FileInfo fileInfo)
    {
        if (fileInfo == null)
        {
            Debug.LogError("存档失败，未指定FileInfo");
            return;
        }
        if (!fileInfo.Exists)
        {
            Debug.LogError("存档失败，指定文件不存在 " + fileInfo.FullName);
            return;
        }
        if (!GameManager.Character.Player)
        {
            Debug.LogError("存档失败，玩家对象没有生成");
            return;
        }

        Save save;
        save = new Save
        {
            NodeID = GameManager.Story.CurrentNodeID
        };
        foreach (Character item in GameManager.Character.Characters)
        {
            save.CharacterDynamicDatas.Add(item.DynamicData);
        }
        foreach (Item item in GameManager.Item.Items)
        {
            save.InteractorDynamicDatas.Add(item.DynamicData);
        }
        
        string json = JsonUtility.ToJson(save, true);
        File.WriteAllText(fileInfo.FullName, json);

        AssetDatabase.Refresh();
        Debug.Log("存档成功：" + fileInfo.FullName + " 时间：" + save.NodeID);
        Debug.Log("存档内容：" + json);
    }

    /// <summary>
    /// 载入游戏从指定文件
    /// </summary>
    public void LoadFrom(FileInfo fileInfo)
    {
        if (!fileInfo.Exists)
        {
            Debug.LogError("读档失败，指定文件不存在：" + fileInfo.FullName);
            return;
        }

        //获取存档信息
        Save save = GetSave(fileInfo);
        //配置对应游戏对象
        GameManager.Story.CurrentNodeID = save.NodeID;
        foreach (RoleDynamicData item in save.CharacterDynamicDatas)
        {
            Character character = GameManager.Character.GetCharacter(item.nameID);
            if (character)
            {
                character.SetDynamicData(item);
            }
            else
            {
                GameManager.Character.SpawnCharacter(item);
            }
        }
        foreach (ItemDynamicData item in save.InteractorDynamicDatas)
        {
            Item it = GameManager.Item.GetItem(item.nameID);
            if (it)
            {
                it.SetDynamicData(item);
            }
            else
            {
                GameManager.Item.SpawnItem(item);
            }
        }

        Debug.Log("读档成功：" + fileInfo.FullName);
    }
    /// <summary>
    /// 快捷载入游戏从最近存档
    /// </summary>
    public void Load()
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
    public PlotID NodeID = new PlotID(0,0,0,0);
    public List<RoleDynamicData> CharacterDynamicDatas = new List<RoleDynamicData>();
    public List<ItemDynamicData> InteractorDynamicDatas = new List<ItemDynamicData>();
}
