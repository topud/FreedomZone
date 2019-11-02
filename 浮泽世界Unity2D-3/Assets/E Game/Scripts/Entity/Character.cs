using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Pathfinding;

namespace E.Tool
{
    [RequireComponent(typeof(AIPath))]
    [RequireComponent(typeof(AIDestinationSetter))]
    public class Character : Entity<CharacterStaticData, CharacterDynamicData>
    {
        public static Character Player;
        public static UnityEvent OnPlayerItemChange = new UnityEvent();
        public static UnityEvent OnPlayerInfoChange = new UnityEvent();

        [Header("角色组件")]
        public AIPath AIPath;
        public AIDestinationSetter AIDestinationSetter;
        public EntityUI TargetUI;
        [SerializeField] private InHandItemController RightHandItemController;

        [Header("角色状态")]
        [SerializeField] private bool isPlayer = false;
        [ReadOnly] public CharacterState State = CharacterState.Idle;
        [ReadOnly] public float RunBeyondDistance = 5;
        [ReadOnly] public List<Item> NearbyItems = new List<Item>();
        [ReadOnly] public List<Character> NearbyCharacters = new List<Character>();


        public bool IsPlayer
        {
            get => isPlayer;
            set
            {
                if (value)
                {
                    if (Player)
                    {
                        Player.IsPlayer = false;
                    }

                    Player = this;
                    AIPath.enabled = false;
                    CameraManager.Singleton.SetFollow(transform);
                }
                else
                {
                    AIPath.enabled = true;
                }
                isPlayer = value;
                DynamicData.IsPlayer = value;
            }
        }
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
        public Transform FollowTarget
        {
            get
            {
                return AIDestinationSetter.target;
            }
            set
            {
                AIDestinationSetter.target = value;
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
        }
        protected override void Update()
        {
            base.Update();

            if (AIDestinationSetter.target)
            {
                if (Vector2.Distance(transform.position, AIDestinationSetter.target.transform.position) > RunBeyondDistance)
                {
                    AIPath.maxSpeed = DynamicData.Speed.Max;
                }
                else
                {
                    AIPath.maxSpeed = DynamicData.Speed.Now;
                }
            }
            if (State != CharacterState.Dead && State != CharacterState.Talk)
            {
                CheckNearby();
                CheckAttack();
                CheckCameraSize();

                if (IsPlayer)
                {
                    if (Input.GetKeyUp(KeyCode.LeftBracket))
                    {
                        PutInRightHand(DynamicData.Items[0]);
                    }
                    if (Input.GetKeyUp(KeyCode.RightBracket))
                    {
                        PutInBag();
                    }
                }

            }
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (State != CharacterState.Dead && State != CharacterState.Talk)
            {
                CheckMove();
            }
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

            if (IsPlayer)
            {
                Player = null;
            }
        }
        protected override void Reset()
        {
            base.Reset();
        }
        private void OnValidate()
        {
            IsPlayer = isPlayer;
        }

        /// <summary>
        /// 设置数据，默认用于从存档读取数据
        /// </summary>
        /// <param name="data"></param>
        public override void SetDynamicData(CharacterDynamicData data)
        {
            base.SetDynamicData(data);
        }
        [ContextMenu("重置静态数据")]
        /// <summary>
        /// 重置静态数据
        /// </summary>
        public override void ResetStaticData()
        {
            base.ResetStaticData();
        }
        [ContextMenu("重置动态数据")]
        /// <summary>
        /// 重置动态数据
        /// </summary>
        public override void ResetDynamicData()
        {
            base.ResetDynamicData();

            gameObject.layer = LayerMask.NameToLayer("Character");
            gameObject.tag = IsPlayer ? "Player" : "NPC";
            AIPath.enabled = false;
            AIDestinationSetter.enabled = false;

            if (!StaticData) return;

            TargetUI.SetName(StaticData.Name);
            TargetUI.HideName();
            TargetUI.HideChat();
            TargetUI.HideHelp();

            DynamicData = new CharacterDynamicData
            {
                Name = StaticData.Name,

                Health = StaticData.Health,
                Mind = StaticData.Mind,
                Power = StaticData.Power,
                Speed = StaticData.Speed,
                IQ = StaticData.IQ,
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
        [ContextMenu("重置组件")]
        /// <summary>
        /// 设置组件
        /// </summary>
        public override void ResetComponents()
        {
            base.ResetComponents();

            AIPath = GetComponent<AIPath>();
            AIDestinationSetter = GetComponent<AIDestinationSetter>();
            TargetUI = GetComponentInChildren<EntityUI>(true);
            RightHandItemController = GetComponentInChildren<InHandItemController>();

            if (!AIPath) Debug.LogError("未找到 AIPath");
            if (!AIDestinationSetter) Debug.LogError("未找到 AIDestinationSetter");
            if (!TargetUI) Debug.LogError("未找到 TargetUI");
            if (!RightHandItemController) Debug.LogError("未找到 RightHandItemController");
        }

        /// <summary>
        /// 脑力百分比
        /// </summary>
        /// <returns></returns>
        public float GetMindPercentage()
        {
            return (DynamicData.Mind.Max > 0) ? (float)DynamicData.Mind.Now / DynamicData.Mind.Max : 0;
        }
        /// <summary>
        /// 体力百分比
        /// </summary>
        /// <returns></returns>
        public float GetPowerPercentage()
        {
            return (DynamicData.Power.Max > 0) ? (float)DynamicData.Power.Now / DynamicData.Power.Max : 0;
        }
        /// <summary>
        /// 是否存活
        /// </summary>
        /// <returns></returns>
        public bool IsAlive()
        {
            return DynamicData.Health.Now > 0;
        }

        /// <summary>
        /// 更改当前生命值上限
        /// </summary>
        public void ChangeHealthMax(int value)
        {
            DynamicData.Health.Max += value;
        }
        /// <summary>
        /// 更改当前生命值
        /// </summary>
        public void ChangeHealthNow(int value)
        {
            DynamicData.Health.Now += value;
        }
        /// <summary>
        /// 复活
        /// </summary>
        /// <param name="healthP"></param>
        public void Revive(float healthP = 1, float mindP = 1, float powerP = 1)
        {
            DynamicData.Health.Now = (int)(DynamicData.Health.Max * healthP);
            DynamicData.Mind.Now = (int)(DynamicData.Mind.Max * mindP);
            DynamicData.Power.Now = (int)(DynamicData.Power.Max * powerP);
        }
        /// <summary>
        /// 恢复
        /// </summary>
        public void Recover()
        {
            if (enabled && DynamicData.Health.Now > 0)
            {
                if (DynamicData.Health.AutoChangeable) DynamicData.Health.Now += (int)(0.1 * DynamicData.Health.AutoChangeRate);
                if (DynamicData.Mind.AutoChangeable) DynamicData.Mind.Now +=(int)(0.1 * DynamicData.Mind.AutoChangeRate);
                if (DynamicData.Power.AutoChangeable) DynamicData.Power.Now +=(int)(0.1 * DynamicData.Power.AutoChangeRate);
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

                OnPlayerItemChange.Invoke();
                Debug.Log(string.Format("已拾取 {0}", item.StaticData.Name));
            }
            else
            {
                TargetUI.ShowChat();
                TargetUI.SetChat("它太远了，我捡不到。");
                Debug.LogError(string.Format("无法拾取 {0}，因距离过远", item.StaticData.Name));
            }
        }
        /// <summary>
        /// 调查物品
        /// </summary>
        /// <param name="item"></param>
        public void Survey(Item item)
        {
            TargetUI.ShowChat();
            if (IsNearby(item))
            {
                TargetUI.SetChat(item.StaticData.Describe);
            }
            else
            {
                TargetUI.SetChat("我需要靠近点才能仔细观察。");
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
        /// 将物品放在右手
        /// </summary>
        /// <param name="item"></param>
        public void PutInRightHand(Item item)
        {
            RightHandItemController.SetItem(item, true);
            item.SetPosition(true);
            item.gameObject.SetActive(true);
            Debug.Log(item.StaticData.Name + "已放在手中");
        }
        /// <summary>
        /// 将物品放在背包
        /// </summary>
        /// <param name="item"></param>
        public void PutInBag()
        {
            Debug.Log(RightHandItemController.Item.StaticData.Name + "已放入背包");

            RightHandItemController.Item.SetPosition(false);
            RightHandItemController.Item.gameObject.SetActive(false);
            RightHandItemController.RemoveItem();
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

        /// <summary>
        /// 攻击
        /// </summary>
        public void Attack()
        {
            Animator.SetTrigger("Attack");
        }

        private void CheckMove()
        {
            if (IsPlayer)
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                int currentSpeed = 0;
                if (horizontal != 0 || vertical != 0)
                {
                    Vector2 direction = new Vector2(horizontal, vertical);
                    if (direction.magnitude > 1)
                        direction = direction.normalized;

                    //是否跑步
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        currentSpeed = DynamicData.Speed.Max;
                    }
                    else
                    {
                        currentSpeed = DynamicData.Speed.Now;
                    }
                    Rigidbody.velocity = direction * currentSpeed;

                    // 绘制动线
                    Debug.DrawLine(transform.position, transform.position + (Vector3)direction * currentSpeed, Color.green, 0, false);
                }
                else
                {
                    Rigidbody.velocity = Vector2.zero;
                }

                //朝向（角色面朝鼠标位置）
                Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                IsFaceRight = mousePositionInWorld.x < transform.position.x ? true : false;
                //动画
                Animator.SetInteger("Speed", currentSpeed);
            }
            else
            {
                //朝向
                if (AIPath.desiredVelocity.x >= 0.01f)
                {
                    IsFaceRight = false;
                }
                else if (AIPath.desiredVelocity.x < -0.01f)
                {
                    IsFaceRight = true;
                }
                //动画
                Animator.SetInteger("Speed", Mathf.RoundToInt(AIPath.desiredVelocity.magnitude));
            }
        }
        private void CheckNearby()
        {
            if (IsPlayer)
            {
                Item nearistItem = null;
                if (NearbyItems.Count != 0)
                {
                    //计算所有附近物品离自己的距离
                    List<float> diss = new List<float>();
                    for (int i = 0; i < NearbyItems.Count; i++)
                    {
                        diss.Add(Vector2.Distance(NearbyItems[i].transform.position, transform.position));
                    }
                    //获取离自己最近的物品
                    nearistItem = NearbyItems[Utility.IndexMin(diss)];
                }

                Character nearistCharacter = null;
                if (NearbyCharacters.Count != 0)
                {
                    //计算所有附近角色离自己的距离
                    List<float> diss = new List<float>();
                    for (int i = 0; i < NearbyCharacters.Count; i++)
                    {
                        diss.Add(Vector2.Distance(NearbyCharacters[i].transform.position, transform.position));
                    }
                    //获取离自己最近的角色
                    nearistCharacter = NearbyCharacters[Utility.IndexMin(diss)];
                    foreach (Character it in NearbyCharacters)
                    {
                        if (nearistCharacter != it)
                        {
                            it.TargetUI.HideHelp();
                        }
                    }
                }

                //最近物品离自己的距离
                float ni = nearistItem ? Vector2.Distance(nearistItem.transform.position, transform.position) : -1;
                //最近角色离自己的距离
                float nc = nearistCharacter ? Vector2.Distance(nearistCharacter.transform.position, transform.position) : -1;
                if (ni > nc)
                {
                    if (nearistItem)
                    {
                        //检测是否按键拾取
                        if (Input.GetKeyUp(KeyCode.F)) PickUp(nearistItem);
                        //检测是否按键调查
                        if (Input.GetKeyUp(KeyCode.G)) Survey(nearistItem);
                    }
                    if (nearistCharacter)
                    {
                        nearistCharacter.TargetUI.HideHelp();
                    }
                }
                else if (ni < nc)
                {
                    if (nearistCharacter)
                    {
                        if (!nearistCharacter.TargetUI.IsShowChat() && !TargetUI.IsShowChat())
                        {
                            nearistCharacter.TargetUI.ShowHelp();
                        }
                        //检测是否按键对话
                        if (Input.GetKeyUp(KeyCode.F)) ChatWith(nearistCharacter);
                        //检测是否按键调查
                        if (Input.GetKeyUp(KeyCode.G)) Survey(nearistCharacter);
                    }
                }
            }
        }
        private void CheckAttack()
        {
            if (IsPlayer)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Attack();
                }
            }
        }
        private void CheckCameraSize()
        {
            if (IsPlayer)
            {
                float f = Input.GetAxis("Mouse ScrollWheel");
                CameraManager.Singleton.ChangeOrthographicSize(f);
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