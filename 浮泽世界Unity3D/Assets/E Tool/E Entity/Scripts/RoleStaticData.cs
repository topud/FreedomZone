using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    [CreateAssetMenu(menuName = "E Role")]
    public class RoleStaticData : EntityStaticData
    {
        [Header("角色实体静态数据")]
        //固定数值
        [Tooltip("种族")] public Race race = Race.人类;
        [Tooltip("性别")] public Gender gender = Gender.无性;

        [Tooltip("社会信息")] public SocialInfo social = new SocialInfo();
        [Tooltip("学历信息")] public EducationalInfo educational = new EducationalInfo();
        [Tooltip("性格信息")] public PersonalityInfo personality = new PersonalityInfo();

        //可变动数值
        [Tooltip("生理素质")] public PhysiqueInfo physique = new PhysiqueInfo();
        [Tooltip("心理素质")] public MentalityInfo mentality = new MentalityInfo();
        [Tooltip("感官素质")] public SenseInfo sense = new SenseInfo();
        [Tooltip("身体状态")] public BodyState bodyState = new BodyState();

        [Tooltip("初始拥有人民币")] public int rmb = 100;
        [Tooltip("初始拥有浮泽币")] public int fzb = 0;
        [Tooltip("初始携带的物品")] public List<ItemStack> items = new List<ItemStack>();
        [Tooltip("初始人际关系")] public List<Relationship> relationships = new List<Relationship>();
        [Tooltip("初始掌握的技能")] public List<Skill> skills = new List<Skill>();
        //[Tooltip("初始穿戴的服装")] public List<ItemStack> clothings = new List<ItemStack>();
    }

    [Serializable]
    public struct EducationalInfo
    {
        [Tooltip("学位")] public Degree degree;
        [Tooltip("学届")] public int startYear;
        [Tooltip("学校")] public string university;
        [Tooltip("学院")] public string college;
        [Tooltip("专业")] public string profession;
        [Tooltip("年级")] public int grade;
        [Tooltip("班级")] public string @class;
        [Tooltip("学号")] public string studentID;

        public EducationalInfo(Degree d, int y, string u, string c, string p, int g, string cl, string s)
        {
            degree = d;
            startYear = y;
            university = u;
            college = c;
            profession = p;
            grade = g;
            @class = cl;
            studentID = s;
        }
    }

    [Serializable]
    public struct SocialInfo
    {
        [Tooltip("身份证号")] public string id;
        [Tooltip("浮泽编号")] public string fz;
    }

    [Serializable]
    public struct PersonalityInfo
    {
        [Tooltip("善良邪恶值")][Range(-150, 150)] public int evilOrGood;
        [Tooltip("混乱守序值")][Range(-150, 150)] public int chaosOrLaw;
    }

    [Serializable]
    public struct PhysiqueInfo
    {
        [Tooltip("力量")] public int force;
        [Tooltip("速度")] public int speed;
        [Tooltip("防御")] public int defense;

        public int Physique { get => force + speed + defense; }
    }

    [Serializable]
    public struct MentalityInfo
    {
        [Tooltip("记忆能力")] public int memory;
        [Tooltip("逻辑能力")] public int logical;
        [Tooltip("想象能力")] public int imagination;
        [Tooltip("表达能力")] public int expression;
        [Tooltip("应激能力")] public int reaction;
        [Tooltip("胆魄能力")] public int courage;

        public int Mentality { get => memory + logical + imagination + expression + reaction + courage; }
    }

    [Serializable]
    public struct SenseInfo
    {
        [Tooltip("视觉")] public int see;
        [Tooltip("听觉")] public int hear;
        [Tooltip("嗅觉")] public int smell;
        [Tooltip("味觉")] public int taste;
        [Tooltip("触觉")] public int touch;

        public int Sense { get => see + hear + smell + taste + taste + touch; }
    }

    [Serializable]
    public struct BodyState
    {
        [Tooltip("生机")] public FloatProperty health;
        [Tooltip("体力")] public FloatProperty power;
        [Tooltip("脑力")] public FloatProperty mind;
    }

    [Serializable]
    public class ItemStack
    {
        public ItemStaticData item;
        public int stack = 1;
    }

    [Serializable]
    public class Relationship
    {
        [Tooltip("角色")] public RoleStaticData role;
        [Tooltip("对其好感度")] [Range(-100, 100)] public int favorability;
        [Tooltip("对其熟悉度")] [Range(0, 100)] public int familiarity;
    }

    public enum Degree
    {
        学前,
        小学,
        初中,
        高中,
        本科,
        研究,
        博士
    }

    public enum Gender
    {
        无性,
        男性,
        女性,
        双性
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