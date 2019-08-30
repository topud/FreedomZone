using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using E.Tool;

public class GameManager : SingletonPattern<GameManager>
{
    [Header("状态")]
    [ReadOnly] public bool IsInLobby = true;
    public bool IsShowUIInGame = true;
    public bool IsAcceptInputInGame = true;

    public AsyncOperation SsceneAsyncOperation;
    public float SceneLoadProcess
    {
        get
        {
            if (SsceneAsyncOperation != null)
            {
                //progress的值最大为0.9  
                if (SsceneAsyncOperation.progress >= 0.9f)
                {
                    return 1.0f;
                }
                else
                {
                    return SsceneAsyncOperation.progress;
                }
            }
            else
            {
                return -1;
            }
        }
    }
    private bool isNewGame = false;

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
        }
        else
        {
            //游戏内
            if (Input.GetKeyUp(KeyCode.KeypadEnter))
            {
                if (isNewGame)
                {
                    if (Player.Myself == null)
                    {
                        EntityManager.Singleton.CheckSceneEntity();
                        Player player = EntityManager.Singleton.SpawnPlayer((CharacterStaticData)CharacterStaticData.GetValue("库娅"), new Vector2(5, 5));
                        Npc npc = EntityManager.Singleton.SpawnNpc((CharacterStaticData)CharacterStaticData.GetValue("从人"), new Vector2(10, 5));
                        npc.FollowTarget = player.transform;
                        Debug.Log("游戏初始化");
                    }
                }
                else
                {
                    SaveManager.Load();
                }
            }
            else if (Input.GetKeyUp(KeyCode.LeftBracket))
            {
            }
            else if (Input.GetKeyUp(KeyCode.RightBracket))
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
        SsceneAsyncOperation = SceneManager.LoadSceneAsync(name);
        SsceneAsyncOperation.allowSceneActivation = false;

        yield return SsceneAsyncOperation;
    }

    public void StartNewGame()
    {
        LoadScene("Game", true);
        isNewGame = true;
    }
    public void ContinueLastGame()
    {
        LoadScene("Game", true);
        isNewGame = false;
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