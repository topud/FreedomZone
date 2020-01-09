using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Pathfinding;

namespace E.Tool
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class Entity<S,D> : MonoBehaviour where S: EntityStaticData where D: EntityDynamicData
    {
        public Collider2D Collider { get => GetComponent<Collider2D>(); }
        public Rigidbody2D Rigidbody { get => GetComponent<Rigidbody2D>(); }
        public AudioSource AudioSource { get => GetComponent<AudioSource>(); }
        public Animator Animator { get => GetComponent<Animator>(); }
        public SpriteSorter SpriteSorter { get => GetComponent<SpriteSorter>(); }
        public EventTrigger EventTrigger { get => GetComponent<EventTrigger>(); }

        [Header("实体数据")]
        public S StaticData;
        public D DynamicData;
        [SerializeField, ReadOnly] private List<Item> items = new List<Item>();
        public List<Item> Items
        {
            get => items;
            set
            {
                items = value;
                DynamicData.ItemIDs.Clear();
                foreach (Item item in Items)
                {
                    DynamicData.ItemIDs.Add(item.gameObject.GetInstanceID());
                }
            }
        }

        protected virtual void Awake()
        {
        }
        protected virtual void OnEnable()
        {
        }
        protected virtual void Start()
        {
        }
        protected virtual void Update()
        {
            DynamicData.Position = transform.position;
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

        public virtual void OnPointerEnter()
        {
            //Debug.Log("光标进入 " + name);
            SpriteSorter.SetAlpha(0.5f);
        }
        public virtual void OnPointerExit()
        {
            //Debug.Log("光标离开 " + name);
            SpriteSorter.SetAlpha(1f);
        }

        /// <summary>
        /// 设置静态数据
        /// </summary>
        public virtual void SetStaticData(string name)
        {
            S sData = (S)EntityStaticData.GetValue(name);
            if (sData)
            {
                StaticData = sData;
            }
            else
            {
                Debug.LogError("静态数据不存在：" + name);
            }
        }
        /// <summary>
        /// 设置动态数据
        /// </summary>
        /// <param name="data"></param>
        public virtual void SetDynamicData(D data)
        {
            if (!StaticData)
            {
                Debug.LogError("静态数据不存在，无法设置数据");
                return;
            }

            if (data.Name == StaticData.Name)
            {
                DynamicData = data;
                transform.position = data.Position;

                Items.Clear();
                foreach (int item in DynamicData.ItemIDs)
                {
                    Item it = ItemManager.GetItem(item);
                    if (it)
                    {
                        Items.Add(it);
                    }
                    else
                    {
                        Debug.LogError("找不到物品ID " + it);
                    }
                }
            }
            else
            {
                Debug.LogError(string.Format("对象名称不匹配，无法设置数据。当前指定对象是 {0}，目标数据指定对象是 {1}",
                    StaticData.Name, data.Name));
            }
        }

        [ContextMenu("刷新数据")]
        /// <summary>
        /// 刷新数据
        /// </summary>
        public virtual void Refresh()
        {
            ResetStaticData();
            ResetDynamicData();
        }
        [ContextMenu("初始化数据")]
        /// <summary>
        /// 初始化数据
        /// </summary>
        public virtual void Reset()
        {
            ResetStaticData();
            ResetDynamicData(false);
        }
        /// <summary>
        /// 重置静态数据
        /// </summary>
        public virtual void ResetStaticData()
        {
            string[] sts = gameObject.name.Split('-');
            S sData = (S)EntityStaticData.GetValue(sts[0]);
            if (sData)
            {
                StaticData = sData;
            }
        }
        /// <summary>
        /// 重置动态数据
        /// </summary>
        public virtual void ResetDynamicData(bool isAddID = true)
        {
            if (!StaticData)
            {
                Debug.LogError("静态数据未设置，动态数据无法设置");
                return;
            }

            if (isAddID)
            {
                name = StaticData.Name + gameObject.GetInstanceID();
            }
            else
            {
                name = StaticData.Name;
            }
            Rigidbody.mass = StaticData.Weight;
        }
    }
}