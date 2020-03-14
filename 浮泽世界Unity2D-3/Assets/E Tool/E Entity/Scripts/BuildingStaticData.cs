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
        [Tooltip("价格")] public int rmbPrice = 10;
        [Tooltip("耐久")] public FloatProperty health;
    }
}