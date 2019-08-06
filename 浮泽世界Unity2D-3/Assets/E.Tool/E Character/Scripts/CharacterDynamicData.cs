using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Utility;

[Serializable]
public class CharacterDynamicData : DynamicData
{
    [SerializeField] private bool invincible = false;

    [SerializeField] private int maxHealth = 20;
    [SerializeField] private int maxMind = 20;
    [SerializeField] private int maxPower = 20;
    [SerializeField] private int health = 20;
    [SerializeField] private int mind = 20;
    [SerializeField] private int power = 20;
    [SerializeField] private int healthRecoveryCoefficient = 1;
    [SerializeField] private int mindRecoveryCoefficient = 1;
    [SerializeField] private int powerRecoveryCoefficient = 1;
    [SerializeField] private int intelligence = 5;
    [SerializeField] private int speed = 5;
    [SerializeField] private int strength = 5;
    [SerializeField] private int defense = 1;

    [SerializeField] private int rmb = 100;
    [SerializeField] private int fzb = 0;
    [SerializeField] private List<Item> inventory = new List<Item>();
    [SerializeField] private List<Skill> skills = new List<Skill>();
    [SerializeField] private List<Buff> buffs = new List<Buff>();
    [SerializeField] private List<Quest> acceptedQuest = new List<Quest>();
    [SerializeField] private List<Quest> publishedQuest = new List<Quest>();

    public bool Invincible
    {
        get => invincible;
        set => invincible = value;
    }

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
    public int HealthRecoveryCoefficient
    {
        get => healthRecoveryCoefficient;
        set => healthRecoveryCoefficient = value;
    }
    public int MindRecoveryCoefficient
    {
        get => mindRecoveryCoefficient;
        set => mindRecoveryCoefficient = value;
    }
    public int PowerRecoveryCoefficient
    {
        get => powerRecoveryCoefficient;
        set => powerRecoveryCoefficient = value;
    }
    public int Intelligence
    {
        get => intelligence;
        set => intelligence = value > 0 ? value : intelligence;
    }
    public int Speed
    {
        get => speed;
        set => speed = value > 0 ? value : speed;
    }
    public int Strength
    {
        get => strength;
        set => strength = value > 0 ? value : strength;
    }
    public int Defense
    {
        get => defense;
        set => defense = value > 0 ? value : defense;
    }

    public int RMB
    {
        get => rmb;
        set => rmb = value;
    }
    public int FZB
    {
        get => fzb;
        set => fzb = value;
    }
    public List<Item> Inventory
    {
        get => inventory;
        set => inventory = value;
    }
    public List<Skill> Skills
    {
        get => skills;
        set => skills = value;
    }
    public List<Buff> Buffs
    {
        get => buffs;
        set => buffs = value;
    }
    public List<Quest> AcceptedQuest
    {
        get => acceptedQuest;
        set => acceptedQuest = value;
    }
    public List<Quest> PublishedQuest
    {
        get => publishedQuest;
        set => publishedQuest = value;
    }

}
