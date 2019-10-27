﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class Item : Entity<ItemStaticData, ItemDynamicData>
    {
        [Header("物品组件")]
        public EntityUI TargetUI;

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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Character character = collision.GetComponent<Character>();
            if (!character) return;

            if (character.IsPlayer)
            {
                SpriteSorter.SetAlpha(0.5f);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            Character character = collision.GetComponent<Character>();
            if (!character) return;

            if (character.IsPlayer)
            {
                SpriteSorter.SetAlpha(1);
            }
        }


        /// <summary>
        /// 设置数据，默认用于从存档读取数据
        /// </summary>
        /// <param name="data"></param>
        public override void SetDynamicData(ItemDynamicData data)
        {
            base.SetDynamicData(data);
        }

        [ContextMenu("重置静态数据")]
        /// <summary>
        /// 重置静态数据
        /// </summary>
        public override void ResetStaticData()
        {

            base.ResetStaticData();
        }
        [ContextMenu("重置动态数据")]
        /// <summary>
        /// 重置数据，默认用于对象初次生成的数据初始化
        /// </summary>
        public override void ResetDynamicData()
        {
            base.ResetDynamicData();

            gameObject.layer = LayerMask.NameToLayer("Item");

            if (!StaticData) return;

            TargetUI.SetName(StaticData.Name);
            TargetUI.HideName();
            TargetUI.HideChat();
            TargetUI.HideHelp();

            DynamicData = new ItemDynamicData
            {
                Name = StaticData.Name,

                Stack = StaticData.Stack,
                Health = StaticData.Health,
                Items = StaticData.Items
            };
            gameObject.tag = StaticData.Movable ? "MovableItem" : "ImmovableItem";
            Rigidbody.bodyType = StaticData.Movable ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
            GetComponent<SpriteRenderer>().sprite = StaticData.Icon;
        }
        [ContextMenu("重置组件")]
        /// <summary>
        /// 设置组件
        /// </summary>
        public override void ResetComponents()
        {
            base.ResetComponents();

            TargetUI = GetComponentInChildren<EntityUI>(true);
            if (!TargetUI) Debug.LogError("未找到 TargetUI");
        }

        /// <summary>
        /// 设置位置状态
        /// </summary>
        /// <param name="isInHand"></param>
        public void SetPosition(bool isInHand)
        {
            Collider.enabled = !isInHand;
            SpriteSorter.enabled = !isInHand;
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