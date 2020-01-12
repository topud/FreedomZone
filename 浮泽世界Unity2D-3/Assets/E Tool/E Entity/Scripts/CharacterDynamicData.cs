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
        [Tooltip("脑力")] public IntProperty mind = new IntProperty(20, 20);
        [Tooltip("速度")] public IntProperty speed = new IntProperty(5, 2, 0, false, false, 0);
        [Tooltip("智力")] public IntProperty iq = new IntProperty(100, 5, 0, false, false, 0);
        [Tooltip("力量")] public IntProperty strength = new IntProperty(100, 5, 0, false, false, 0);
        [Tooltip("防御")] public IntProperty defense = new IntProperty(100, 1, 0, false, false, 0);

        [Tooltip("当前携带的人民币")] public int rmb = 100;
        [Tooltip("当前携带的浮泽币")] public int fzb = 0;
        [Tooltip("当前掌握的技能")] public List<Skill> skills = new List<Skill>();
        [Tooltip("当前拥有的增益")] public List<Buff> buffs = new List<Buff>();
        [Tooltip("当前接受的任务")] public List<Quest> acceptedQuests = new List<Quest>();
        [Tooltip("当前发布的任务")] public List<Quest> publishedQuests = new List<Quest>();
        [Tooltip("当前人际关系")] public List<Relationship> relationships = new List<Relationship>();
    }
}