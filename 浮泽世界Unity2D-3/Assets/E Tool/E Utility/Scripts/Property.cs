using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{

    [Serializable]
    public struct IntProperty
    {
        [SerializeField, Tooltip("是否可变动当前值")] private bool changeable;
        [SerializeField, Tooltip("是否可自动变动当前值")] private bool autoChangeable;
        [SerializeField, Tooltip("自动变动速率")] private int autoChangeRate;
        [SerializeField, Tooltip("最大值")] private int max;
        [SerializeField, Tooltip("最小值")] private int min;
        [SerializeField, Tooltip("当前值")] private int now;

        /// <summary>
        /// 是否可变动当前值
        /// </summary>
        public bool Changeable
        {
            get => changeable;
            set => changeable = value;
        }
        /// <summary>
        /// 是否可自动变动当前值
        /// </summary>
        public bool AutoChangeable
        {
            get => autoChangeable;
            set => autoChangeable = value;
        }
        /// <summary>
        /// 自动变动速率
        /// </summary>
        public int AutoChangeRate
        {
            get => autoChangeRate;
            set => autoChangeRate = value;
        }
        /// <summary>
        /// 最大值
        /// </summary>
        public int Max
        {
            get => max;
            set => max = Utility.ClampMin(value, min);
        }
        /// <summary>
        /// 最小值
        /// </summary>
        public int Min
        {
            get => min;
            set => min = Utility.ClampMax(value, max);
        }
        /// <summary>
        /// 当前值
        /// </summary>
        public int Now
        {
            get => now;
            set => now = Utility.Clamp(value, min, max);
        }

        public IntProperty(int max, int now, int min = 0, bool changeable = true, bool autoChangeable = true, int changeRate = 1)
        {
            if (max < min)
            {
                Debug.LogError("最大值不能小于最小值，已自动交换两个数值");
                int temp = max;
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

            this.changeable = changeable;
            this.autoChangeable = autoChangeable;
            this.autoChangeRate = changeRate;
        }
    }
}