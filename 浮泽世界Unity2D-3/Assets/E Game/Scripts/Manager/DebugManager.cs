using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using E.Tool;

public class DebugManager : MonoBehaviour
{
#if UNITY_EDITOR
    public bool isShowLogInfo = false;
    public bool IsShowVersionInfo = true;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            isShowLogInfo = !isShowLogInfo;
        }
        else if (Input.GetKeyUp(KeyCode.F2))
        {
            IsShowVersionInfo = !IsShowVersionInfo;
        }
    }
    private void OnGUI()
    {
        if (isShowLogInfo)
        {
            GUILayout.TextField(GameManager.Time.Now.ToString());
        }
        if (IsShowVersionInfo)
        {
            GUILayout.TextField(PlayerSettings.bundleVersion);
        }
    }
#endif
}