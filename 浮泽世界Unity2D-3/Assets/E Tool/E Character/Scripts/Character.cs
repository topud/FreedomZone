using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

namespace E.Tool
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class Character : MonoBehaviour
    {
        [Header("角色数据")]
        public CharacterStaticData StaticData;
        public CharacterDynamicData DynamicData;

        [Header("角色状态")]
        [ReadOnly] public CharacterState State = CharacterState.Idle;
        [ReadOnly] public bool IsFaceRight = true;
        [ReadOnly] public float RunBeyondDistance = 5;
        [ReadOnly] public Character Target;
        [ReadOnly] public List<Item> NearbyItems = new List<Item>();
        [ReadOnly] public List<Character> NearbyCharacters = new List<Character>();

        [Header("组件")]
        [ReadOnly] public Collider2D Collider;
        [ReadOnly] public Rigidbody2D Rigidbody;
        [ReadOnly] public AudioSource AudioSource;
        [ReadOnly] public Animator Animator;
        [ReadOnly] public AIPath AIPath;
        [ReadOnly] public AIDestinationSetter AIDestinationSetter;
        [ReadOnly] public TargetUI TargetUI;
        [ReadOnly] public CharacterSprite SpriteController;


        protected virtual void Awake()
        {
            SetComponents();
        }
        protected virtual void OnEnable()
        {
            //数据载入
            ResetData();
            //数据应用，显示更新
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
            if (AIDestinationSetter.target)
            {
                if (Vector2.Distance(transform.position, AIDestinationSetter.target.transform.position) > RunBeyondDistance)
                {
                    AIPath.maxSpeed = DynamicData.MaxSpeed;
                }
                else
                {
                    AIPath.maxSpeed = DynamicData.BaseSpeed;
                }
            }
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
            StaticData = (CharacterStaticData)CharacterStaticData.GetValue(gameObject.name);
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
                UICharacter.Target = this;
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
        public virtual void SetData(CharacterDynamicData data)
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

            DynamicData.MaxHealth = StaticData.MaxHealth;
            DynamicData.MaxMind = StaticData.MaxMind;
            DynamicData.MaxPower = StaticData.MaxPower;
            DynamicData.Health = StaticData.MaxHealth;
            DynamicData.Mind = StaticData.MaxMind;
            DynamicData.Power = StaticData.MaxPower;
            DynamicData.HealthRecoveryCoefficient = StaticData.HealthRecoveryCoefficient;
            DynamicData.MindRecoveryCoefficient = StaticData.MindRecoveryCoefficient;
            DynamicData.PowerRecoveryCoefficient = StaticData.PowerRecoveryCoefficient;
            DynamicData.MaxSpeed = StaticData.MaxSpeed;
            DynamicData.BaseSpeed = StaticData.BaseSpeed;
            DynamicData.Intelligence = StaticData.Intelligence;
            DynamicData.Strength = StaticData.Strength;
            DynamicData.Defense = StaticData.Defense;

            DynamicData.RMB = StaticData.RMB;
            DynamicData.FZB = StaticData.FZB;
            DynamicData.Items = new List<Item>(StaticData.Items);
            DynamicData.Skills = new List<Skill>(StaticData.Skills);
            DynamicData.Buffs = StaticData.Buffs;
            DynamicData.AcceptedQuests = StaticData.AcceptedQuests;
            DynamicData.PublishedQuests = StaticData.PublishedQuests;
            DynamicData.Relationships = StaticData.Relationships;
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
            Animator = GetComponentInChildren<Animator>();
            //插件组件
            AIPath = GetComponent<AIPath>();
            AIDestinationSetter = GetComponent<AIDestinationSetter>();
            //E 组件
            TargetUI = GetComponentInChildren<TargetUI>();
            SpriteController = GetComponentInChildren<CharacterSprite>();
        }

        /// <summary>
        /// 生机百分比
        /// </summary>
        /// <returns></returns>
        public float GetHealthPercentage()
        {
            return (DynamicData.MaxHealth > 0) ? (float)DynamicData.Health / DynamicData.MaxHealth : 0;
        }
        /// <summary>
        /// 脑力百分比
        /// </summary>
        /// <returns></returns>
        public float GetMindPercentage()
        {
            return (DynamicData.MaxMind > 0) ? (float)DynamicData.Mind / DynamicData.MaxMind : 0;
        }
        /// <summary>
        /// 体力百分比
        /// </summary>
        /// <returns></returns>
        public float GetPowerPercentage()
        {
            return (DynamicData.MaxPower > 0) ? (float)DynamicData.Power / DynamicData.MaxPower : 0;
        }
        /// <summary>
        /// 是否存活
        /// </summary>
        /// <returns></returns>
        public bool IsAlive()
        {
            return DynamicData.Health > 0;
        }

        /// <summary>
        /// 更改当前生命值上限
        /// </summary>
        public void ChangeMaxHealth(int value)
        {
            DynamicData.MaxHealth += value;
        }
        /// <summary>
        /// 更改当前生命值
        /// </summary>
        public void ChangeHealth(int value)
        {
            DynamicData.Health += value;
        }
        /// <summary>
        /// 复活
        /// </summary>
        /// <param name="healthP"></param>
        public void Revive(float healthP = 1, float mindP = 1, float powerP = 1)
        {
            DynamicData.Health = Mathf.RoundToInt(DynamicData.MaxHealth * healthP);
            DynamicData.Mind = Mathf.RoundToInt(DynamicData.MaxMind * mindP);
            DynamicData.Power = Mathf.RoundToInt(DynamicData.MaxPower * powerP);
        }
        /// <summary>
        /// 恢复
        /// </summary>
        public void Recover()
        {
            if (enabled && DynamicData.Health > 0)
            {
                DynamicData.Health += DynamicData.HealthRecoveryCoefficient * 1;
                DynamicData.Mind += DynamicData.MindRecoveryCoefficient * 1;
                DynamicData.Power += DynamicData.PowerRecoveryCoefficient * 1;
            }
        }
    }

    public enum CharacterState
    {
        Idle,
        Talk,
        Walk,
        Run,
        Rest,
        Dead
    }
}