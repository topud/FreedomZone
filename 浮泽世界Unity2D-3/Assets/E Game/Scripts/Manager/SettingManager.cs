using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

public class SettingManager : MonoBehaviour
{
    public bool IsFullScreen = true;

    private void OnEnable()
    {
        Load();
    }
    private void OnDisable()
    {
        Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("IsFullScreen")) IsFullScreen = PlayerPrefs.GetInt("IsFullScreen") == 1 ? true : false;
        else IsFullScreen = true;
    }
    public void Save()
    {
        PlayerPrefs.SetInt("IsFullScreen", IsFullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }
}
