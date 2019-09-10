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
        [SerializeField, Tooltip("质量")] private int weight = 50;
        [SerializeField, Tooltip("体积")] private int volume = 1;
        [SerializeField, Tooltip("生日")] private DateTime birthday = new DateTime(2000, 1, 1);
        [SerializeField, Tooltip("初始生机")] private IntProperty health = new IntProperty(20, 20);

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
        public DateTime Birthday { get => birthday; }
        /// <summary>
        /// 初始生机
        /// </summary>
        public IntProperty Health { get => health; }
    }
}
