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
        [SerializeField, ReadOnly] private CharacterState state = CharacterState.Idle;
        [SerializeField, ReadOnly] private bool isPlayer = false;
        [SerializeField, ReadOnly] private int currentSpeed = 0;
        [SerializeField, ReadOnly] private float RunBeyondDistance = 5;
        [SerializeField, ReadOnly] private Item lastPutInBagItem;
        [SerializeField, ReadOnly] private List<Item> clothings = new List<Item>();
        [SerializeField, ReadOnly] private Item nearistItem;
        [SerializeField, ReadOnly] private Character nearistCharacter;
        [SerializeField, ReadOnly] private GameObject nearistEntity;
        [SerializeField, ReadOnly] private GameObject lastNearistEntity;
        [SerializeField, ReadOnly] private List<Item> nearbyItems = new List<Item>();
        [SerializeField, ReadOnly] private List<Character> nearbyCharacters = new List<Character>();

        /// <summary>
        /// 角色状态
        /// </summary>
        public CharacterState State
        {
            get => state;
            private set => state = value;
        }
        /// <summary>
        /// 是否玩家控制角色
        /// </summary>
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
                    CameraManager.SetFollow(transform);
                }
                else
                {
                    AIPath.enabled = true;
                }
                isPlayer = value;
                DynamicData.IsPlayer = value;
            }
        }
        /// <summary>
        /// 穿着物品
        /// </summary>
        public List<Item> Clothings
        {
            get => clothings;
            set
            {
                clothings = value;
                DynamicData.Clothings.Clear();
                foreach (Item item in Items)
                {
                    DynamicData.Clothings.Add(item.gameObject.GetInstanceID());
                }
            }
        }
        /// <summary>
        /// 是否面朝右边
        /// </summary>
        public bool IsFaceRight
        {
            get
            {
                return SpriteSorter.transform.localScale.x < 0;
            }
            set
            {
                SpriteSorter.transform.localScale = new Vector3(value ? 1 : -1, 1, 1);
            }
        }
        /// <summary>
        /// 是否存活
        /// </summary>
        public bool IsAlive
        {
            get
            {
                return DynamicData.Health.Now > 0;
            }
        }
        /// <summary>
        /// 跟随目标
        /// </summary>
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
        /// <summary>
        /// 距离最近的物品
        /// </summary>
        public Item NearistItem
        {
            get => nearistItem;
            private set => nearistItem = value;
        }
        /// <summary>
        /// 距离最近的角色
        /// </summary>
        public Character NearistCharacter
        {
            get => nearistCharacter;
            private set => nearistCharacter = value;
        }
        /// <summary>
        /// 距离最近的实体
        /// </summary>
        public GameObject NearistEntity
        {
            get => nearistEntity;
            private set => nearistEntity = value;
        }
        /// <summary>
        /// 附近的物品
        /// </summary>
        public List<Item> NearbyItems
        {
            get => nearbyItems;
            set => nearbyItems = value;
        }
        /// <summary>
        /// 附近的角色
        /// </summary>
        public List<Character> NearbyCharacters
        {
            get => nearbyCharacters;
            set => nearbyCharacters = value;
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
                CheckAIPathMaxSpeed();
            }
            if (State != CharacterState.Dead && State != CharacterState.Talk)
            {
                if (IsPlayer)
                {
                    if (!UIManager.IsShowAnyUIPanel)
                    {
                        CheckNearistItem();
                        CheckNearistCharacter();
                        CheckNearistEntity();

                        CheckKeyUp_E();
                        CheckKeyUp_F();
                        CheckKeyUp_Q();
                        CheckMouseButtonDown_0();
                        CheckMouseButtonDown_1();
                        CheckMouseButtonDown_2();
                        CheckMouseScrollWheelWithAlt();

                        CheckPlayerFaceTo();
                        CheckPlayerAnimation();

                        RightHandItemController.SetIsLookAtCursor(true);
                    }
                    else
                    {
                        RightHandItemController.SetIsLookAtCursor(false);
                    }
                    CheckMouseScrollWheelWithoutAlt();
                }
                else
                {
                    CheckNPCFaceTo();
                    CheckNPCAnimation();
                }
            }
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (State != CharacterState.Dead && State != CharacterState.Talk)
            {
                if (IsPlayer)
                {
                    if (!UIManager.IsShowAnyUIPanel)
                    {
                        CheckMove();
                    }
                }
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

        /// <summary>
        /// 设置数据，默认用于从存档读取数据
        /// </summary>
        /// <param name="data"></param>
        public override void SetDynamicData(CharacterDynamicData data)
        {
            base.SetDynamicData(data);

            IsPlayer = data.IsPlayer;
        }
        /// <summary>
        /// 重置静态数据
        /// </summary>
        [ContextMenu("重置静态数据")]
        public override void ResetStaticData()
        {
            base.ResetStaticData();
        }
        /// <summary>
        /// 重置动态数据
        /// </summary>
        [ContextMenu("重置动态数据")]
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

            DynamicData = new CharacterDynamicData
            {
                Name = StaticData.Name,
                ID = gameObject.GetInstanceID(),
                //Position

                Health = StaticData.Health,
                Mind = StaticData.Mind,
                Power = StaticData.Power,
                Speed = StaticData.Speed,
                IQ = StaticData.IQ,
                Strength = StaticData.Strength,
                Defense = StaticData.Defense,

                RMB = StaticData.RMB,
                FZB = StaticData.FZB,
                //ItemInstanceIDs
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
        [ContextMenu("重置组件")]
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
        /// 设为玩家控制角色
        /// </summary>
        [ContextMenu("设为玩家控制角色")]
        public void SetPlayer()
        {
            IsPlayer = true;
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
                if (DynamicData.Mind.AutoChangeable) DynamicData.Mind.Now += (int)(0.1 * DynamicData.Mind.AutoChangeRate);
                if (DynamicData.Power.AutoChangeable) DynamicData.Power.Now += (int)(0.1 * DynamicData.Power.AutoChangeRate);
            }
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
        /// 检测玩家是否携带物品
        /// </summary>
        /// <returns></returns>
        public bool IsCarrying(Item item)
        {
            return Items.Contains(item);
        }
        /// <summary>
        /// 检测玩家是否携带物品
        /// </summary>
        /// <returns></returns>
        public bool IsWearing(Item item)
        {
            return Clothings.Contains(item);
        }
        /// <summary>
        /// 获取物品排序
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int GetIndex(Item item)
        {
            return Items.IndexOf(item);
        }

        /// <summary>
        /// 拾取物品
        /// </summary>
        /// <param name="item"></param>
        public void PickUp(Item item)
        {
            if (!item)
            {
                Debug.LogError("没有物品可拾取");
                return;
            }
            if (IsNearby(item))
            {
                NearbyItems.Remove(item);

                item.gameObject.SetActive(false);
                Items.Add(item);
                PutRightHandItemInBag();
                PutItemInRightHand(item);

                OnPlayerItemChange.Invoke();
                Debug.Log(string.Format("已拾取 {0}", item.name));
            }
            else
            {
                TargetUI.ShowChat();
                TargetUI.SetChat("它太远了，我捡不到。");
                Debug.LogError(string.Format("无法拾取 {0}，因距离过远", item.name));
            }
        }
        /// <summary>
        /// 调查物品
        /// </summary>
        /// <param name="item"></param>
        public void Survey(Item item)
        {
            if (!item)
            {
                Debug.LogError("没有物品可调查");
                return;
            }
            TargetUI.ShowChat();
            if (IsNearby(item))
            {
                TargetUI.SetChat(item.StaticData.Describe);
            }
            else
            {
                TargetUI.SetChat("我需要靠近点才能仔细观察。");
                Debug.LogError(string.Format("无法调查 {0}，因距离过远", item.name));
            }
        }
        /// <summary>
        /// 显示细节
        /// </summary>
        /// <param name="item"></param>
        public void ShowDetail(Item item)
        {
            if (!item)
            {
                Debug.LogError("没有物品可显示细节");
                return;
            }
            UIManager.Singleton.UIItemDetail.SetData(item);
            if (!UIManager.Singleton.UIItemDetail.IsShow)
            {
                UIManager.Singleton.UIItemDetail.Show();
            }

            Debug.Log(item.StaticData.Name);
        }
        /// <summary>
        /// 丢弃物品
        /// </summary>
        /// <param name="item"></param>
        public void Discard(Item item)
        {
            if (!item)
            {
                Debug.LogError("没有物品可丢弃");
                return;
            }
            if (IsCarrying(item))
            {
                item.transform.position = transform.position;
                item.gameObject.SetActive(true);

                if (GetRightHandItem() == item)
                {
                    RightHandItemController.Item.gameObject.SetActive(true);
                    RightHandItemController.Item.SetPosition(false);
                    RightHandItemController.RemoveItem();
                }

                Items.Remove(item);

                OnPlayerItemChange.Invoke();
                Debug.Log(string.Format("已丢弃 {0}", item.name));
            }
            else
            {
                Debug.LogError(string.Format("无法丢弃 {0}，因未携带该物品", item.name));
            }
        }
        /// <summary>
        /// 使用物品
        /// </summary>
        /// <param name="item"></param>
        public void Use(Item item)
        {
            if (!item)
            {
                Debug.LogError("没有物品可使用");
                return;
            }
            if (IsCarrying(item))
            {
                switch (item.StaticData.Type)
                {
                    case ItemType.Food:
                        DynamicData.Skills.AddRange(item.StaticData.Skills);
                        DynamicData.Buffs.AddRange(item.StaticData.Buffs);
                        break;
                    case ItemType.Weapon:
                        break;
                    case ItemType.Book:
                        break;
                    case ItemType.Clothing:
                        Clothings.Add(item);
                        Items.Remove(item);
                        break;
                    case ItemType.Bag:
                        break;
                    case ItemType.Switch:
                        item.SwitchState();
                        break;
                    case ItemType.Other:
                        break;
                    default:
                        break;
                }
                Debug.Log("使用了" + item.name);
            }
            else
            {
                Debug.LogError(string.Format("无法使用 {0}，因未携带该物品", item.name));
            }
        }
        /// <summary>
        /// 投掷物品
        /// </summary>
        /// <param name="item"></param>
        public void Throw(Item item)
        {
            if (item)
            {
                Debug.Log("没有物品可投掷");
                return;
            }
            if (IsCarrying(item))
            {
                //item.Use();
            }
            else
            {
                Debug.LogError(string.Format("无法投掷 {0}，因未携带该物品", item.StaticData.Name));
            }
        }
        /// <summary>
        /// 拿出上一个物品
        /// </summary>
        public void TakeOutLastItem()
        {
            Item item = GetRightHandItem();

            if (Items.Count == 0)
            {
                Debug.LogError("身上没有物品");
                return;
            }
            else if (Items.Count == 1 && item)
            {
                Debug.LogError("身上只有一个物品，无需切换");
                return;
            }

            int last = 0;
            if (item)
            {
                last = GetIndex(item) - 1;
                if (last < 0) last = Items.Count - 1;
            }

            PutRightHandItemInBag();
            PutItemInRightHand(Items[last]);

            OnPlayerItemChange.Invoke();
        }
        /// <summary>
        /// 拿出下一个物品
        /// </summary>
        public void TakeOutNextItem()
        {
            Item item = GetRightHandItem();

            if (Items.Count == 0)
            {
                Debug.LogError("身上没有物品");
                return;
            }
            else if (Items.Count == 1 && item)
            {
                Debug.LogError("身上只有一个物品，无需切换");
                return;
            }

            int next = 0;
            if (item)
            {
                next = GetIndex(item) + 1;
                if (next > Items.Count - 1) next = 0;
            }

            PutRightHandItemInBag();
            PutItemInRightHand(Items[next]);

            OnPlayerItemChange.Invoke();
        }

        /// <summary>
        /// 获取右手上的物品
        /// </summary>
        /// <returns></returns>
        public Item GetRightHandItem()
        {
            return RightHandItemController.Item;
        }
        /// <summary>
        /// 将物品放在右手
        /// </summary>
        /// <param name="item"></param>
        public void PutItemInRightHand(Item item)
        {
            RightHandItemController.SetItem(item, true);
            item.SetPosition(true);
            item.gameObject.SetActive(true);

            Debug.Log(item.StaticData.Name + " 放在手中");
        }
        /// <summary>
        /// 将右手物品放在背包
        /// </summary>
        /// <param name="item"></param>
        public void PutRightHandItemInBag()
        {
            if (!GetRightHandItem()) return;
            Debug.Log(RightHandItemController.Item.StaticData.Name + " 放入背包");

            RightHandItemController.Item.gameObject.SetActive(false);
            RightHandItemController.Item.SetPosition(false);
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
                if (target.StaticData.RandomStorys.Count > 0)
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
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

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
                currentSpeed = 0;
            }
        }
        private void CheckNearistItem()
        {
            if (NearbyItems.Count > 0)
            {
                //计算所有附近物品离自己的距离
                List<float> diss = new List<float>();
                for (int i = 0; i < NearbyItems.Count; i++)
                {
                    diss.Add(Vector2.Distance(NearbyItems[i].transform.position, transform.position));
                }
                //获取离自己最近的物品
                NearistItem = NearbyItems[Utility.IndexMin(diss)];
            }
            else
            {
                NearistItem = null;
            }
        }
        private void CheckNearistCharacter()
        {
            if (NearbyCharacters.Count > 0)
            {
                //计算所有附近角色离自己的距离
                List<float> diss = new List<float>();
                for (int i = 0; i < NearbyCharacters.Count; i++)
                {
                    diss.Add(Vector2.Distance(NearbyCharacters[i].transform.position, transform.position));
                }
                //获取离自己最近的角色
                NearistCharacter = NearbyCharacters[Utility.IndexMin(diss)];
            }
            else
            {
                NearistCharacter = null;
            }
        }
        private void CheckNearistEntity()
        {
            bool isChangeNearistEntity = false;

            //最近物品离自己的距离
            float ni = NearistItem ? Vector2.Distance(NearistItem.transform.position, transform.position) : -1;
            //最近角色离自己的距离
            float nc = NearistCharacter ? Vector2.Distance(NearistCharacter.transform.position, transform.position) : -1;
            if (ni > nc)
            {
                if (NearistItem)
                {
                    if (NearistEntity != NearistItem.gameObject)
                    {
                        NearistEntity = NearistItem.gameObject;
                        NearistItem.SpriteSorter.SetAlpha(0.5f);
                        isChangeNearistEntity = true;
                    }
                }
                else
                {
                    if (NearistEntity != null)
                    {
                        NearistEntity = null;
                        isChangeNearistEntity = true;
                    }
                }
            }
            else
            {
                if (NearistCharacter)
                {
                    if (NearistEntity != NearistCharacter.gameObject)
                    {
                        NearistEntity = NearistCharacter.gameObject;
                        NearistCharacter.SpriteSorter.SetAlpha(0.5f);
                        isChangeNearistEntity = true;
                    }
                }
                else
                {
                    if (NearistEntity != null)
                    {
                        NearistEntity = null;
                        isChangeNearistEntity = true;
                    }
                }
            }

            //如果改变了最近的实体
            if (isChangeNearistEntity)
            {
                CancelInvoke();
                TargetUI.HideChat();
                if (NearistEntity)
                {
                    if (NearistEntity.GetComponent<Item>())
                    {
                        Invoke("ShowItemTip", 2);
                    }
                    else if (NearistEntity.GetComponent<Character>())
                    {
                        Invoke("ShowCharacterTip", 2);
                    }
                    else
                    {
                    }
                }
                if (lastNearistEntity)
                {
                    if (lastNearistEntity.GetComponent<Item>())
                    {
                        lastNearistEntity.GetComponent<Item>().SpriteSorter.SetAlpha(1f);
                    }
                    else if (lastNearistEntity.GetComponent<Character>())
                    {
                        lastNearistEntity.GetComponent<Character>().SpriteSorter.SetAlpha(1f);
                    }
                    else
                    {
                    }
                }
                else
                {
                }
                lastNearistEntity = NearistEntity;
            }
        }
        private void ShowItemTip()
        {
            if (!TargetUI.IsShowChat())
            {
                TargetUI.SetChat("[E]拾取\n[F]调查");
                TargetUI.ShowChat();
            }
        }
        private void ShowCharacterTip()
        {
            if (!TargetUI.IsShowChat())
            {
                TargetUI.SetChat("[E]对话\n[F]调查");
                TargetUI.ShowChat();
            }
        }

        private void CheckKeyUp_E()
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                if (NearistEntity)
                {
                    if (NearistEntity.GetComponent<Item>())
                    {
                        PickUp(NearistEntity.GetComponent<Item>());
                    }
                    else if (NearistEntity.GetComponent<Character>())
                    {
                        ChatWith(NearistEntity.GetComponent<Character>());
                    }
                }
            }
        }
        private void CheckKeyUp_F()
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                if (NearistEntity)
                {
                    if (NearistEntity.GetComponent<Item>())
                    {
                        Survey(NearistEntity.GetComponent<Item>());
                    }
                    else if (NearistEntity.GetComponent<Character>())
                    {
                        Survey(NearistEntity.GetComponent<Character>());
                    }
                }
            }
        }
        private void CheckKeyUp_Q()
        {
            if (Input.GetKeyUp(KeyCode.Q))
            {
                Discard(GetRightHandItem());
            }
        }
        private void CheckKeyUp_F1()
        {

        }

        private void CheckMouseButtonDown_0()
        {
            if (IsPlayer)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Item item = GetRightHandItem();
                    if (item)
                    {
                        Use(item);
                    }
                    else
                    {
                        Attack();
                    }
                }
            }
        }
        private void CheckMouseButtonDown_1()
        {
            if (Input.GetMouseButtonDown(1))
            {
            }
        }
        private void CheckMouseButtonDown_2()
        {
            if (Input.GetMouseButtonDown(2))
            {
                if (GetRightHandItem())
                {
                    lastPutInBagItem = GetRightHandItem();
                    PutRightHandItemInBag();
                }
                else
                {
                    if (lastPutInBagItem) PutItemInRightHand(lastPutInBagItem);
                }
            }
        }
        private void CheckMouseScrollWheelWithoutAlt()
        {
            if (!Input.GetKey(KeyCode.LeftAlt))
            {
                float f = Input.GetAxis("Mouse ScrollWheel");
                if (f > 0.0001)
                {
                    if (GetRightHandItem())
                    {
                        TakeOutLastItem();
                        if (UIManager.Singleton.UIItemDetail.IsShow)
                        {
                            ShowDetail(GetRightHandItem());
                        }
                    }
                    else
                    {
                        if (lastPutInBagItem)
                            PutItemInRightHand(lastPutInBagItem);
                        else
                            TakeOutLastItem();
                    }
                }
                else if (f < -0.0001)
                {
                    if (GetRightHandItem())
                    {
                        TakeOutNextItem();
                        if (UIManager.Singleton.UIItemDetail.IsShow)
                        {
                            ShowDetail(GetRightHandItem());
                        }
                    }
                    else
                    {
                        if (lastPutInBagItem)
                            PutItemInRightHand(lastPutInBagItem);
                        else
                            TakeOutNextItem();
                    }
                }
            }
        }
        private void CheckMouseScrollWheelWithAlt()
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                float f = Input.GetAxis("Mouse ScrollWheel");
                CameraManager.ChangeOrthographicSize(f);
            }
        }

        private void CheckAIPathMaxSpeed()
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
        private void CheckPlayerFaceTo()
        {
            //朝向（角色面朝鼠标位置）
            Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            IsFaceRight = mousePositionInWorld.x < transform.position.x ? true : false;
        }
        private void CheckPlayerAnimation()
        {
            Animator.SetInteger("Speed", currentSpeed);
        }
        private void CheckNPCFaceTo()
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
        }
        private void CheckNPCAnimation()
        {
            Animator.SetInteger("Speed", Mathf.RoundToInt(AIPath.desiredVelocity.magnitude));
        }
    }

    public enum CharacterState
    {
        Idle = 0,
        Walk = 1,
        Run = 2,
        Rest = 3,
        Talk = 4,
        Dead = 5
    }
}