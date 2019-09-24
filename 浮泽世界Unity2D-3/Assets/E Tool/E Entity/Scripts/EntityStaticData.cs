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
        [SerializeField, Tooltip("生日 年"), Range(1970, 2020)] private int birthdayYear = 2000;
        [SerializeField, Tooltip("生日 月"), Range(1, 12)] private int birthdayMonth = 1;
        [SerializeField, Tooltip("生日 日"), Range(1, 31)] private int birthdayDay = 1;
        [SerializeField, Tooltip("初始生机/耐久")] private IntProperty health = new IntProperty(20, 20);

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
        public DateTime Birthday { get => new DateTime(birthdayYear, birthdayMonth, birthdayDay); }
        /// <summary>
        /// 初始生机
        /// </summary>
        public IntProperty Health { get => health; }
    }
}
