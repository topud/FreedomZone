using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

namespace E.Tool
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class Entity<S,D> : MonoBehaviour where S: EntityStaticData where D: EntityDynamicData
    {
        [Header("组件")]
        public Collider2D Collider;
        public Rigidbody2D Rigidbody;
        public AudioSource AudioSource;
        public Animator Animator;
        public AIPath AIPath;
        public AIDestinationSetter AIDestinationSetter;
        public TargetUI TargetUI;
        public SpriteSorter SpriteSorter;

        [Header("数据")]
        public S StaticData;
        public D DynamicData;

        protected virtual void Awake()
        {
            SetComponents();
        }
        protected virtual void OnEnable()
        {
            //数据载入
            ResetData();
            //数据应用，显示更新
            TargetUI.SetName(StaticData.Name);
            TargetUI.HideName();
            TargetUI.HideChat();
        }
        protected virtual void Start()
        {
        }
        protected virtual void Update()
        {
        }
        protected virtual void FixedUpdate()
        {

        }
        protected virtual void LateUpdate()
        {

        }
        protected virtual void OnDisable()
        {

        }
        protected virtual void OnDestroy()
        {

        }
        protected virtual void Reset()
        {
            StaticData = (S)EntityStaticData.GetValue(gameObject.name);
            ResetData();
            SetComponents();
        }

        /// <summary>
        /// 设置数据，默认用于从存档读取数据
        /// </summary>
        /// <param name="data"></param>
        public virtual void SetData(D data)
        {
            if (!StaticData)
            {
                Debug.LogError("静态数据不存在，无法设置数据");
                return;
            }

            if (data.Name == StaticData.Name)
            {
                DynamicData = data;
            }
            else
            {
                Debug.LogError(string.Format("对象名称不匹配，无法设置数据。当前指定对象是 {0}，目标数据指定对象是 {1}",
                    StaticData.Name, data.Name));
            }
        }
        /// <summary>
        /// 重置数据，默认用于对象初次生成的数据初始化
        /// </summary>
        protected virtual void ResetData()
        {
            if (!StaticData)
            {
                Debug.LogError("静态数据不存在，无法设置数据");
                return;
            }

            DynamicData.Name = StaticData.Name;
            DynamicData.Invincible = StaticData.Invincible;
        }
        /// <summary>
        /// 设置组件
        /// </summary>
        protected virtual void SetComponents()
        {
            //自身组件
            Collider = GetComponent<Collider2D>();
            Rigidbody = GetComponent<Rigidbody2D>();
            AudioSource = GetComponent<AudioSource>();
            AIPath = GetComponent<AIPath>();
            AIDestinationSetter = GetComponent<AIDestinationSetter>();
            //子对象组件
            Animator = GetComponentInChildren<Animator>();
            TargetUI = GetComponentInChildren<TargetUI>();
            SpriteSorter = GetComponentInChildren<SpriteSorter>();

            if (!Collider) Debug.LogError("未找到 + Collider");
            if (!Rigidbody) Debug.LogError("未找到 Rigidbody");
            if (!AudioSource) Debug.LogError("未找到 AudioSource");
            if (!AIPath) Debug.LogError("未找到 AIPath");
            if (!AIDestinationSetter) Debug.LogError("未找到 AIDestinationSetter");
            if (!Animator) Debug.LogError("未找到 Animator");
            if (!TargetUI) Debug.LogError("未找到 TargetUI");
            if (!SpriteSorter) Debug.LogError("未找到 SpriteSorter");
        }
    }
}