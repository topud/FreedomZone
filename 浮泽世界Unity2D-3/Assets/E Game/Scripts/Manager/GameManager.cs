﻿using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using E.Tool;

public class GameManager : SingletonClass<GameManager>
{
    [Header("状态")]
    [ReadOnly] public bool IsInLobby = true;
    public bool IsShowUIInGame = true;
    public bool IsAcceptInputInGame = true;

    private bool isStartGame = false;
    private FileInfo selectSave;

    private AsyncOperation sceneAsyncOperation;
    public float SceneLoadProcess
    {
        get
        {
            if (sceneAsyncOperation != null)
            {
                //progress的值最大为0.9  
                return sceneAsyncOperation.progress >= 0.9f ? 1.0f :sceneAsyncOperation.progress;
            }
            else
            {
                return -1;
            }
        }
    }

    protected override void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        IsInLobby = SceneManager.GetActiveScene().name == "Lobby" ? true : false;
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (Character.Player == null)
            {
                Character player = CharacterManager.GetCharacter("璃亚");
                player.IsPlayer = true;
                Debug.Log("本次运行为调试模式，已自动添加可控制角色");
            }
        }
        UIManager.Singleton.SetUIActive(true);
    }
    private void Update()
    {
        IsInLobby = SceneManager.GetActiveScene().name == "Lobby" ? true : false;

        if (SceneLoadProcess >= 0.9f)
        {
            //允许异步加载完毕后自动切换场景  
            sceneAsyncOperation.allowSceneActivation = true;
            UIManager.Singleton.HideLoading();
            //开始游戏
            if (isStartGame && sceneAsyncOperation.isDone)
            {
                //未选择存档时开始新存档
                if (selectSave == null)
                {
                    if (Character.Player == null)
                    {
                        Character player = CharacterManager.SpawnCharacter("库娅", new Vector2(165, 5), true);
                        Character npc = CharacterManager.SpawnCharacter("从人", new Vector2(170, 10));
                        npc.FollowTarget = player.transform;
                        Debug.Log("新存档初始化");
                    }
                }
                //否则载入存档
                else
                {
                    SaveManager.LoadFrom(selectSave);
                    Debug.Log("载入存档初始化");
                }
                isStartGame = false;
            }
        }

# if UNITY_EDITOR
        if (IsInLobby)
        {
        }
        else
        {
            //游戏内
            if (Input.GetKeyUp(KeyCode.KeypadEnter))
            {
            }
        }
#endif
    }

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="name"></param>
    /// <param name="useLoadUI"></param>
    public void LoadScene(string name, bool useLoadUI = false)
    {
        if (SceneManager.GetSceneByName(name) == null)
        {
            Debug.LogError("请求加载的场景不存在");
            return;
        }
        if (useLoadUI)
        {
            UIManager.Singleton.ShowLoading();
        }
        StartCoroutine(AsyncLoading(name));
    }
    private IEnumerator AsyncLoading(string name)
    {
        sceneAsyncOperation = SceneManager.LoadSceneAsync(name);
        sceneAsyncOperation.allowSceneActivation = false;

        yield return sceneAsyncOperation;
    }

    public void StartNewSave()
    {
        LoadScene("Game", true);
        isStartGame = true;
        selectSave = null;
    }
    public void ContinueLastSave()
    {
        LoadScene("Game", true);
        isStartGame = true;
        selectSave = SaveManager.GetLatestSaveFile();
    }
    public void ContinueSelectSave(FileInfo fileInfo)
    {
        if (fileInfo == null)
        {
            Debug.LogError("未指定存档文件");
            return;
        }
        LoadScene("Game", true);
        isStartGame = true;
        selectSave = fileInfo;
    }
    public void BackToLobby()
    {
        LoadScene("Lobby", false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}