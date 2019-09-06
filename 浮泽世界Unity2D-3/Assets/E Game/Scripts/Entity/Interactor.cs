using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public abstract class Interactor : Entity<InteractorStaticData, InteractorDynamicData>
    {
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void Start()
        {
            base.Start();
            
            gameObject.layer = StaticData.Movable ? 9 : 10;
            Rigidbody.bodyType = StaticData.Movable ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
            Rigidbody.mass = StaticData.Weight;
        }
        protected override void Update()
        {
            base.Update();
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }
        protected override void LateUpdate()
        {
            base.LateUpdate();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        protected override void Reset()
        {
            base.Reset();
        }

        /// <summary>
        /// 设置数据，默认用于从存档读取数据
        /// </summary>
        /// <param name="data"></param>
        public override void SetDynamicData(InteractorDynamicData data)
        {
            base.SetDynamicData(data);
        }
        /// <summary>
        /// 重置数据，默认用于对象初次生成的数据初始化
        /// </summary>
        public override void ResetDynamicData()
        {
            if (!StaticData)
            {
                Debug.LogError("静态数据不存在，无法设置数据");
                return;
            }
            DynamicData = new InteractorDynamicData
            {
                Name = StaticData.Name,
                Invincible = StaticData.Invincible,

                Stack = StaticData.Stack,
                MaxHealth = StaticData.MaxHealth,
                Health = StaticData.MaxHealth,
                Items = StaticData.Items
            };
        }
        /// <summary>
        /// 设置组件
        /// </summary>
        public override void ResetComponents()
        {
            base.ResetComponents();
        }

        /// <summary>
        /// 耐久百分比
        /// </summary>
        /// <returns></returns>
        public float GetHealthPercentage()
        {
            return (DynamicData.MaxHealth > 0) ? (float)DynamicData.Health / DynamicData.MaxHealth : 0;
        }
        /// <summary>
        /// 容量占用比
        /// </summary>
        /// <returns></returns>
        public float GetCapacityPercentage()
        {
            if (StaticData.Accommodatable)
            {
                int vo = 0;
                foreach (Item item in DynamicData.Items)
                {
                    vo += item.StaticData.Volume;
                }
                return (StaticData.Capacity > 0) ? (float)vo / StaticData.Capacity : -1;
            }
            else
            {
                return -1;
            }
        }
    }
}