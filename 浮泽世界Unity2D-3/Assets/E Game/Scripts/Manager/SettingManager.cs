using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

public class SettingManager : SingletonClass<SettingManager>
{
    public bool IsShowVersionInfo = true;
    public bool IsFullScreen = true;

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("IsShowVersionInfo")) IsShowVersionInfo = PlayerPrefs.GetInt("IsShowVersionInfo") == 1 ? true : false;
        else IsShowVersionInfo = true;
        if (PlayerPrefs.HasKey("IsFullScreen")) IsFullScreen = PlayerPrefs.GetInt("IsFullScreen") == 1 ? true : false;
        else IsFullScreen = true;
    }
    private void OnDisable()
    {
        SaveSetting();
    }

    public void SaveSetting()
    {
        PlayerPrefs.SetInt("IsShowVersionInfo", IsShowVersionInfo ? 1 : 0);
        PlayerPrefs.SetInt("IsFullScreen", IsFullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }
}
