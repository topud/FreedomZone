using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    [Serializable]
    public class ItemDynamicData : EntityDynamicData
    {
        [Header("物品实体动态数据")]
        [Tooltip("当前堆叠数量")] public int Stack = 1;
        [Tooltip("当前容纳物品")] public List<Item> Items = new List<Item>();
    }
}