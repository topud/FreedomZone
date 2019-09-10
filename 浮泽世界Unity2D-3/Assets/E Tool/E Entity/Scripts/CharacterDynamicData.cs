using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

namespace E.Tool
{
    [Serializable]
    public class CharacterDynamicData : EntityDynamicData
    {
        [Header("角色实体动态数据")]
        [Tooltip("脑力")] public IntProperty Mind = new IntProperty(20, 20);
        [Tooltip("体力")] public IntProperty Power = new IntProperty(20, 20);
        [Tooltip("速度")] public IntProperty Speed = new IntProperty(5, 2, 0, false, false, 0);
        [Tooltip("智力")] public IntProperty IQ = new IntProperty(100, 5, 0, false, false, 0);
        [Tooltip("力量")] public IntProperty Strength = new IntProperty(100, 5, 0, false, false, 0);
        [Tooltip("防御")] public IntProperty Defense = new IntProperty(100, 1, 0, false, false, 0);

        [Tooltip("当前携带的人民币")] public int RMB = 100;
        [Tooltip("当前携带的浮泽币")] public int FZB = 0;
        [Tooltip("当前携带的物品")] public List<Item> Items = new List<Item>();
        [Tooltip("当前掌握的技能")] public List<Skill> Skills = new List<Skill>();
        [Tooltip("当前拥有的增益")] public List<Buff> Buffs = new List<Buff>();
        [Tooltip("当前接受的任务")] public List<Quest> AcceptedQuests = new List<Quest>();
        [Tooltip("当前发布的任务")] public List<Quest> PublishedQuests = new List<Quest>();
        [Tooltip("当前人际关系")] public List<Relationship> Relationships = new List<Relationship>();

        [Tooltip("是否为玩家操控角色"), ReadOnly] public bool IsPlayer = false;
    }
}