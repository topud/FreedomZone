using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace E.Tool
{
    public abstract class StaticData : ScriptableObject
    {
        [Header("静态数据")]
        [Tooltip("名称")] public new string name = "无名称";
        [Tooltip("描述")] public string description = "无描述";
        [Tooltip("图标")] public Sprite icon = null;
    }
}
