using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

namespace E.Tool
{
    [Serializable]
    public class CharacterDynamicData : DynamicData
    {
        [SerializeField, Tooltip("当前生机上限")] private int maxHealth = 20;
        [SerializeField, Tooltip("当前脑力上限")] private int maxMind = 20;
        [SerializeField, Tooltip("当前体力上限")] private int maxPower = 20;
        [SerializeField, Tooltip("当前生机")] private int health = 20;
        [SerializeField, Tooltip("当前脑力")] private int mind = 20;
        [SerializeField, Tooltip("当前体力")] private int power = 20;
        [SerializeField, Tooltip("当前生机恢复系数")] private int healthRecoveryCoefficient = 1;
        [SerializeField, Tooltip("当前脑力恢复系数")] private int mindRecoveryCoefficient = 1;
        [SerializeField, Tooltip("当前体力恢复系数")] private int powerRecoveryCoefficient = 1;
        [SerializeField, Tooltip("当前速度上限")] private int maxSpeed = 5;
        [SerializeField, Tooltip("当前基础速度")] private int baseSpeed = 2;
        [SerializeField, Tooltip("当前智力")] private int intelligence = 5;
        [SerializeField, Tooltip("当前力量")] private int strength = 5;
        [SerializeField, Tooltip("当前防御")] private int defense = 1;

        [SerializeField, Tooltip("当前携带的人民币")] private int rmb = 100;
        [SerializeField, Tooltip("当前携带的浮泽币")] private int fzb = 0;
        [SerializeField, Tooltip("当前携带的物品")] private List<Item> inventory = new List<Item>();
        [SerializeField, Tooltip("当前掌握的技能")] private List<Skill> skills = new List<Skill>();
        [SerializeField, Tooltip("当前拥有的增益")] private List<Buff> buffs = new List<Buff>();
        [SerializeField, Tooltip("当前接受的任务")] private List<Quest> acceptedQuests = new List<Quest>();
        [SerializeField, Tooltip("当前发布的任务")] private List<Quest> publishedQuests = new List<Quest>();
        [SerializeField, Tooltip("当前人际关系")] private List<Relationship> relationships = new List<Relationship>();

        /// <summary>
        /// 当前生机上限
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
        /// 当前脑力上限
        /// </summary>
        public int MaxMind
        {
            get => maxMind;
            set
            {
                if (value > 0)
                {
                    maxMind = value;
                }
            }
        }
        /// <summary>
        /// 当前体力上限
        /// </summary>
        public int MaxPower
        {
            get => maxPower;
            set
            {
                if (value > 0)
                {
                    maxPower = value;
                }
            }
        }
        /// <summary>
        /// 当前生机
        /// </summary>
        public int Health
        {
            get => health;
            set
            {
                if (!invincible)
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
        }
        /// <summary>
        /// 当前脑力
        /// </summary>
        public int Mind
        {
            get => mind;
            set
            {
                if (value < 0)
                {
                    mind = 0;
                }
                else if (value > MaxMind)
                {
                    mind = MaxMind;
                }
                else
                {
                    mind = value;
                }
            }
        }
        /// <summary>
        /// 当前体力
        /// </summary>
        public int Power
        {
            get => power;
            set
            {
                if (value < 0)
                {
                    power = 0;
                }
                else if (value > MaxPower)
                {
                    power = MaxPower;
                }
                else
                {
                    power = value;
                }
            }
        }
        /// <summary>
        /// 当前生机恢复系数
        /// </summary>
        public int HealthRecoveryCoefficient
        {
            get => healthRecoveryCoefficient;
            set => healthRecoveryCoefficient = value;
        }
        /// <summary>
        /// 当前脑力恢复系数
        /// </summary>
        public int MindRecoveryCoefficient
        {
            get => mindRecoveryCoefficient;
            set => mindRecoveryCoefficient = value;
        }
        /// <summary>
        /// 当前体力恢复系数
        /// </summary>
        public int PowerRecoveryCoefficient
        {
            get => powerRecoveryCoefficient;
            set => powerRecoveryCoefficient = value;
        }
        /// <summary>
        /// 当前速度上限
        /// </summary>
        public int MaxSpeed
        {
            get => maxSpeed;
            set => maxSpeed = value > 0 ? value : maxSpeed;
        }
        /// <summary>
        /// 当前基础速度
        /// </summary>
        public int BaseSpeed
        {
            get => baseSpeed;
            set => baseSpeed = value > 0 ? value : baseSpeed;
        }
        /// <summary>
        /// 当前智力
        /// </summary>
        public int Intelligence
        {
            get => intelligence;
            set => intelligence = value > 0 ? value : intelligence;
        }
        /// <summary>
        /// 当前力量
        /// </summary>
        public int Strength
        {
            get => strength;
            set => strength = value > 0 ? value : strength;
        }
        /// <summary>
        /// 当前防御
        /// </summary>
        public int Defense
        {
            get => defense;
            set => defense = value > 0 ? value : defense;
        }

        /// <summary>
        /// 当前携带的人民币
        /// </summary>
        public int RMB
        {
            get => rmb;
            set => rmb = value;
        }
        /// <summary>
        /// 当前携带的浮泽币
        /// </summary>
        public int FZB
        {
            get => fzb;
            set => fzb = value;
        }
        /// <summary>
        /// 当前携带的物品
        /// </summary>
        public List<Item> Inventory
        {
            get => inventory;
            set => inventory = value;
        }
        /// <summary>
        /// 当前掌握的技能
        /// </summary>
        public List<Skill> Skills
        {
            get => skills;
            set => skills = value;
        }
        /// <summary>
        /// 当前拥有的增益
        /// </summary>
        public List<Buff> Buffs
        {
            get => buffs;
            set => buffs = value;
        }
        /// <summary>
        /// 当前接受的任务
        /// </summary>
        public List<Quest> AcceptedQuests
        {
            get => acceptedQuests;
            set => acceptedQuests = value;
        }
        /// <summary>
        /// 当前发布的任务
        /// </summary>
        public List<Quest> PublishedQuests
        {
            get => publishedQuests;
            set => publishedQuests = value;
        }
        /// <summary>
        /// 当前人际关系
        /// </summary>
        public List<Relationship> Relationships
        {
            get => relationships;
            set => relationships = value;
        }
    }
}