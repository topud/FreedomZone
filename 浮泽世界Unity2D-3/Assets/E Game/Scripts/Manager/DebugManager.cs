using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

public class DebugManager : SingletonClass<AudioManager>
{
    public bool IsShowDebugInfo = false;
    private void Start()
    {

    }
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.F1))
        {
            IsShowDebugInfo = !IsShowDebugInfo;
        }
        if (Input.GetKeyUp(KeyCode.F2))
        {

        }

#else
        if (Input.GetKeyUp(KeyCode.F1))
        {

        }
#endif
    }
    private void OnGUI()
    {
        if (IsShowDebugInfo)
        {
            GUILayout.TextField(TimeManager.Singleton.Now.ToString());
        }
    }
}