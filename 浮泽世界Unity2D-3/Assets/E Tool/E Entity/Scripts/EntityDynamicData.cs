using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    public abstract class EntityDynamicData
    {
        [Header("实体动态数据")]
        [Tooltip("名称与编号")] public NameAndID nameID = new NameAndID("NoName", 0);
        [Tooltip("坐标")] public Vector2 position = new Vector2(0, 0);
        [Tooltip("当前生机/耐久")] public IntProperty health = new IntProperty(20, 20);
        [Tooltip("当前体力/能量")] public IntProperty power = new IntProperty(20, 20);
        [Tooltip("当前容纳/携带物品")] public List<NameAndID> items = new List<NameAndID>();
    }

    [Serializable]
    public struct NameAndID
    {
        [ReadOnly] public string name;
        public int id;

        public NameAndID(string name, int id)
        {
            this.name = name;
            this.id = id;
        }
        public string NameID
        {
            get => name + id.ToString();
        }
    }
}