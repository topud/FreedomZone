using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    [Serializable]
    public struct ConditionComparison
    {
        [Tooltip("条件索引")] public int KeyIndex;
        [Tooltip("对比方式")] public Comparison Comparison;
        [Tooltip("目标值"), Range(-100, 100)] public int Value;

        public void SetIndex(int index)
        {
            KeyIndex = index;
        }
        public void SetComparison(Comparison comparison)
        {
            Comparison = comparison;
        }
        public void SetValue(int value)
        {
            Value = value;
        }
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