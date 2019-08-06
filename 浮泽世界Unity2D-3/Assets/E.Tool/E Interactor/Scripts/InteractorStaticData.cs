using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

[CreateAssetMenu(menuName = "E Interactor")]
public class InteractorStaticData : StaticData
{
    [SerializeField, Tooltip("质量")] private int weight = 10;

    [SerializeField, Tooltip("耐久")] private int health = 10;
    [SerializeField, Tooltip("价格")] private int rmbPrice = 10;
    [SerializeField, Tooltip("内置物品")] private List<Item> inventory = new List<Item>();

    [SerializeField, Tooltip("使用后习得的技能")] private List<Skill> skills = new List<Skill>();
    [SerializeField, Tooltip("使用后获得的增益")] private List<Buff> buffs = new List<Buff>();

    public int Weight { get => weight; }
    public int Health { get => health; }
    public int RMBPrice { get => rmbPrice; set => rmbPrice = value; }

    public List<Item> Inventory { get => inventory; }
    public List<Skill> Skills { get => skills; }
    public List<Buff> Buffs { get => buffs; }
}