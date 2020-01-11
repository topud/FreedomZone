using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace E.Tool
{
    public class Item : Entity<ItemStaticData, ItemDynamicData>
    {
        public GameObject SwitchableObject 
        {
            get => transform.Find("SwitchableObject").gameObject; 
        }
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
        public bool IsPowerEnough
        {
            get => DynamicData.Power.Now > 0;
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

        /// <summary>
        /// 使用物品
        /// </summary>
        /// <param name="target"></param>
        /// <param name="user"></param>
        public void Use(Character target)
        {
            string info = "";
            switch (StaticData.Type)
            {
                case ItemType.Food:
                    target.DynamicData.Skills.AddRange(StaticData.Skills);
                    target.DynamicData.Buffs.AddRange(StaticData.Buffs);
                    break;
                case ItemType.Weapon:
                    break;
                case ItemType.Book:
                    break;
                case ItemType.Clothing:
                    target.Clothings.Add(this);
                    target.Items.Remove(this);
                    break;
                case ItemType.Bag:
                    break;
                case ItemType.Switch:
                    info = Switch();
                    break;
                case ItemType.Other:
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(info))
            {
                target.TargetUI.ShowChat();
                target.TargetUI.SetChat(info);
            }
            Debug.Log(target.name + " 使用了" + name);
        }
        private void CheckHealth()
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
        private void CheckPower()
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
                    if (!IsPowerEnough)
                    {
                        if (IsUsing)
                        { SwitchOff(); }
                    }
                    break;
                case ItemType.Other:
                    break;
                default:
                    break;
            }
        }

        #region Switch
        private string Switch()
        {
            string info = "";
            if (StaticData.Type != ItemType.Switch)
            {
                Debug.LogError(name + " 不是开关型物品");
                return info;
            }
            if (!SwitchableObject)
            {
                Debug.LogError("切换对象不存在");
                return info;
            }
            if (IsUsing)
            {
                SwitchOff();
                return info;
            }
            else
            {
                if (IsPowerEnough)
                {
                    SwitchOn();
                }
                else
                {
                    info = string.Format("能量不足，无法打开{0}", StaticData.Name);
                }
                return info;
            }
        }
        private void SwitchOn()
        {
            SwitchableObject.SetActive(true);
            Debug.Log("打开了 " + name);
        }
        private void SwitchOff()
        {
            SwitchableObject.SetActive(false);
            Debug.Log("关闭了 " + name);
        }
        #endregion
    }
}