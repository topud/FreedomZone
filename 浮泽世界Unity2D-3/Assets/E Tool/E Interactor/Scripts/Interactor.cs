using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class Interactor : MonoBehaviour
    {
        [Header("物品数据")]
        public InteractorStaticData StaticData;
        public InteractorDynamicData DynamicData;

        [Header("物品状态")]
        [ReadOnly] public bool IsFaceRight = true;

        [Header("组件")]
        [ReadOnly] public Collider2D Collider;
        [ReadOnly] public Rigidbody2D Rigidbody;
        [ReadOnly] public AudioSource AudioSource;
        [ReadOnly] public Animator Animator;
        [ReadOnly] public TargetUI TargetUI;
        [ReadOnly] public InteractorSprite SpriteController;


        protected virtual void Awake()
        {
            SetComponents();
        }
        protected virtual void OnEnable()
        {
            //数据载入
            ResetData();
            //数据应用，显示更新
            gameObject.layer = StaticData.Movable ? 9 : 10;
            Rigidbody.bodyType = StaticData.Movable ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
            Rigidbody.mass = StaticData.Weight;
            TargetUI.SetName(StaticData.Name);
            TargetUI.HideName();
            TargetUI.HideTalk();
        }
        protected virtual void Start()
        {
        }
        protected virtual void Update()
        {
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
            StaticData = (InteractorStaticData)InteractorStaticData.GetValue(gameObject.name);
            ResetData();
            SetComponents();
        }
        private void OnMouseEnter()
        {
            SpriteController.SetColor(new Color(0.8f, 0.8f, 0.8f));
            TargetUI.ShowName();
        }
        private void OnMouseOver()
        {
            if (Input.GetMouseButtonUp(1))
            {
                //UIManager.Singleton.uiCharacterInfo.SetCharacterEntity(this);
            }
        }
        private void OnMouseDown()
        {
            SpriteController.SetColor(new Color(0.6f, 0.6f, 0.6f));
        }
        private void OnMouseDrag()
        {

        }
        private void OnMouseUp()
        {
            SpriteController.SetColor(new Color(0.8f, 0.8f, 0.8f));
        }
        private void OnMouseExit()
        {
            SpriteController.SetColor(new Color(1, 1, 1));
            TargetUI.HideName();
        }

        /// <summary>
        /// 设置数据，默认用于从存档读取数据
        /// </summary>
        /// <param name="data"></param>
        public virtual void SetData(InteractorDynamicData data)
        {
            if (!StaticData)
            {
                Debug.LogError("静态数据不存在，无法设置数据");
                return;
            }

            if (data.Name == StaticData.Name)
            {
                DynamicData = data;
            }
            else
            {
                Debug.LogError(string.Format("对象名称不匹配，无法设置数据。当前指定对象是 {0}，目标数据指定对象是 {1}",
                    StaticData.Name, data.Name));
            }
        }
        /// <summary>
        /// 重置数据，默认用于对象初次生成的数据初始化
        /// </summary>
        protected virtual void ResetData()
        {
            if (!StaticData)
            {
                Debug.LogError("静态数据不存在，无法设置数据");
                return;
            }

            DynamicData.Name = StaticData.Name;
            DynamicData.Invincible = StaticData.Invincible;

            DynamicData.Stack = StaticData.Stack;
            DynamicData.MaxHealth = StaticData.MaxHealth;
            DynamicData.Health = StaticData.MaxHealth;
            DynamicData.Items = StaticData.Items;
        }
        /// <summary>
        /// 设置组件
        /// </summary>
        protected virtual void SetComponents()
        {
            //自带组件
            Collider = GetComponent<Collider2D>();
            Rigidbody = GetComponent<Rigidbody2D>();
            AudioSource = GetComponent<AudioSource>();
            Animator = GetComponent<Animator>();
            //E 组件
            TargetUI = GetComponentInChildren<TargetUI>();
            SpriteController = GetComponent<InteractorSprite>();
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