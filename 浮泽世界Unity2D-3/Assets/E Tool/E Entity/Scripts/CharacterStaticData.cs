using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    [CreateAssetMenu(menuName = "E Character")]
    public class CharacterStaticData : EntityStaticData
    {
        [Header("角色实体静态数据")]
        [SerializeField, Tooltip("种族")] private Race race = Race.人类;
        [SerializeField, Tooltip("性别")] private Gender gender = Gender.无;
        [SerializeField, Tooltip("身高")] private int height = 160;
        [SerializeField, Tooltip("学届")] private int startYear = 2020;
        [SerializeField, Tooltip("学院")] private string college = "设计学院";
        [SerializeField, Tooltip("专业")] private string profession = "视觉传达设计";
        [SerializeField, Tooltip("学位")] private string degree = "本科";
        [SerializeField, Tooltip("年级")] private string grade = "一年级";
        [SerializeField, Tooltip("班级")] private string @class = "甲";
        [SerializeField, Tooltip("学号")] private string studentID = "F20390052";

        [SerializeField, Tooltip("初始脑力")] private IntProperty mind = new IntProperty(20, 20);
        [SerializeField, Tooltip("初始体力")] private IntProperty power = new IntProperty(20, 20);
        [SerializeField, Tooltip("初始速度")] private IntProperty speed = new IntProperty(5, 2, 0, false, false, 0);
        [SerializeField, Tooltip("初始智力")] private IntProperty iq = new IntProperty(100, 5, 0, false, false, 0);
        [SerializeField, Tooltip("初始力量")] private IntProperty strength = new IntProperty(100, 5, 0, false, false, 0);
        [SerializeField, Tooltip("初始防御")] private IntProperty defense = new IntProperty(100, 1, 0, false, false, 0);

        [SerializeField, Tooltip("初始携带的人民币")] private int rmb = 100;
        [SerializeField, Tooltip("初始携带的浮泽币")] private int fzb = 0;
        [SerializeField, Tooltip("初始携带的物品")] private List<ItemStaticData> items = new List<ItemStaticData>();
        [SerializeField, Tooltip("初始穿戴的服装")] private List<ItemStaticData> clothings = new List<ItemStaticData>();
        [SerializeField, Tooltip("初始掌握的技能")] private List<Skill> skills = new List<Skill>();
        [SerializeField, Tooltip("初始拥有的增益")] private List<Buff> buffs = new List<Buff>();
        [SerializeField, Tooltip("初始接受的任务")] private List<Quest> acceptedQuests = new List<Quest>();
        [SerializeField, Tooltip("初始发布的任务")] private List<Quest> publishedQuests = new List<Quest>();
        [SerializeField, Tooltip("初始人际关系")] private List<Relationship> relationships = new List<Relationship>();

        [SerializeField, Tooltip("随机对话内容")] private List<string> randomStorys = new List<string>();
        
        /// <summary>
        /// 种族
        /// </summary>
        public Race Race { get => race; }
        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get => gender; }
        /// <summary>
        /// 身高
        /// </summary>
        public int Height { get => height; }
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
        /// 初始脑力
        /// </summary>
        public IntProperty Mind { get => mind; }
        /// <summary>
        /// 初始体力
        /// </summary>
        public IntProperty Power { get => power; }
        /// <summary>
        /// 初始速度
        /// </summary>
        public IntProperty Speed { get => speed; }
        /// <summary>
        /// 初始智力
        /// </summary>
        public IntProperty IQ { get => iq; }
        /// <summary>
        /// 初始力量
        /// </summary>
        public IntProperty Strength { get => strength; }
        /// <summary>
        /// 初始防御
        /// </summary>
        public IntProperty Defense { get => defense; }
        
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
        public List<ItemStaticData> Items { get => items; }
        /// <summary>
        /// 初始穿戴的服装
        /// </summary>
        public List<ItemStaticData> Clothings { get => clothings; set => clothings = value; }
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

        /// <summary>
        /// 随机对话内容
        /// </summary>
        public List<string> RandomStorys { get => randomStorys; }
    }

    [Serializable]
    public struct Relationship
    {
        [SerializeField, Tooltip("对象")] private CharacterStaticData character;
        [SerializeField, Tooltip("好感度")] private int favorability;

        /// <summary>
        /// 对象
        /// </summary>
        public CharacterStaticData Character { get => character; }
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