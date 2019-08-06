using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

[Serializable]
public class SkillDynamicData : DynamicData
{
    [SerializeField, Tooltip("熟练等级")] private SkillLevel level;
    [SerializeField, Tooltip("熟练等级经验"), Range(0, 1)] private float levelEx;

    public SkillLevel Level { get => level; set => level = value; }
    public float LevelEx { get => levelEx; set => levelEx = value; }
}

public enum SkillLevel
{
    生涩,
    熟练,
    巅峰
}