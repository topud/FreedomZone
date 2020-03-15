using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace E.Tool
{
    public abstract class EntityStaticData : StaticData
    {
        [Header("实体静态数据")]
        [Tooltip("预制体")] public GameObject prefab = null;
        [Tooltip("体积")] public int volume = 1;
        [Tooltip("质量")] public int weight = 50;
        [Tooltip("高度")] public int height = 10;
        [Tooltip("生日")] public Vector3Int birthday = new Vector3Int(2000, 1, 1);

        public DateTime Birthday { get => new DateTime(birthday.x, birthday.y, birthday.z); }
    }
}
