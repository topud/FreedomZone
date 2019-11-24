using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    [Serializable]
    public class ConditionComparison
    {
        [Tooltip("条件索引")] public int keyIndex;
        [Tooltip("对比方式")] public Comparison comparison;
        [Tooltip("目标值"), Range(-100, 100)] public int value;
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
}