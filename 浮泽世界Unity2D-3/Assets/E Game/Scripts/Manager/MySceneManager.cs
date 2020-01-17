using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    private AsyncOperation asyncOperation;

    public bool IsInLobby
    {
        get => SceneManager.GetActiveScene().name == "Lobby" ? true : false;
    }
    public float SceneLoadProgress
    {
        get
        {
            if (asyncOperation != null)
            {
                //AsyncOperation的progress（0-1）属性在isDone为false时，最大加载到0.9就会暂停，
                //直到isDone为true时才会继续加载0.9-1.0的这10%，
                //而isDone为true或fasle的标志为是：
                //当allowSceneActivation = false，isDone为false，
                //allowSceneActivation = false 的作用是让场景不会在加载完成后自动跳转到下一个场景，  
                //当allowSceneActivation = true，isDone才可以为true，直到progress = 1.0时 isDone = true
                return asyncOperation.progress;// >= 0.9f ? 1.0f :sceneAsyncOperation.progress;
            }
            else
            {
                return -1;
            }
        }
    }

    private void Awake()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="name"></param>
    /// <param name="useLoadUI"></param>
    public void LoadScene(string name, bool useLoadUI = false, bool isStartGame = false)
    {
        if (SceneManager.GetSceneByName(name) == null)
        {
            Debug.LogError("请求加载的场景不存在");
            return;
        }

        asyncOperation = SceneManager.LoadSceneAsync(name);
        if (isStartGame)
        {
            asyncOperation.completed += OnSceneLoadCompleted_StartGame;
        }
        else
        {
            asyncOperation.completed += OnSceneLoadCompleted;
        }

        if (useLoadUI)
        {
            GameManager.UI.UILoading.Show();
        }
    }
    private void OnSceneLoadCompleted_StartGame(AsyncOperation obj)
    {
        GameManager.UI.UILoading.Hide();
        GameManager.Save.LoadFrom(GameManager.Save.currentSave);
    }
    private void OnSceneLoadCompleted(AsyncOperation obj)
    {
        GameManager.UI.UILoading.Hide();
    }
    private void OnActiveSceneChanged(Scene arg0, Scene arg1)
    {
        GameManager.UI.RefreshMode();
    }
}
