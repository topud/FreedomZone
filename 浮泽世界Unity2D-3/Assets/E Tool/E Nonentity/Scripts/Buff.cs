using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public struct Buff
{
    [Tooltip("增益持续时间（单位秒）")] public int time;

    [Tooltip("生机上限加成")] public int maxHealthBonus;
    [Tooltip("脑力上限加成")] public int maxMindBonus;
    [Tooltip("体力上限加成")] public int maxPowerBonus;
    [Tooltip("生机加成")] public int healthBonus;
    [Tooltip("脑力加成")] public int mindBonus;
    [Tooltip("体力加成")] public int powerBonus;
    [Tooltip("智力加成")] public int intelligenceBonus;
    [Tooltip("速度加成")] public int speedBonus;
    [Tooltip("力量加成")] public int strengthBonus;
    [Tooltip("防御加成")] public int defenseBonus;
}
