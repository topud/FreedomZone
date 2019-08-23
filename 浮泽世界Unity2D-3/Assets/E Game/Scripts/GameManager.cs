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
            if (Input.GetKeyUp(KeyCode.KeypadEnter))
            {
                LoadScene("Game", true);
            }
            if (Input.GetKeyUp(KeyCode.LeftBracket))
            {
                SaveManager.CreateSaveFile();
            }
            else if (Input.GetKeyUp(KeyCode.RightBracket))
            {
                SaveManager.Save();
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.KeypadEnter))
            {
                Player player = SpawnManager.Singleton.SpawnPlayer((CharacterStaticData)CharacterStaticData.GetValue("库娅"), new Vector2(5, 5));
                Npc npc = SpawnManager.Singleton.SpawnNpc((CharacterStaticData)CharacterStaticData.GetValue("从人"), new Vector2(10, 5));
                npc.FollowTarget = player.transform;
            }
            else if (Input.GetKeyUp(KeyCode.LeftBracket))
            {
                SaveManager.CreateSaveFile();
            }
            else if (Input.GetKeyUp(KeyCode.RightBracket))
            {
                //SaveManager.Singleton.LoadGame();
            }
        }
# endif
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

        //UIManager.Singleton.LoadUI.SetActive(useLoadUI);
        SceneManager.LoadScene(name);
    }


    public void StartNewGame()
    {
    }
    public void ContinueLastGame()
    {
    }
    public void ShowSetting()
    {
    }
    public void ShowHelp()
    {
    }
    public void ShowSave()
    {
    }
}