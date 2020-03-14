using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace E.Tool
{
    public class Item : Entity<ItemStaticData, ItemDynamicData>
    {
        [SerializeField, ReadOnly] private List<Item> items = new List<Item>();

        public GameObject SwitchableObject 
        {
            get => transform.Find("SwitchableObject").gameObject; 
        }
        public bool IsUsing
        {
            get
            {
                SwitchableObject.SetActive(DynamicData.isUsing);
                return DynamicData.isUsing;
            }
            set 
            {
                switch (StaticData.type)
                {
                    case ItemType.Food:
                        break;
                    case ItemType.Weapon:
                        break;
                    case ItemType.Ammo:
                        break;
                    case ItemType.Book:
                        break;
                    case ItemType.Switch:
                        SwitchableObject.SetActive(value);
                        Debug.Log(name + " 使用状态 " + value);
                        break;
                    case ItemType.Bag:
                        break;
                    case ItemType.Other:
                        break;
                    default:
                        break;
                }
                DynamicData.isUsing = value;
            }
        }
        public bool IsPowerEnough
        {
            get => DynamicData.power.Now > 0;
        }
        public List<Item> Items
        {
            get => items;
            set
            {
                items = value;
                DynamicData.items.Clear();
                foreach (Item item in Items)
                {
                    DynamicData.items.Add(item.DynamicData.nameID);
                }
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
        protected override void Reset()
        {
            base.Reset();
        }

        [ContextMenu("重置动态数据")]
        /// <summary>
        /// 重置动态数据
        /// </summary>
        public override void ResetDynamicData()
        {
            if (!StaticData)
            {
                Debug.LogError("静态数据未设置，动态数据无法设置");
                return;
            }

            DynamicData = new ItemDynamicData
            {
                nameID = new NameAndID(StaticData.name, IsAsset ? -1 : GameManager.Item.AvailableID),
                position = IsAsset ? new Vector2(0, 0) : new Vector2(transform.position.x, transform.position.y),
                
                health = StaticData.health,
                power = StaticData.power,

                //DynamicData.items = StaticData.Items; 
            };

            Refresh();
        }
        [ContextMenu("刷新对象")]
        /// <summary>
        /// 刷新对象
        /// </summary>
        public override void Refresh()
        {
            if (!StaticData)
            {
                Debug.LogError("静态数据未设置，动态数据无法设置");
                return;
            }
            if (DynamicData == null)
            {
                Debug.Log("动态数据初始化 " + StaticData.name);
                ResetDynamicData();
            }

            gameObject.layer = LayerMask.NameToLayer("Item");
            gameObject.tag = "Item";

            name = IsAsset ? DynamicData.nameID.name : DynamicData.nameID.NameID;
            transform.position = DynamicData.position;
            Rigidbody.mass = StaticData.weight;

            Items.Clear();
            switch (StaticData.type)
            {
                case ItemType.Food:
                    break;
                case ItemType.Weapon:
                    break;
                case ItemType.Ammo:
                    break;
                case ItemType.Book:
                    break;
                case ItemType.Switch:
                    if (SwitchableObject)
                    {
                        SwitchableObject.SetActive(DynamicData.isUsing);
                    }
                    break;
                case ItemType.Bag:
                    foreach (NameAndID item in DynamicData.items)
                    {
                        Item it = GameManager.Item.GetItem(item);
                        if (it)
                        {
                            Items.Add(it);
                        }
                        else
                        {
                            Debug.LogError("找不到物品 " + item.NameID);
                        }
                    }
                    break;
                case ItemType.Other:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 设置碰撞状态
        /// </summary>
        /// <param name="isInHand"></param>
        public void SetCollider(bool isInHand)
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
                vo += item.StaticData.volume;
            }
            return (StaticData.capacity > 0) ? (float)vo / StaticData.capacity : 0;
        }

        /// <summary>
        /// 使用物品
        /// </summary>
        /// <param name="target"></param>
        /// <param name="user"></param>
        public void Use(Role target)
        {
            string info = "";
            switch (StaticData.type)
            {
                case ItemType.Food:
                    target.DynamicData.skills.AddRange(StaticData.skills);
                    break;
                case ItemType.Weapon:
                    break;
                case ItemType.Book:
                    break;
                case ItemType.Ammo:
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
                switch (StaticData.type)
                {
                    case ItemType.Food:
                        break;
                    case ItemType.Weapon:
                        break;
                    case ItemType.Book:
                        break;
                    case ItemType.Ammo:
                        break;
                    case ItemType.Bag:
                        break;
                    case ItemType.Switch:
                        DynamicData.power.Now -= 1;
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
            switch (StaticData.type)
            {
                case ItemType.Food:
                    break;
                case ItemType.Weapon:
                    break;
                case ItemType.Book:
                    break;
                case ItemType.Ammo:
                    break;
                case ItemType.Bag:
                    break;
                case ItemType.Switch:
                    if (!IsPowerEnough)
                    {
                        if (IsUsing)
                        { IsUsing = false; }
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
            if (StaticData.type != ItemType.Switch)
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
                IsUsing = false;
                return info;
            }
            else
            {
                if (IsPowerEnough)
                {
                    IsUsing = true;
                }
                else
                {
                    info = string.Format("能量不足，无法打开{0}", StaticData.name);
                }
                return info;
            }
        }
        #endregion
    }

    [Serializable]
    public class ItemDynamicData : EntityDynamicData
    {
        [Tooltip("是否正在使用")] public bool isUsing = false;
        [Tooltip("快捷键0~9")] public KeyCode hotKey = KeyCode.None;

        [Tooltip("耐久")] public FloatProperty health;
        [Tooltip("能量")] public FloatProperty power;
        [Tooltip("容纳的物品")] public List<NameAndID> items = new List<NameAndID>();
    }
}