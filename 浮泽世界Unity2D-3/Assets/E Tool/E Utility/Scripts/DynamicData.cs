using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    public abstract class DynamicData
    {
        [SerializeField, Tooltip("名称"), ReadOnly] protected string name = "";
        [SerializeField, Tooltip("当前是否无敌")] protected bool invincible = false;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get => name;
            set => name = value;
        }
        /// <summary>
        /// 当前是否无敌
        /// </summary>
        public bool Invincible
        {
            get => invincible;
            set => invincible = value;
        }
    }
}