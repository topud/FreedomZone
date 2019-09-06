using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace E.Tool
{
    public abstract class EntityStaticData : StaticData
    {
        [SerializeField, Tooltip("是否无敌")] protected bool invincible = false;
        
        /// <summary>
        /// 是否无敌
        /// </summary>
        public bool Invincible { get => invincible; }
    }
}
