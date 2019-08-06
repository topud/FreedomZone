// ========================================================
// 作者：E Star
// 创建时间：2019-01-27 01:46:58
// 当前版本：1.0
// 作用描述：
// 挂载目标：
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Utility;

public class SettingManager : SingletonPattern<SettingManager>
{
    [SerializeField] private bool m_IsShowVersionInfo;


    private void Start()
    {
        //PlayerPrefs.SetInt("music", 1);
    }
    private void Update()
    {
    }
}
