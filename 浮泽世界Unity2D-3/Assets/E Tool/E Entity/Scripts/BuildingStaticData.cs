using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    [CreateAssetMenu(menuName = "E Building")]
    public class BuildingStaticData : EntityStaticData
    {
        [Header("建筑实体静态数据")]
        [SerializeField, Tooltip("价格")] private int rmbPrice = 10;

        /// <summary>
        /// 价格
        /// </summary>
        public int RMBPrice { get => rmbPrice; }
    }
}