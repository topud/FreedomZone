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
        [SerializeField, Tooltip("名称")] protected new string name = "";
        [SerializeField, Tooltip("描述"), TextArea(1, 30)] protected string description = "";
        [SerializeField, Tooltip("图标")] protected Sprite icon = null;
        [SerializeField, Tooltip("预制体")] protected GameObject prefab = null;
        [Space(4)]
        [SerializeField, Tooltip("质量")] private int weight = 50;
        [SerializeField, Tooltip("体积")] private int volume = 1;
        [SerializeField, Tooltip("生日")] private Vector3Int birthday = new Vector3Int(2000,1,1);
        [SerializeField, Tooltip("初始生机/耐久")] private IntProperty health = new IntProperty(20, 20);
        [SerializeField, Tooltip("初始体力/能量")] private IntProperty power = new IntProperty(20, 20);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get => name; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get => description; }
        /// <summary>
        /// 图标
        /// </summary>
        public Sprite Icon { get => icon; }
        /// <summary>
        /// 预制体
        /// </summary>
        public GameObject Prefab { get => prefab; }
        /// <summary>
        /// 质量
        /// </summary>
        public int Weight { get => weight; }
        /// <summary>
        /// 体积
        /// </summary>
        public int Volume { get => volume; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get => new DateTime(birthday.x, birthday.y, birthday.z); }
        /// <summary>
        /// 初始生机/耐久
        /// </summary>
        public IntProperty Health { get => health; }
        /// <summary>
        /// 初始体力/能量
        /// </summary>
        public IntProperty Power { get => power; }
    }
}
