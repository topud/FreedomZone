using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; protected set; }

    public static AudioManager Audio{ get => FindObjectOfType<AudioManager>(); }
    public static CameraManager Camera { get => FindObjectOfType<CameraManager>(); }
    public static CharacterManager Character { get => FindObjectOfType<CharacterManager>(); }
    public static DebugManager DebugManager { get => FindObjectOfType<DebugManager>(); }
    public static EnvironmentManager Environment { get => FindObjectOfType<EnvironmentManager>(); }
    public static EventManager Event { get => FindObjectOfType<EventManager>(); }
    public static ItemManager Item { get => FindObjectOfType<ItemManager>(); }
    public static LocalizationManager Localization { get => FindObjectOfType<LocalizationManager>(); }
    public static MySceneManager Scene { get => FindObjectOfType<MySceneManager>(); }
    public static PlayableManager Playable { get => FindObjectOfType<PlayableManager>(); }
    public static SaveManager Save { get => FindObjectOfType<SaveManager>(); }
    public static SettingManager Setting { get => FindObjectOfType<SettingManager>(); }
    public static StoryManager Story { get => FindObjectOfType<StoryManager>(); }
    public static TimeManager Time { get => FindObjectOfType<TimeManager>(); }
    public static UIManager UI { get => FindObjectOfType<UIManager>(); }
    
    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        UI.RefreshMode();

        if (Scene.IsInLobby)
        {
            //正常游戏流程
        }
        else
        {
            //场景测试流程
            if (!Character.Player)
            {
                Character player = Character.GetCharacter("璃亚");
                player.IsPlayer = true;
                Debug.Log("本次运行为调试模式，已自动添加可控制角色");
            }

            UI.UIHelp.Show();
        }
    }

    /// <summary>
    /// 开始新存档
    /// </summary>
    public static void StartNewSave()
    {
        Scene.LoadScene("Game", true, true);
        Save.currentSave = Save.CreateSaveFile();
    }
    /// <summary>
    /// 继续上一次的存档
    /// </summary>
    public static void ContinueLastSave()
    {
        Scene.LoadScene("Game", true, true);
        Save.currentSave = Save.GetLatestSaveFile();
    }
    /// <summary>
    /// 继续选中的存档
    /// </summary>
    /// <param name="fileInfo"></param>
    public static void ContinueSelectSave(FileInfo fileInfo)
    {
        if (fileInfo.Exists)
        {
            Debug.LogError("存档文件不存在");
            return;
        }
        Scene.LoadScene("Game", true, true);
        Save.currentSave = fileInfo;
    }
    /// <summary>
    /// 返回大厅
    /// </summary>
    public static void BackToLobby()
    {
        Scene.LoadScene("Lobby", false);
        Save.currentSave = null;
    }
    /// <summary>
    /// 退出游戏
    /// </summary>
    public static void QuitGame()
    {
        Application.Quit();
    }
}