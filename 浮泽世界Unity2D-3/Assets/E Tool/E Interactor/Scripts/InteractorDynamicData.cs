using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    [Serializable]
    public class InteractorDynamicData : DynamicData
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
            set => stack = value;
        }
        /// <summary>
        /// 当前耐久上限
        /// </summary>
        public int MaxHealth
        {
            get => maxHealth;
            set
            {
                if (value > 0)
                {
                    maxHealth = value;
                }
            }
        }
        /// <summary>
        /// 当前耐久
        /// </summary>
        public int Health
        {
            get => health;
            set
            {
                if (value < 0)
                {
                    health = 0;
                }
                else if (value > MaxHealth)
                {
                    health = MaxHealth;
                }
                else
                {
                    health = value;
                }
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