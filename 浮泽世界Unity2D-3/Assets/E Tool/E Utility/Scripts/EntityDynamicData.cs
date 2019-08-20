using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    public abstract class EntityDynamicData : DynamicData
    {
        [SerializeField, Tooltip("当前是否无敌")] protected bool invincible = false;
        
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