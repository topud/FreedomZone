using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    [CreateAssetMenu(menuName = "E Item")]
    public class ItemStaticData : EntityStaticData
    {
        [Header("物品实体静态数据")]
        [SerializeField, Tooltip("物品类型")] private ItemType type = ItemType.Food;
        [SerializeField, Tooltip("价格")] private int rmbPrice = 10;
        [SerializeField, Tooltip("容量")] private int capacity = 0;
        [SerializeField, Tooltip("初始容纳物品")] private List<ItemStaticData> items = new List<ItemStaticData>();
        [SerializeField, Tooltip("组成的部件")] private List<ItemStaticData> components = new List<ItemStaticData>();
        [SerializeField, Tooltip("使用后习得的技能")] private List<Skill> skills = new List<Skill>();
        [SerializeField, Tooltip("使用后获得的增益")] private List<Buff> buffs = new List<Buff>();

        /// <summary>
        /// 物品类型
        /// </summary>
        public ItemType Type { get => type; }
        /// <summary>
        /// 价格
        /// </summary>
        public int RMBPrice { get => rmbPrice; }
        /// <summary>
        /// 容量
        /// </summary>
        public int Capacity { get => capacity; }
        /// <summary>
        /// 初始容纳物品
        /// </summary>
        public List<ItemStaticData> Items { get => items; }
        /// <summary>
        /// 组成的部件
        /// </summary>
        public List<ItemStaticData> Components { get => components; }
        /// <summary>
        /// 使用后习得的技能
        /// </summary>
        public List<Skill> Skills { get => skills; }
        /// <summary>
        /// 使用后获得的增益
        /// </summary>
        public List<Buff> Buffs { get => buffs; }

        private void OnValidate()
        {
            switch (Type)
            {
                case ItemType.Food:
                    capacity = 0;
                    items = new List<ItemStaticData>();
                    break;
                case ItemType.Weapon:
                    capacity = 0;
                    items = new List<ItemStaticData>();
                    break;
                case ItemType.Book:
                    capacity = 0;
                    items = new List<ItemStaticData>();
                    break;
                case ItemType.Clothing:
                    capacity = 0;
                    items = new List<ItemStaticData>();
                    break;
                case ItemType.Bag:
                    //capacity = 0;
                    //items = new List<ItemStaticData>();
                    items.RemoveAll(x => x.Type == ItemType.Bag);
                    break;
                case ItemType.Switch:
                    capacity = 0;
                    items = new List<ItemStaticData>();
                    break;
                case ItemType.Other:
                    capacity = 0;
                    items = new List<ItemStaticData>();
                    break;
                default:
                    break;
            }
        }
    }

    public enum ItemType
    {
        /// <summary>
        /// 使用时，食用，长时间食用会逐渐腐烂，health表示为新鲜程度
        /// </summary>
        Food,
        /// <summary>
        /// 使用时，攻击，每次有效攻击时会受到损伤，health表示为耐久
        /// </summary>
        Weapon,
        /// <summary>
        /// 使用时进入阅读状态
        /// </summary>
        Book,
        /// <summary>
        /// 穿在身上时即为使用状态，health表示为洁净程度
        /// </summary>
        Clothing,
        /// <summary>
        /// 可以在里面放其他物品
        /// </summary>
        Bag,
        /// <summary>
        /// 使用后会切换使用状态，如手机、地图、手电筒、收音机，health表示为剩余能量
        /// </summary>
        Switch,
        /// <summary>
        /// 其他类型
        /// </summary>
        Other
    }
}