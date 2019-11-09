using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    public abstract class EntityDynamicData : DynamicData
    {
        [Header("实体动态数据")]
        [Tooltip("坐标")] public Vector2 Position = new Vector2(0, 0);
        [Tooltip("当前生机/耐久")] public IntProperty Health = new IntProperty(20, 20);
        [Tooltip("当前体力/能量")] public IntProperty Power = new IntProperty(20, 20);
        [Tooltip("当前容纳/携带物品")] public List<int> ItemIDs = new List<int>();
    }
}