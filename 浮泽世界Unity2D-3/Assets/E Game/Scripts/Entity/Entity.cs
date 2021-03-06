using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Pathfinding;
using UnityEngine.AddressableAssets;
using UnityEditor;

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
        
        public bool IsAsset
        {
            get
            {
                string assetPath = AssetDatabase.GetAssetPath(gameObject);
                return !string.IsNullOrEmpty(assetPath);
            }
        }

        protected virtual void Awake()
        {
        }
        protected virtual void OnEnable()
        {
            Refresh();
        }
        protected virtual void Start()
        {
        }
        protected virtual void Update()
        {
            DynamicData.position = transform.position;
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
        public void SetStaticData(string name)
        {
            S sData = Addressables.LoadAsset<S>(name).Result;
            if (sData)
            {
                StaticData = sData;
                ResetDynamicData();
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
                Debug.LogError("静态数据不存在，动态数据无法设置");
                return;
            }
            if (data.nameID.name != StaticData.name)
            {
                Debug.LogError(string.Format("对象名称不匹配，无法设置数据。当前指定对象是 {0}，目标数据指定对象是 {1}",
                    StaticData.name, data.nameID));
                return;
            }

            DynamicData = data;
            Refresh();
        }
        /// <summary>
        /// 重置动态数据
        /// </summary>
        public abstract void ResetDynamicData();
        /// <summary>
        /// 刷新对象
        /// </summary>
        public abstract void Refresh();
    }

    public abstract class EntityDynamicData
    {
        [Header("实体动态数据")]
        [Tooltip("名称与编号")] public NameAndID nameID = new NameAndID("NoName", 0);
        [Tooltip("坐标")] public Vector2 position = new Vector2(0, 0);
    }

    [Serializable]
    public struct NameAndID
    {
        [ReadOnly] public string name;
        public int id;

        public NameAndID(string name, int id)
        {
            this.name = name;
            this.id = id;
        }
        public string NameID
        {
            get => name + id.ToString();
        }
    }
}