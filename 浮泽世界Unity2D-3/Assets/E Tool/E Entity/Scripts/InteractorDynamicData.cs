using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    [Serializable]
    public class InteractorDynamicData : EntityDynamicData
    {
        [SerializeField, Tooltip("当前堆叠数量")] private int stack = 1;
        [SerializeField, Tooltip("当前耐久上限")] private int maxHealth = 10;
        [SerializeField, Tooltip("当前耐久")] private int health = 10;
        [SerializeField, Tooltip("当前容纳物品")] private List<Item> items = new List<Item>();

        /// <summary>
        /// 当前堆叠数量
        /// </summary>
        public int Stack
        {
            get => stack;
            set => stack = Utility.ClampMin(value, 1);
        }
        /// <summary>
        /// 当前耐久上限
        /// </summary>
        public int MaxHealth
        {
            get => maxHealth;
            set => maxHealth = Utility.ClampMin(value, 0);
        }
        /// <summary>
        /// 当前耐久
        /// </summary>
        public int Health
        {
            get => health;
            set
            {
                if (!Invincible)
                { health = Utility.Clamp(value, 0, MaxHealth); }
            }
        }

        /// <summary>
        /// 当前容纳物品
        /// </summary>
        public List<Item> Items
        {
            get => items;
            set => items = value;
        }
    }
}