using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

[CreateAssetMenu(menuName = "E Character")]
public class CharacterStaticData : StaticData
{
    [SerializeField, Tooltip("生日")] private DateTime birthday = new DateTime(2000,1,1);
    [SerializeField, Tooltip("性别")] private Gender gender = Gender.无;
    [SerializeField, Tooltip("身高")] private int height = 160;
    [SerializeField, Tooltip("体重")] private int weight = 50;

    [SerializeField, Tooltip("学届")] private int startYear = 2020;
    [SerializeField, Tooltip("学院")] private string college = "设计学院";
    [SerializeField, Tooltip("专业")] private string profession = "视觉传达设计";
    [SerializeField, Tooltip("学位")] private string degree = "本科";
    [SerializeField, Tooltip("年级")] private string grade = "一年级";
    [SerializeField, Tooltip("班级")] private string @class = "甲";
    [SerializeField, Tooltip("学号")] private string studentID = "F20390052";

    [SerializeField, Tooltip("生机")] private int health = 20;
    [SerializeField, Tooltip("脑力")] private int mind = 20;
    [SerializeField, Tooltip("体力")] private int power = 20;
    [SerializeField, Tooltip("生机恢复系数")] private int healthRecoveryCoefficient = 1;
    [SerializeField, Tooltip("脑力恢复系数")] private int mindRecoveryCoefficient = 1;
    [SerializeField, Tooltip("体力恢复系数")] private int powerRecoveryCoefficient = 1;
    [SerializeField, Tooltip("智力")] private int intelligence = 5;
    [SerializeField, Tooltip("速度")] private int speed = 5;
    [SerializeField, Tooltip("力量")] private int strength = 5;
    [SerializeField, Tooltip("防御")] private int defense = 1;

    [SerializeField, Tooltip("携带的人民币")] private int rmb = 100;
    [SerializeField, Tooltip("携带的浮泽币")] private int fzb = 0;
    [SerializeField, Tooltip("携带的物品")] private List<Item> inventory = new List<Item>();
    [SerializeField, Tooltip("掌握的技能")] private List<Skill> skills = new List<Skill>();
    [SerializeField, Tooltip("当前的增益")] private List<Buff> buffs = new List<Buff>();
    [SerializeField, Tooltip("接受的任务")] private List<Quest> acceptedQuest = new List<Quest>();
    [SerializeField, Tooltip("发布的任务")] private List<Quest> publishedQuest = new List<Quest>();

    public DateTime Birthday { get => birthday; }
    public Gender Gender { get => gender; }
    public int Height { get => height; }
    public int Weight { get => weight; }

    public int StartYear { get => startYear; }
    public string College { get => college; }
    public string Profession { get => profession; }
    public string Degree { get => degree; }
    public string Grade { get => grade; }
    public string Class { get => @class; }
    public string StudentID { get => studentID; }

    public int Health { get => health; }
    public int Mind { get => mind; }
    public int Power { get => power; }
    public int HealthRecoveryCoefficient { get => healthRecoveryCoefficient; }
    public int MindRecoveryCoefficient { get => mindRecoveryCoefficient; }
    public int PowerRecoveryCoefficient { get => powerRecoveryCoefficient; }
    public int Intelligence { get => intelligence; }
    public int Speed { get => speed; }
    public int Strength { get => strength; }
    public int Defense { get => defense;}

    public int RMB { get => rmb; }
    public int FZB { get => fzb; }
    public List<Item> Inventory { get => inventory; }
    public List<Skill> Skills { get => skills; }
    public List<Buff> Buffs { get => buffs; }
    public List<Quest> AcceptedQuest { get => acceptedQuest; }
    public List<Quest> PublishedQuest { get => publishedQuest; }

}

public enum Gender
{
    无,
    男,
    女
}