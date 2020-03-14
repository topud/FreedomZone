using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace E.Tool
{
    [Serializable]
    public struct FloatProperty
    {
        [Tooltip("当前值")] public float now;
        [Tooltip("最小值")] public float min;
        [Tooltip("最大值")] public float max;
        [Tooltip("每秒自动增加")] public float autoAdd;

        /// <summary>
        /// 当前值
        /// </summary>
        public float Now
        {
            get => now;
            set => now = Utility.Clamp(value, min, max);
        }
        /// <summary>
        /// 最大值
        /// </summary>
        public float Max
        {
            get => max;
            set => max = Utility.ClampMin(value, min);
        }
        /// <summary>
        /// 最小值
        /// </summary>
        public float Min
        {
            get => min;
            set => min = Utility.ClampMax(value, max);
        }
        /// <summary>
        /// 当前值百分比
        /// </summary>
        public float NowPercent
        {
            get
            {
                if (max == 0) return 0;
                else return (float)now / max;
            }
            set => now = Mathf.RoundToInt(max * value);
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="max">最大值</param>
        /// <param name="now">当前值</param>
        /// <param name="min">最小值</param>
        /// <param name="changeable">是否可变动当前值</param>
        /// <param name="autoChangeable">是否可自动变动当前值</param>
        /// <param name="changeRate">自动变动速率</param>
        public FloatProperty(float max, float now, float min = 0, float autoAdd = 0)
        {
            if (max < min)
            {
                Debug.LogError("最大值不能小于最小值，已自动交换两个数值");
                float temp = max;
                max = min;
                min = temp;
            }
            if (now < min || now > max)
            {
                Debug.LogError("当前值不能超出最大值与最小值的限定范围，已自动设置为最大值");
                now = max;
            }

            this.max = max;
            this.min = min;
            this.now = now;

            this.autoAdd = autoAdd;
        }
    }
}