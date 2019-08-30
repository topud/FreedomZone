using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    public abstract class EntityDynamicData : DynamicData
    {
        [SerializeField, Tooltip("坐标")] private Vector2 position = new Vector2(0, 0);
        [SerializeField, Tooltip("当前是否无敌")] private bool invincible = false;

        /// <summary>
        /// 坐标
        /// </summary>
        public Vector2 Position
        {
            get => position;
            set => position = value;
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