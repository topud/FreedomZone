using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    public abstract class DynamicData
    {
        [SerializeField, Tooltip("名称"), ReadOnly] protected string name = "";
       
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get => name;
            set => name = value;
        }
    }
}