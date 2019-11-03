using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class Item : Entity<ItemStaticData, ItemDynamicData>
    {
        [Tooltip("可以切换激活状态的子对象")] public GameObject SwitchableObject;

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
            gameObject.tag = "Item";

            if (!StaticData) return;

            GetComponent<SpriteRenderer>().sprite = StaticData.Icon;

             DynamicData = new ItemDynamicData()
            {
                Name = StaticData.name,
                ID = gameObject.GetInstanceID(),
                //Position

                Health = StaticData.Health,
                //ItemInstanceIDs
            };
        }
        [ContextMenu("重置组件")]
        /// <summary>
        /// 设置组件
        /// </summary>
        public override void ResetComponents()
        {
            base.ResetComponents();

            SwitchableObject = transform.GetChild(0).gameObject;
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
            int vo = 0;
            foreach (Item item in Items)
            {
                vo += item.StaticData.Volume;
            }
            return (StaticData.Capacity > 0) ? (float)vo / StaticData.Capacity : 0;
        }

        public void SwitchState()
        {
            if (SwitchableObject)
            {
                SwitchableObject.SetActive(!SwitchableObject.activeInHierarchy);
            }
        }
        /// <summary>
        /// 更新物品数据
        /// </summary>
        public void UpdateItemDatas()
        {
            DynamicData.ItemIDs.Clear();
            foreach (Item item in Items)
            {
                DynamicData.ItemIDs.Add(item.gameObject.GetInstanceID());
            }
        }
    }
}