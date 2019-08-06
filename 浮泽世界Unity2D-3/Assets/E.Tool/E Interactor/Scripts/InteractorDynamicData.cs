using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Utility;

[Serializable]
public class InteractorDynamicData : DynamicData
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int health = 10;
    [SerializeField] private int rmbPrice = 10;

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
    public int Health
    {
        get => health;
        set
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
    public int RMBPrice
    {
        get => rmbPrice;
        set
        {
            if (value < 0)
            {
                rmbPrice = 0;
            }
            else
            {
                rmbPrice = value;
            }
        }
    }
}
