using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace E.Tool
{
    public abstract class StaticData : StaticDataDictionary<StaticData>
    {
        [SerializeField, Tooltip("名称")] protected new string name = "";
        [SerializeField, Tooltip("描述"), TextArea(1, 30)] protected string describe = "";
        [SerializeField, Tooltip("图标")] protected Sprite icon = null;
        [SerializeField, Tooltip("预制体")] protected GameObject prefab = null;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get => name; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get => describe; }
        /// <summary>
        /// 图标
        /// </summary>
        public Sprite Icon { get => icon; }
        /// <summary>
        /// 预制体
        /// </summary>
        public GameObject Prefab { get => prefab; }
    }
}
