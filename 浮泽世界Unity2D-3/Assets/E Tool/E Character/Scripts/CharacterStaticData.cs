using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    [CreateAssetMenu(menuName = "E Character")]
    public class CharacterStaticData : StaticData
    {
        [SerializeField, Tooltip("生日")] private DateTime birthday = new DateTime(2000, 1, 1);
        [SerializeField, Tooltip("性别")] private Gender gender = Gender.无;
        [SerializeField, Tooltip("身高")] private int height = 160;
        [SerializeField, Tooltip("体重")] private int weight = 50;
        [SerializeField, Tooltip("种族")] private Race race = Race.人类;

        [SerializeField, Tooltip("学届")] private int startYear = 2020;
        [SerializeField, Tooltip("学院")] private string college = "设计学院";
        [SerializeField, Tooltip("专业")] private string profession = "视觉传达设计";
        [SerializeField, Tooltip("学位")] private string degree = "本科";
        [SerializeField, Tooltip("年级")] private string grade = "一年级";
        [SerializeField, Tooltip("班级")] private string @class = "甲";
        [SerializeField, Tooltip("学号")] private string studentID = "F20390052";

        [SerializeField, Tooltip("初始生机上限")] private int maxHealth = 20;
        [SerializeField, Tooltip("初始脑力上限")] private int maxMind = 20;
        [SerializeField, Tooltip("初始体力上限")] private int maxPower = 20;
        [SerializeField, Tooltip("初始生机恢复系数")] private int healthRecoveryCoefficient = 1;
        [SerializeField, Tooltip("初始脑力恢复系数")] private int mindRecoveryCoefficient = 1;
        [SerializeField, Tooltip("初始体力恢复系数")] private int powerRecoveryCoefficient = 1;
        [SerializeField, Tooltip("初始速度上限")] private int maxSpeed = 5;
        [SerializeField, Tooltip("初始基础速度")] private int baseSpeed = 2;
        [SerializeField, Tooltip("初始智力")] private int intelligence = 5;
        [SerializeField, Tooltip("初始力量")] private int strength = 5;
        [SerializeField, Tooltip("初始防御")] private int defense = 1;
        
        [SerializeField, Tooltip("初始携带的人民币")] private int rmb = 100;
        [SerializeField, Tooltip("初始携带的浮泽币")] private int fzb = 0;
        [SerializeField, Tooltip("初始携带的物品")] private List<Item> items = new List<Item>();
        [SerializeField, Tooltip("初始掌握的技能")] private List<Skill> skills = new List<Skill>();
        [SerializeField, Tooltip("初始拥有的增益")] private List<Buff> buffs = new List<Buff>();
        [SerializeField, Tooltip("初始接受的任务")] private List<Quest> acceptedQuests = new List<Quest>();
        [SerializeField, Tooltip("初始发布的任务")] private List<Quest> publishedQuests = new List<Quest>();
        [SerializeField, Tooltip("初始人际关系")] private List<Relationship> relationships = new List<Relationship>();

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get => birthday; }
        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get => gender; }
        /// <summary>
        /// 身高
        /// </summary>
        public int Height { get => height; }
        /// <summary>
        /// 体重
        /// </summary>
        public int Weight { get => weight; }
        /// <summary>
        /// 种族
        /// </summary>
        public Race Race { get => race; }

        /// <summary>
        /// 学届
        /// </summary>
        public int StartYear { get => startYear; }
        /// <summary>
        /// 学院
        /// </summary>
        public string College { get => college; }
        /// <summary>
        /// 专业
        /// </summary>
        public string Profession { get => profession; }
        /// <summary>
        /// 学位
        /// </summary>
        public string Degree { get => degree; }
        /// <summary>
        /// 年级
        /// </summary>
        public string Grade { get => grade; }
        /// <summary>
        /// 班级
        /// </summary>
        public string Class { get => @class; }
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentID { get => studentID; }

        /// <summary>
        /// 初始生机上限
        /// </summary>
        public int MaxHealth { get => maxHealth; }
        /// <summary>
        /// 初始脑力上限
        /// </summary>
        public int MaxMind { get => maxMind; }
        /// <summary>
        /// 初始体力上限
        /// </summary>
        public int MaxPower { get => maxPower; }
        /// <summary>
        /// 初始生机恢复系数
        /// </summary>
        public int HealthRecoveryCoefficient { get => healthRecoveryCoefficient; }
        /// <summary>
        /// 初始脑力恢复系数
        /// </summary>
        public int MindRecoveryCoefficient { get => mindRecoveryCoefficient; }
        /// <summary>
        /// 初始体力恢复系数
        /// </summary>
        public int PowerRecoveryCoefficient { get => powerRecoveryCoefficient; }
        /// <summary>
        /// 初始速度上限
        /// </summary>
        public int MaxSpeed { get => maxSpeed; }
        /// <summary>
        /// 初始基础速度
        /// </summary>
        public int BaseSpeed { get => baseSpeed; }
        /// <summary>
        /// 初始智力
        /// </summary>
        public int Intelligence { get => intelligence; }
        /// <summary>
        /// 初始力量
        /// </summary>
        public int Strength { get => strength; }
        /// <summary>
        /// 初始防御
        /// </summary>
        public int Defense { get => defense; }
        
        /// <summary>
        /// 初始携带的人民币
        /// </summary>
        public int RMB { get => rmb; }
        /// <summary>
        /// 初始携带的浮泽币
        /// </summary>
        public int FZB { get => fzb; }
        /// <summary>
        /// 初始携带的物品
        /// </summary>
        public List<Item> Items { get => items; }
        /// <summary>
        /// 初始掌握的技能
        /// </summary>
        public List<Skill> Skills { get => skills; }
        /// <summary>
        /// 初始拥有的增益
        /// </summary>
        public List<Buff> Buffs { get => buffs; }
        /// <summary>
        /// 初始接受的任务
        /// </summary>
        public List<Quest> AcceptedQuests { get => acceptedQuests; }
        /// <summary>
        /// 初始发布的任务
        /// </summary>
        public List<Quest> PublishedQuests { get => publishedQuests; }
        /// <summary>
        /// 初始人际关系
        /// </summary>
        public List<Relationship> Relationships { get => relationships; }
    }

    [Serializable]
    public struct Relationship
    {
        [SerializeField, Tooltip("对象")] private CharacterStaticData character;
        [SerializeField, Tooltip("好感度")] private int favorability;

        /// <summary>
        /// 对象
        /// </summary>
        public CharacterStaticData Character { get => character;  }
        /// <summary>
        /// 好感度
        /// </summary>
        public int Favorability
        {
            get => favorability;
            set
            {
                if (value >= -100 && value <= 100)
                {
                    favorability = value;
                }
            }
        }

        public Relationship(CharacterStaticData character, int favorability = 0)
        {
            if (character)
            {
                this.character = character;
            }
            else
            {
                this.character = null;
                Debug.LogError("未指定人际关系对象");
            }
            if (favorability >= -100 && favorability <= 100)
            {
                this.favorability = favorability;
            }
            else
            {
                this.favorability = 0;
                Debug.LogError("超出了好感度设置范围 -100~100");
            }
        }
    }
    public enum Gender
    {
        无,
        男,
        女
    }
    public enum Race
    {
        人类,
        半人,
        野兽,
        精灵,
        植物
    }
}