using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    [Serializable]
    public struct Condition
    {
        [Tooltip("条件名")] public string key;
        [Tooltip("初始值"), Range(-100, 100)] public int value;
    }

    [Serializable]
    public class ConditionComparison
    {
        [Tooltip("条件索引")] public int keyIndex;
        [Tooltip("对比方式")] public Comparison comparison;
        [Tooltip("目标值"), Range(-100, 100)] public int value;
    }

    [Serializable]
    public class ConditionChange
    {
        [Tooltip("条件索引")] public int keyIndex;
        [Tooltip("变动方式")] public Change change;
        [Tooltip("变动值"), Range(-100, 100)] public int value;
    }

    public enum Comparison
    {
        大于,
        大于等于,
        小于,
        小于等于,
        等于,
        不等于
    }
    public enum Change
    {
        增加,
        倍乘,
        指定,
    }
}