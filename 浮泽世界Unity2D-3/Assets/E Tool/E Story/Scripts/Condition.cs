using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    [Serializable]
    public struct Condition
    {
        [Tooltip("条件名")] public string Key;
        [Tooltip("默认值"), Range(-100, 100)] public int DefaultValue;
    }
}