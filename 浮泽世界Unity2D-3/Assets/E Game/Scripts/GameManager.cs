using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using E.Tool;

public class GameManager : SingletonPattern<GameManager>
{
    public GameState GameState = GameState.Lobby;
    public UIAndIOState UIAndIOState = UIAndIOState.ShowUIAndUseIO;

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
    private void Update()
    {
        UpdateGameState();
        switch (GameState)
        {
            case GameState.Lobby:
                if (Input.GetKeyUp(KeyCode.KeypadEnter))
                {
                    LoadScene("Game", true);
                }
                break;
            case GameState.Game:
                if (Input.GetKeyUp(KeyCode.LeftBracket))
                {
                    SaveManager.Singleton.SaveGame();
                }
                else if (Input.GetKeyUp(KeyCode.RightBracket))
                {
                    SaveManager.Singleton.LoadGame();
                }
                break;
            default:
                break;
        }
    }

    public void LoadScene(string name, bool useLoadUI = false)
    {
        if (SceneManager.GetSceneByName(name) == null)
        {
            Debug.LogError("请求加载的场景不存在");
            return;
        }

        if (useLoadUI)
        {
            UIManager.Singleton.LoadUI.alpha = 1;
        }
        else
        {
            UIManager.Singleton.LoadUI.alpha = 0;
        }
        SceneManager.LoadScene(name);
    }
    private void UpdateGameState()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.Contains("Lobby"))
        {
            GameState = GameState.Lobby;
        }
        else if (sceneName.Contains("Game"))
        {
            GameState = GameState.Game;
        }
    }
}

public enum GameState
{
    Lobby,
    Game
}
public enum UIAndIOState
{
    /// <summary>
    /// 显示UI且不可交互，通常用于正常游戏进程
    /// </summary>
    ShowUIAndUseIO,
    /// <summary>
    /// 显示UI且不可交互，通常用于加载场景/玩家角色硬直
    /// </summary>
    ShowUIAndUnuseIO,
    /// <summary>
    /// 隐藏UI且可交互，通常用于提升玩家沉浸感
    /// </summary>
    HideUIAndUseIO,
    /// <summary>
    /// 隐藏UI且不可交互，通常用于过场动画
    /// </summary>
    HideUIAndUnuseIO
}
