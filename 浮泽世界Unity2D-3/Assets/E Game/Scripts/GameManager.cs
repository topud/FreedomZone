using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using E.Tool;

public class GameManager : SingletonPattern<GameManager>
{
    [Header("状态")]
    public bool IsInLobby = true;
    public bool IsShowUIInGame = true;
    public bool IsAcceptInputInGame = true;

    private AsyncOperation sceneAsyncOperation;
    public float SceneLoadProcess
    {
        get
        {
            if (sceneAsyncOperation != null)
            {
                //progress的值最大为0.9  
                if (sceneAsyncOperation.progress >= 0.9f)
                {
                    return 1.0f;
                }
                else
                {
                    return sceneAsyncOperation.progress;
                }
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
    }
    private void Update()
    {
        IsInLobby = SceneManager.GetActiveScene().name == "Lobby" ? true : false;

# if UNITY_EDITOR
        if (IsInLobby)
        {
            //大厅内
            if (Input.GetKeyUp(KeyCode.KeypadEnter))
            {
                StartNewGame();
            }
        }
        else
        {
            //游戏内
            if (Input.GetKeyUp(KeyCode.KeypadEnter))
            {
            }
            else if (Input.GetKeyUp(KeyCode.LeftBracket))
            {
            }
            else if (Input.GetKeyUp(KeyCode.RightBracket))
            {
            }
        }
#endif
        
        if ((int)(SceneLoadProcess * 100) == 100)
        {
            //允许异步加载完毕后自动切换场景  
            sceneAsyncOperation.allowSceneActivation = true;
            UIManager.Singleton.HideLoading();
        }
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
        StartCoroutine(AsyncLoading(name));
        UIManager.Singleton.ShowLoading();
    }
    private IEnumerator AsyncLoading(string name)
    {
        sceneAsyncOperation = SceneManager.LoadSceneAsync(name);
        sceneAsyncOperation.allowSceneActivation = false;

        yield return sceneAsyncOperation;
    }

    public void StartNewGame()
    {
        LoadScene("Game", true);

        Player player = SpawnManager.Singleton.SpawnPlayer((CharacterStaticData)CharacterStaticData.GetValue("库娅"), new Vector2(5, 5));
        Npc npc = SpawnManager.Singleton.SpawnNpc((CharacterStaticData)CharacterStaticData.GetValue("从人"), new Vector2(10, 5));
        npc.FollowTarget = player.transform;
    }
    public void ContinueLastGame()
    {
        LoadScene("Game", true);

        Player player = SpawnManager.Singleton.SpawnPlayer((CharacterStaticData)CharacterStaticData.GetValue("库娅"), new Vector2(5, 5));
        Npc npc = SpawnManager.Singleton.SpawnNpc((CharacterStaticData)CharacterStaticData.GetValue("从人"), new Vector2(10, 5));
        npc.FollowTarget = player.transform;

        SaveManager.Load();
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