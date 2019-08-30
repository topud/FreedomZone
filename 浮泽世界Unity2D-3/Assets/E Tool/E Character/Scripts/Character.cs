using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

namespace E.Tool
{
    public abstract class Character : Entity<CharacterStaticData,CharacterDynamicData>
    {
        [Header("状态")]
        [ReadOnly] public CharacterState State = CharacterState.Idle;
        [ReadOnly] public float RunBeyondDistance = 5;
        [ReadOnly] public List<Item> NearbyItems = new List<Item>();
        [ReadOnly] public List<Character> NearbyCharacters = new List<Character>();
        public bool IsFaceRight
        {
            get
            {
                return SpriteSorter.transform.localScale.x < 0;
            }
            set
            {
                SpriteSorter.transform.localScale = new Vector3(value ? 1:-1, 1, 1);
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
            
            Rigidbody.mass = StaticData.Weight;
        }
        protected override void Update()
        {
            base.Update();

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

        /// <summary>
        /// 设置数据，默认用于从存档读取数据
        /// </summary>
        /// <param name="data"></param>
        public override void SetDynamicData(CharacterDynamicData data)
        {
            base.SetDynamicData(data);
        }
        /// <summary>
        /// 重置数据，默认用于对象初次生成的数据初始化
        /// </summary>
        public override void ResetDynamicData()
        {
            if (!StaticData)
            {
                Debug.LogError("静态数据不存在，无法设置数据");
                return;
            }

            DynamicData = new CharacterDynamicData
            {
                Name = StaticData.Name,
                Invincible = StaticData.Invincible,

                MaxHealth = StaticData.MaxHealth,
                MaxMind = StaticData.MaxMind,
                MaxPower = StaticData.MaxPower,
                Health = StaticData.MaxHealth,
                Mind = StaticData.MaxMind,
                Power = StaticData.MaxPower,
                HealthRecoveryCoefficient = StaticData.HealthRecoveryCoefficient,
                MindRecoveryCoefficient = StaticData.MindRecoveryCoefficient,
                PowerRecoveryCoefficient = StaticData.PowerRecoveryCoefficient,
                MaxSpeed = StaticData.MaxSpeed,
                BaseSpeed = StaticData.BaseSpeed,
                Intelligence = StaticData.Intelligence,
                Strength = StaticData.Strength,
                Defense = StaticData.Defense,

                RMB = StaticData.RMB,
                FZB = StaticData.FZB,
                Items = new List<Item>(StaticData.Items),
                Skills = new List<Skill>(StaticData.Skills),
                Buffs = StaticData.Buffs,
                AcceptedQuests = StaticData.AcceptedQuests,
                PublishedQuests = StaticData.PublishedQuests,
                Relationships = StaticData.Relationships
            };
        }
        /// <summary>
        /// 设置组件
        /// </summary>
        public override void ResetComponents()
        {
            base.ResetComponents();
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
        
        /// <summary>
        /// 拾取物品
        /// </summary>
        /// <param name="item"></param>
        public void PickUp(Item item)
        {
            if (IsNearby(item))
            {
                item.gameObject.SetActive(false);
                DynamicData.Items.Add(item);
                Debug.Log(string.Format("已拾取 {0}", item.StaticData.Name));
            }
            else
            {
                Debug.LogError(string.Format("无法拾取 {0}，因距离过远", item.StaticData.Name));
            }
        }
        /// <summary>
        /// 调查物品
        /// </summary>
        /// <param name="item"></param>
        public void Survey(Item item)
        {
            if (IsNearby(item))
            {
                item.TargetUI.HideAll();
                TargetUI.ShowChat();
                TargetUI.SetChat(item.StaticData.Describe);
            }
            else
            {
                Debug.LogError(string.Format("无法调查 {0}，因距离过远", item.StaticData.Name));
            }
        }
        /// <summary>
        /// 丢弃物品
        /// </summary>
        /// <param name="item"></param>
        public void Discard(Item item)
        {
            if (IsOwning(item))
            {
                item.transform.position = transform.position;
                item.gameObject.SetActive(true);
                DynamicData.Items.Remove(item);
                Debug.Log(string.Format("已丢弃 {0}", item.StaticData.Name));
            }
            else
            {
                Debug.LogError(string.Format("无法丢弃 {0}，因未携带该物品", item.StaticData.Name));
            }
            //UIManager.Singleton.UIInventory.RefreshContent();
        }
        /// <summary>
        /// 使用物品
        /// </summary>
        /// <param name="item"></param>
        public void Use(Item item)
        {
        }
        /// <summary>
        /// 投掷物品
        /// </summary>
        /// <param name="item"></param>
        public void Throw(Item item)
        {
        }
        /// <summary>
        /// 检测物品是否在拾取范围
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsNearby(Item item)
        {
            return NearbyItems.Contains(item);
        }
        /// <summary>
        /// 检测玩家是否拥有物品
        /// </summary>
        /// <returns></returns>
        public bool IsOwning(Item item)
        {
            return DynamicData.Items.Contains(item);
        }
    
        /// <summary>
        /// 与角色对话
        /// </summary>
        /// <param name="target"></param>
        public void ChatWith(Character target)
        {
            if (IsNearby(target))
            {
                if (target.StaticData.RandomStorys.Count>0)
                {
                    target.TargetUI.HideName();
                    target.TargetUI.ShowChat();
                    string content = target.StaticData.RandomStorys[new System.Random().Next(0, target.StaticData.RandomStorys.Count)];
                    target.TargetUI.SetChat(content);
                    Debug.Log(string.Format("正在与 {0} 对话", target.StaticData.Name));
                }
                else
                {
                    Debug.Log(string.Format("{0} 没有什么想说的", target.StaticData.Name));
                }
            }
            else
            {
                Debug.LogError(string.Format("无与 {0} 对话，因距离过远", target.StaticData.Name));
            }
        }
        /// <summary>
        /// 调查角色
        /// </summary>
        /// <param name="item"></param>
        public void Survey(Character target)
        {
            if (IsNearby(target))
            {
                target.TargetUI.HideAll();
                TargetUI.ShowChat();
                TargetUI.SetChat(target.StaticData.Describe);
            }
            else
            {
                Debug.LogError(string.Format("无法调查 {0}，因距离过远", target.StaticData.Name));
            }
        }
        /// <summary>
        /// 检测角色是否在对话范围
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsNearby(Character target)
        {
            return NearbyCharacters.Contains(target);
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