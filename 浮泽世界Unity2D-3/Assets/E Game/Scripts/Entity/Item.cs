using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace E.Tool
{
    public class Item : Entity<ItemStaticData, ItemDynamicData>
    {
        public GameObject SwitchableObject { get => transform.Find("SwitchableObject").gameObject; }

        public bool IsUsing
        {
            get
            {
                bool isUsing = false;
                switch (StaticData.Type)
                {
                    case ItemType.Food:
                        break;
                    case ItemType.Weapon:
                        break;
                    case ItemType.Book:
                        break;
                    case ItemType.Clothing:
                        break;
                    case ItemType.Bag:
                        break;
                    case ItemType.Switch:
                        isUsing = SwitchableObject.activeSelf;
                        break;
                    case ItemType.Other:
                        break;
                    default:
                        break;
                }
                return isUsing;
            }
        }

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

            InvokeRepeating("CheckHealth", 1, 1);

        }
        protected override void Update()
        {
            base.Update();

            CheckPower();
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

        /// <summary>
        /// 设置数据，默认用于从存档读取数据
        /// </summary>
        /// <param name="data"></param>
        public override void SetDynamicData(ItemDynamicData data)
        {
            base.SetDynamicData(data);
        }

        [ContextMenu("刷新数据")]
        /// <summary>
        /// 刷新数据
        /// </summary>
        public override void Refresh()
        {
            ResetStaticData();
            ResetDynamicData();
        }
        [ContextMenu("初始化数据")]
        /// <summary>
        /// 初始化数据
        /// </summary>
        public override void Reset()
        {
            base.Reset();
        }
        /// <summary>
        /// 重置静态数据
        /// </summary>
        public override void ResetStaticData()
        {
            base.ResetStaticData();

            SpriteSorter.SetSprite(StaticData.Icon);
        }
        /// <summary>
        /// 重置数据，默认用于对象初次生成的数据初始化
        /// </summary>
        public override void ResetDynamicData(bool isAddID = true)
        {
            base.ResetDynamicData(isAddID);

            gameObject.layer = LayerMask.NameToLayer("Item");
            gameObject.tag = "Item";

            if (!StaticData) return;

             DynamicData = new ItemDynamicData()
             {
                Name = StaticData.name,
                ID = gameObject.GetInstanceID(),
                //Position

                 Health = StaticData.Health,
                 Power = StaticData.Power,
                 //ItemInstanceIDs
             };
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

        public void TrySwitchUse()
        {
            switch (StaticData.Type)
            {
                case ItemType.Food:
                    break;
                case ItemType.Weapon:
                    break;
                case ItemType.Book:
                    break;
                case ItemType.Clothing:
                    break;
                case ItemType.Bag:
                    break;
                case ItemType.Switch:
                    if (SwitchableObject)
                    {
                        if (IsUsing)
                        {
                            SwitchableObject.SetActive(false);
                            Debug.Log("关闭了 " + name);
                        }
                        else
                        {
                            if (DynamicData.Power.Now > 0)
                            {
                                SwitchableObject.SetActive(true);
                                Debug.Log("打开了 " + name);
                            }
                            else
                            {
                                Debug.Log("能量不足，无法打开 " + name);
                            }
                        }
                    }
                    break;
                case ItemType.Other:
                    break;
                default:
                    break;
            }
        }
        public void CheckHealth()
        {
            if (IsUsing)
            {
                switch (StaticData.Type)
                {
                    case ItemType.Food:
                        break;
                    case ItemType.Weapon:
                        break;
                    case ItemType.Book:
                        break;
                    case ItemType.Clothing:
                        break;
                    case ItemType.Bag:
                        break;
                    case ItemType.Switch:
                        DynamicData.Power.Now -= 1;
                        break;
                    case ItemType.Other:
                        break;
                    default:
                        break;
                }
            }
        }
        public void CheckPower()
        {
            switch (StaticData.Type)
            {
                case ItemType.Food:
                    break;
                case ItemType.Weapon:
                    break;
                case ItemType.Book:
                    break;
                case ItemType.Clothing:
                    break;
                case ItemType.Bag:
                    break;
                case ItemType.Switch:
                    if (DynamicData.Power.Now == 0)
                    {
                        if (IsUsing)
                        { TrySwitchUse(); }
                    }
                    break;
                case ItemType.Other:
                    break;
                default:
                    break;
            }
        }
    }
}