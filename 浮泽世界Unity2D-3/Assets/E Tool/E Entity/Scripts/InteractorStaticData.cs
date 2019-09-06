using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    [CreateAssetMenu(menuName = "E Interactor")]
    public class InteractorStaticData : EntityStaticData
    {
        [SerializeField, Tooltip("是否可移动")] private bool movable = true;
        [SerializeField, Tooltip("是否可堆叠")] private bool stackable = false;
        [SerializeField, Tooltip("是否可容纳")] private bool accommodatable = false;
        [SerializeField, Tooltip("是否可腐烂")] private bool perishable = false;

        [SerializeField, Tooltip("质量")] private int weight = 10;
        [SerializeField, Tooltip("体积")] private int volume = 1;
        [SerializeField, Tooltip("价格")] private int rmbPrice = 10;
        [SerializeField, Tooltip("容量")] private int capacity = 0;
        [SerializeField, Tooltip("使用后习得的技能")] private List<Skill> skills = new List<Skill>();
        [SerializeField, Tooltip("使用后获得的增益")] private List<Buff> buffs = new List<Buff>();

        [SerializeField, Tooltip("初始堆叠数量")] private int stack = 1;
        [SerializeField, Tooltip("初始耐久上限")] private int maxHealth = 10;
        [SerializeField, Tooltip("初始容纳物品")] private List<Item> items = new List<Item>();

        /// <summary>
        /// 是否可移动
        /// </summary>
        public bool Movable { get => movable; }
        /// <summary>
        /// 是否可堆叠
        /// </summary>
        public bool Stackable { get => stackable; }
        /// <summary>
        /// 是否可容纳
        /// </summary>
        public bool Accommodatable { get => accommodatable; }
        /// <summary>
        /// 是否可腐烂
        /// </summary>
        public bool Perishable { get => perishable; }

        /// <summary>
        /// 质量
        /// </summary>
        public int Weight { get => weight; }
        /// <summary>
        /// 体积
        /// </summary>
        public int Volume { get => volume; }
        /// <summary>
        /// 价格
        /// </summary>
        public int RMBPrice { get => rmbPrice; }
        /// <summary>
        /// 容量
        /// </summary>
        public int Capacity { get => capacity; }
        /// <summary>
        /// 使用后习得的技能
        /// </summary>
        public List<Skill> Skills { get => skills; }
        /// <summary>
        /// 使用后获得的增益
        /// </summary>
        public List<Buff> Buffs { get => buffs; }

        /// <summary>
        /// 初始堆叠数量
        /// </summary>
        public int Stack { get => stack; }
        /// <summary>
        /// 初始耐久上限
        /// </summary>
        public int MaxHealth { get => maxHealth; }
        /// <summary>
        /// 初始容纳物品
        /// </summary>
        public List<Item> Items { get => items; }
    }
}