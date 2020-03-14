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
        [Tooltip("物品类型")] public ItemType type = ItemType.Food;
        [Tooltip("初始耐久")] public FloatProperty health;
        [Tooltip("初始能量")] public FloatProperty power;
        [Tooltip("价格")] public int rmbPrice = 10;
        [Tooltip("容量")] public int capacity = 0;
        [Tooltip("初始容纳物品")] public List<ItemStack> items = new List<ItemStack>();
        [Tooltip("组成的部件")] public List<ItemStack> components = new List<ItemStack>();
        [Tooltip("使用时/使用后习得的技能")] public List<Skill> skills = new List<Skill>();
        [Tooltip("使用时/使用后获得的增益")] public List<Buff> buffs = new List<Buff>();

        private void OnValidate()
        {
            switch (type)
            {
                case ItemType.Food:
                    capacity = 0;
                    items = new List<ItemStack>();
                    break;
                case ItemType.Weapon:
                    capacity = 0;
                    items = new List<ItemStack>();
                    break;
                case ItemType.Book:
                    capacity = 0;
                    items = new List<ItemStack>();
                    break;
                case ItemType.Ammo:
                    capacity = 0;
                    items = new List<ItemStack>();
                    break;
                case ItemType.Bag:
                    //capacity = 0;
                    //items = new List<ItemStaticData>();
                    items.RemoveAll(x => x.item.type == ItemType.Bag);
                    break;
                case ItemType.Switch:
                    capacity = 0;
                    items = new List<ItemStack>();
                    break;
                case ItemType.Other:
                    capacity = 0;
                    items = new List<ItemStack>();
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
        /// 弹药，耐久表示堆叠数量
        /// </summary>
        Ammo,
        /// <summary>
        /// 使用时进入阅读状态
        /// </summary>
        Book,
        /// <summary>
        /// 打开时为使用状态，如手机、地图、手电筒、收音机，会消耗能量
        /// </summary>
        Switch,
        /// <summary>
        /// 可以提升物品携带上限
        /// </summary>
        Bag,
        /// <summary>
        /// 其他类型
        /// </summary>
        Other
    }
}