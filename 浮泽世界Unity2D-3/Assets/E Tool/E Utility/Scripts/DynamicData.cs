using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    public abstract class DynamicData
    {
        [Header("动态数据")]
        [Tooltip("名称"), ReadOnly] public string Name = "";
        [Tooltip("名称"), ReadOnly] public int ID = 0;
    }
}