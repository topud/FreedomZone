using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Pathfinding;
using UnityEditor;

namespace E.Tool
{
    [RequireComponent(typeof(AIPath))]
    [RequireComponent(typeof(AIDestinationSetter))]
    public class Role : Entity<RoleStaticData, RoleDynamicData>
    {
        public static UnityEvent onPlayerItemChange = new UnityEvent();
        public static UnityEvent onPlayerInfoChange = new UnityEvent();
        public static UnityEvent onNearbyItemChange = new UnityEvent();

        public new Animator Animator
        { get => GetComponentInChildren<Animator>(true); }
        public new SpriteSorter SpriteSorter
        { get => GetComponentInChildren<SpriteSorter>(true); }
        public AIPath AIPath
        { get => GetComponent<AIPath>(); }
        public AIDestinationSetter AIDestinationSetter
        { get => GetComponent<AIDestinationSetter>(); }
        public EntityUI TargetUI
        { get => GetComponentInChildren<EntityUI>(true); }
        public InHandItemController RightHandItemController
        { get => GetComponentInChildren<InHandItemController>(true); }

        [Header("角色状态")]
        [SerializeField, ReadOnly] private RoleState state = RoleState.Idle;
        [SerializeField, ReadOnly] private InteractiveMode mode = InteractiveMode.Survey;
        [SerializeField, ReadOnly] private int currentSpeed = 0;
        [SerializeField, ReadOnly] private float runBeyondDistance = 5;
        [SerializeField, ReadOnly] private Item lastPutInBagItem;
        [SerializeField, ReadOnly] private Item nearistItem;
        [SerializeField, ReadOnly] private Role nearistRole;
        [SerializeField, ReadOnly] private GameObject nearistEntity;
        [SerializeField, ReadOnly] private GameObject lastNearistEntity;
        [SerializeField, ReadOnly] private List<Item> nearbyItems = new List<Item>();
        [SerializeField, ReadOnly] private List<Role> nearbyRoles = new List<Role>();
        [SerializeField, ReadOnly] private List<Item> items = new List<Item>();

        public RoleState State
        {
            get => state;
            private set => state = value;
        }
        public InteractiveMode Mode
        { 
            get => mode;
            private set => mode = value; 
        }
        public bool IsPlayer
        {
            get => GameManager.Character.Player == this;
            set 
            {
                if (IsPlayer)
                {
                    if (!value)
                    {
                        GameManager.Character.Player = null;
                    }
                }
                else
                {
                    if (value)
                    {
                        GameManager.Character.Player = this;
                    }
                }
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
                SpriteSorter.transform.localScale = new Vector3(value ? 1 : -1, 1, 1);
            }
        }
        public bool IsAlive
        {
            get
            {
                return DynamicData.bodyState.health.Now > 0;
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
        public Item NearistItem
        {
            get => nearistItem;
            private set => nearistItem = value;
        }
        public Role NearistRole
        {
            get => nearistRole;
            private set => nearistRole = value;
        }
        public GameObject NearistEntity
        {
            get => nearistEntity;
            private set => nearistEntity = value;
        }
        public List<Item> NearbyItems
        {
            get => nearbyItems;
            set => nearbyItems = value;
        }
        public List<Role> NearbyRoles
        {
            get => nearbyRoles;
            set => nearbyRoles = value;
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
        }
        protected override void Update()
        {
            base.Update();

            if (AIDestinationSetter.target)
            {
                CheckAIPathMaxSpeed();
            }
            if (State != RoleState.Dead && State != RoleState.Talk)
            {
                if (IsPlayer)
                {
                    if (GameManager.UI.IsShowAnyUIForInteraction)
                    {
                        RightHandItemController.SetIsLookAtCursor(false);
                    }
                    else
                    {
                        CheckNearistItem();
                        CheckNearistCharacter();
                        CheckNearistEntity();

                        CheckKeyUp_E();
                        CheckKeyUp_F();
                        CheckKeyUp_Q(); 
                        CheckKeyUp_Tab();
                        CheckMouseButtonDown_0();
                        CheckMouseButtonDown_1();
                        CheckMouseButtonDown_2();
                        CheckMouseScrollWheelWithAlt();
                        CheckMouseScrollWheelWithoutAlt();

                        CheckWatch();
                        CheckPlayerFaceTo();
                        CheckPlayerAnimation();

                        RightHandItemController.SetIsLookAtCursor(true);
                    }
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

            if (State != RoleState.Dead && State != RoleState.Talk)
            {
                if (IsPlayer)
                {
                    if (!GameManager.UI.IsShowAnyUIForInteraction)
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
        }
        protected override void Reset()
        {
            base.Reset();
        }

        public override void OnPointerEnter()
        {
            //Debug.Log("光标进入 " + name);
            SpriteSorter.SetAlpha(0.5f);
        }
        public override void OnPointerExit()
        {
            //Debug.Log("光标离开 " + name);
            SpriteSorter.SetAlpha(1f);
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

            DynamicData = new RoleDynamicData
            {
                nameID = new NameAndID(StaticData.name, IsAsset ? -1 : GameManager.Character.AvailableID),
                position = IsAsset ? new Vector2(0, 0) : new Vector2(transform.position.x, transform.position.y),
                
                physique = StaticData.physique,
                mentality = StaticData.mentality,
                sense = StaticData.sense,
                bodyState = StaticData.bodyState,
                //DynamicData.items = StaticData.Items; 
                rmb = StaticData.rmb,
                fzb = StaticData.fzb,
                //ItemInstanceIDs
                relationships = StaticData.relationships,
                skills = new List<Skill>(StaticData.skills),
            };

            Refresh();

            Undo.RecordObject(gameObject, "Refresh " + DynamicData.nameID.NameID);
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

            gameObject.layer = LayerMask.NameToLayer("Character");
            gameObject.tag = IsPlayer ? "Player" : "NPC";
            AIPath.enabled = false;
            AIDestinationSetter.enabled = false;

            TargetUI.SetName(DynamicData.nameID.name);
            TargetUI.HideAll();

            name = IsAsset ? DynamicData.nameID.name : DynamicData.nameID.NameID;
            transform.position = DynamicData.position;
            Rigidbody.mass = StaticData.weight;

            Items.Clear();
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
        }

        //行为
        /// <summary>
        /// 切换交互模式
        /// </summary>
        public void SwitchMode()
        {
            switch (Mode)
            {
                case InteractiveMode.Survey:
                    Mode = InteractiveMode.Fight;
                    GameManager.UI.UICharacterStatus.Show();
                    Debug.Log("切换至战斗模式");
                    break;
                case InteractiveMode.Fight:
                    Mode = InteractiveMode.Survey;
                    GameManager.UI.UICharacterStatus.Hide();
                    Debug.Log("切换至调查模式");
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 更改当前生命值上限
        /// </summary>
        public void ChangeHealthMax(int value)
        {
            DynamicData.bodyState.health.Max += value;
        }
        /// <summary>
        /// 更改当前生命值
        /// </summary>
        public void ChangeHealthNow(int value)
        {
            DynamicData.bodyState.health.Now += value;
        }
        /// <summary>
        /// 复活
        /// </summary>
        /// <param name="healthP"></param>
        public void Revive(float healthP = 1, float mindP = 1, float powerP = 1)
        {
            DynamicData.bodyState.health.Now = DynamicData.bodyState.health.Max * healthP;
            DynamicData.bodyState.mind.Now = DynamicData.bodyState.mind.Max * mindP;
            DynamicData.bodyState.power.Now = DynamicData.bodyState.power.Max * powerP;
        }
        /// <summary>
        /// 恢复
        /// </summary>
        public void Recover()
        {
            if (enabled && DynamicData.bodyState.health.now > 0)
            {
                DynamicData.bodyState.health.now += DynamicData.bodyState.health.autoAdd;
                DynamicData.bodyState.mind.now += DynamicData.bodyState.mind.autoAdd;
                DynamicData.bodyState.power.now += DynamicData.bodyState.power.autoAdd;
            }
        }
        /// <summary>
        /// 攻击
        /// </summary>
        public void Attack()
        {
            Animator.SetTrigger("Attack");
        }

        //物品
        /// <summary>
        /// 是否在视野内
        /// </summary>
        /// <returns></returns>
        public bool IsInView(Item target)
        {
            return Vector2.Distance(target.transform.position, transform.position) < 5;
        }
        /// <summary>
        /// 检测物品是否在拾取范围
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool IsNearby(Item target)
        {
            return NearbyItems.Contains(target);
        }
        /// <summary>
        /// 检测物品是否被携带
        /// </summary>
        /// <returns></returns>
        public bool IsHave(Item target)
        {
            return Items.Contains(target);
        }
        /// <summary>
        /// 检测物品是否在玩家手上
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool IsInHandOrBag(Item target)
        {
            Item item = GameManager.Character.Player.GetRightHandItem();
            return item && target && item == target;
        }
        /// <summary>
        /// 获取物品排序
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public int GetIndex(Item target)
        {
            return Items.IndexOf(target);
        }
        /// <summary>
        /// 拾取物品
        /// </summary>
        /// <param name="target"></param>
        public void PickUp(Item target, bool toHand = true)
        {
            if (IsNearby(target))
            {
                NearbyItems.Remove(target);

                target.gameObject.SetActive(false);
                Items.Add(target);

                if (toHand)
                {
                    PutItemInRightHand(target, true);
                }

                onPlayerItemChange.Invoke();
                onNearbyItemChange.Invoke();
                Debug.Log(string.Format("已拾取 {0}", target.name));
            }
            else
            {
                TargetUI.ShowChat();
                TargetUI.SetChat("它太远了，我捡不到。");
            }
        }
        /// <summary>
        /// 调查物品
        /// </summary>
        /// <param name="target"></param>
        public void Survey(Item target)
        {
            if (IsInView(target))
            {
                TargetUI.ShowChat();
                TargetUI.SetChat(target.StaticData.description);
            }
            else
            {
                TargetUI.ShowChat();
                TargetUI.SetChat("我需要靠近点才能仔细观察");
            }
        }
        /// <summary>
        /// 丢弃物品
        /// </summary>
        /// <param name="target"></param>
        public void Drop(Item target)
        {
            if (IsHave(target))
            {
                if (GetRightHandItem() && GetRightHandItem() == target)
                {
                    RightHandItemController.RemoveItem(true);
                    lastPutInBagItem = null;
                }
                else
                {
                    target.gameObject.SetActive(true);
                    target.SetCollider(false);
                }

                Items.Remove(target);
                target.transform.position = transform.position - new Vector3(0, 0.5f, 0);
                target.transform.localScale = new Vector3(1, 1, 1);
                target.DynamicData.hotKey = KeyCode.None;

                onPlayerItemChange.Invoke();
                onNearbyItemChange.Invoke();
                Debug.Log(string.Format("{0} 已丢弃 {1}", name, target.name));
            }
            else
            {
                Debug.LogWarning(string.Format("{0} 无法丢弃 {1}，因未携带该物品", name, target.name));
            }
        }
        /// <summary>
        /// 投掷物品
        /// </summary>
        /// <param name="target"></param>
        public void Throw(Item target)
        {
            if (target)
            {
                Debug.Log(string.Format("{0} 投掷物品不存在", name));
                return;
            }
            if (IsHave(target))
            {
                //item.Use();
            }
            else
            {
                Debug.LogError(string.Format("{0} 无法投掷未携带该物品 {1}", name, target.StaticData.name));
            }
        }
        /// <summary>
        /// 使用物品
        /// </summary>
        /// <param name="target"></param>
        public void Use(Item target)
        {
            if (IsHave(target))
            {
                target.Use(this);
            }
            else
            {
                if (IsNearby(target))
                {
                    target.Use(this);
                }
                else
                {
                    TargetUI.ShowChat();
                    TargetUI.SetChat("那东西离我太远了，我无法操作它");
                }
            }
        }
        /// <summary>
        /// 移交物品
        /// </summary>
        /// <param name="targetItem"></param>
        /// <param name="targetCharacter"></param>
        public void GiveTo(Role target)
        {
            Item item = GetRightHandItem();
            if (item)
            {
                if (IsNearby(target))
                {
                    Items.Remove(item);
                    item.DynamicData.hotKey = KeyCode.None;
                    RightHandItemController.RemoveItem(true);

                    target.Items.Add(item);
                    target.PutItemInRightHand(item, target.IsPlayer);

                    onPlayerItemChange.Invoke();
                    Debug.Log(string.Format("{0} 把 {1} 给了 {2}", name, item.name, target.name));
                }
                else
                {
                    TargetUI.ShowChat();
                    TargetUI.SetChat("我需要靠近点才能才能把手上的东西给他");
                }
            }
            else
            {
                Debug.LogWarning(string.Format("{0} 手中没有物品可移交", name));
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
                Debug.LogWarning(string.Format("{0} 身上没有物品", name));
                return;
            }
            else if (Items.Count == 1 && item)
            {
                Debug.LogWarning(string.Format("{0} 身上只有一个物品，无需切换", name));
                return;
            }

            int last = 0;
            if (item)
            {
                last = GetIndex(item) - 1;
                if (last < 0) last = Items.Count - 1;
            }

            PutItemInRightHand(Items[last], true);
        }
        /// <summary>
        /// 拿出下一个物品
        /// </summary>
        public void TakeOutNextItem()
        {
            Item item = GetRightHandItem();

            if (Items.Count == 0)
            {
                Debug.LogWarning(string.Format("{0} 身上没有物品", name));
                return;
            }
            else if (Items.Count == 1 && item)
            {
                Debug.LogWarning(string.Format("{0} 身上只有一个物品，无需切换", name));
                return;
            }

            int next = 0;
            if (item)
            {
                next = GetIndex(item) + 1;
                if (next > Items.Count - 1) next = 0;
            }

            PutItemInRightHand(Items[next], true);
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
        /// <param name="target"></param>
        public void PutItemInRightHand(Item target, bool isLookAtCursor)
        {
            PutRightHandItemInBag();
            RightHandItemController.SetItem(target, isLookAtCursor);

            Debug.Log(string.Format("{0} 把 {1} 放在手中", name, target.StaticData.name));
        }
        /// <summary>
        /// 将右手物品放在背包
        /// </summary>
        /// <param name="item"></param>
        public void PutRightHandItemInBag()
        {
            if (!GetRightHandItem()) return;

            Item item = RightHandItemController.Item;
            RightHandItemController.RemoveItem(false);

            Debug.Log(string.Format("{0} 把 {1} 放入背包", name, item.StaticData.name));
        }
        /// <summary>
        /// 切换物品位置
        /// </summary>
        /// <param name="target"></param>
        public void SwitchItemToHandOrBag(Item target)
        {
            if (IsInHandOrBag(target))
            {
                PutRightHandItemInBag();
            }
            else
            {
                PutItemInRightHand(target, true);
            }
        }

        //角色
        /// <summary>
        /// 是否在视野内
        /// </summary>
        /// <returns></returns>
        public bool IsInView(Role target)
        {
            return Vector2.Distance(target.transform.position, transform.position) < 7;
        }
        /// <summary>
        /// 检测角色是否在对话范围
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsNearby(Role target)
        {
            return NearbyRoles.Contains(target);
        }
        /// <summary>
        /// 与角色对话
        /// </summary>
        /// <param name="target"></param>
        public void ChatWith(Role target)
        {
            if (IsNearby(target))
            {
                //if (target.StaticData.ra.Count > 0)
                //{
                //    TargetUI.HideChat();
                //    target.TargetUI.HideName();
                //    target.TargetUI.ShowChat();
                //    string content = target.StaticData.RandomStorys[new System.Random().Next(0, target.StaticData.RandomStorys.Count)];
                //    target.TargetUI.SetChat(content);
                //    Debug.Log(string.Format("正在与 {0} 对话", target.StaticData.name));
                //}
                //else
                //{
                //    string str = string.Format("{0}好像没有什么想说的", target.StaticData.name);
                //    TargetUI.ShowChat();
                //    TargetUI.SetChat(str);
                //}
            }
            else
            {
                string str = string.Format("我需要靠近点才能和{0}对话", target.StaticData.name);
                TargetUI.ShowChat();
                TargetUI.SetChat(str);
            }
        }
        /// <summary>
        /// 调查角色
        /// </summary>
        /// <param name="item"></param>
        public void Survey(Role target)
        {
            if (IsInView(target))
            {
                //target.TargetUI.HideAll();
                TargetUI.ShowChat();
                TargetUI.SetChat(target.StaticData.description);
            }
            else
            {
                TargetUI.ShowChat();
                TargetUI.SetChat("我需要靠近点才能认出是谁。");
            }
        }

        //检测
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
                    currentSpeed = DynamicData.physique.speed;
                }
                else
                {
                    currentSpeed = 2;
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
            if (NearbyRoles.Count > 0)
            {
                //计算所有附近角色离自己的距离
                List<float> diss = new List<float>();
                for (int i = 0; i < NearbyRoles.Count; i++)
                {
                    diss.Add(Vector2.Distance(NearbyRoles[i].transform.position, transform.position));
                }
                //获取离自己最近的角色
                NearistRole = NearbyRoles[Utility.IndexMin(diss)];
            }
            else
            {
                NearistRole = null;
            }
        }
        private void CheckNearistEntity()
        {
            bool isChangeNearistEntity = false;

            //最近物品离自己的距离
            float ni = NearistItem ? Vector2.Distance(NearistItem.transform.position, transform.position) : -1;
            //最近角色离自己的距离
            float nc = NearistRole ? Vector2.Distance(NearistRole.transform.position, transform.position) : -1;
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
                if (NearistRole)
                {
                    if (NearistEntity != NearistRole.gameObject)
                    {
                        NearistEntity = NearistRole.gameObject;
                        NearistRole.SpriteSorter.SetAlpha(0.5f);
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
                //CancelInvoke();
                //TargetUI.HideChat();
                if (NearistEntity)
                {
                    //if (NearistEntity.GetComponent<Item>())
                    //{
                    //    Invoke("ShowItemTip", 0);
                    //}
                    //else if (NearistEntity.GetComponent<Character>())
                    //{
                    //    Invoke("ShowCharacterTip", 0);
                    //}
                    //else
                    //{
                    //}
                }
                if (lastNearistEntity)
                {
                    if (lastNearistEntity.GetComponent<Item>())
                    {
                        lastNearistEntity.GetComponent<Item>().SpriteSorter.SetAlpha(1f);
                    }
                    else if (lastNearistEntity.GetComponent<Role>())
                    {
                        lastNearistEntity.GetComponent<Role>().SpriteSorter.SetAlpha(1f);
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
                    else if (NearistEntity.GetComponent<Role>())
                    {
                        ChatWith(NearistEntity.GetComponent<Role>());
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
                    else if (NearistEntity.GetComponent<Role>())
                    {
                        Survey(NearistEntity.GetComponent<Role>());
                    }
                }
            }
        }
        private void CheckKeyUp_Q()
        {
            if (Input.GetKeyUp(KeyCode.Q))
            {
                Item handItem = GetRightHandItem();
                bool isGetRightHandItem = handItem;
                bool isPointerOverItem = GameManager.Event.EventSystem.IsPointerOverGameObject();
                GameObject go;
                Item targetItem = null;
                Role targetCharacter = null;
                if (isPointerOverItem)
                {
                    go = GameManager.Event.GetOverGameObject();
                    targetItem = go.GetComponent<Item>();
                    targetCharacter = go.GetComponent<Role>();
                }

                if (isGetRightHandItem)
                {
                    if (isPointerOverItem)
                    {
                        if (targetItem)
                        {
                            //Use(targetItem);
                        }
                        if (targetCharacter)
                        {
                            GiveTo(targetCharacter);
                        }
                    }
                    else
                    {
                        Drop(handItem);
                    }
                }
            }
        }
        private void CheckKeyUp_F1()
        {

        }
        private void CheckKeyUp_Tab()
        {
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                SwitchMode();
            }
        }

        private void CheckMouseButtonDown_0()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TargetUI.HideAll();

                switch (Mode)
                {
                    case InteractiveMode.Survey:
                        if (GameManager.Event.EventSystem.IsPointerOverGameObject())
                        {
                            GameObject go = GameManager.Event.GetOverGameObject();
                            Item item = go.GetComponent<Item>();
                            Role character = go.GetComponent<Role>();
                            if (item)
                            {
                                Survey(item);
                            }
                            if (character)
                            {
                                Survey(character);
                            }
                        }
                        break;
                    case InteractiveMode.Fight:
                        Attack();
                        break;
                    default:
                        break;
                }
            }
        }
        private void CheckMouseButtonDown_1()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Item handItem = GetRightHandItem();
                bool isGetRightHandItem = handItem;
                bool isPointerOverItem = GameManager.Event.EventSystem.IsPointerOverGameObject();
                GameObject go;
                Item targetItem = null;
                Role targetCharacter = null;
                if (isPointerOverItem)
                {
                    go = GameManager.Event.GetOverGameObject();
                    targetItem = go.GetComponent<Item>();
                    targetCharacter = go.GetComponent<Role>();
                }

                TargetUI.HideAll();

                if (isGetRightHandItem)
                {
                    if (isPointerOverItem)
                    {
                        if (targetItem)
                        {
                            Use(targetItem);
                        }
                        if (targetCharacter)
                        {
                            ChatWith(targetCharacter);
                        }
                    }
                    else
                    {
                        Use(handItem);
                    }
                }
                else
                {
                    if (isPointerOverItem)
                    {
                        if (targetItem)
                        {
                            Use(targetItem);
                        }
                        if (targetCharacter)
                        {
                            ChatWith(targetCharacter);
                        }
                    }
                }
            }
        }
        private void CheckMouseButtonDown_2()
        {
            if (Input.GetMouseButtonDown(2))
            {
                TargetUI.HideAll();

                if (GetRightHandItem())
                {
                    lastPutInBagItem = GetRightHandItem();
                    PutRightHandItemInBag();
                }
                else
                {
                    if (lastPutInBagItem) PutItemInRightHand(lastPutInBagItem, true);
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
                    }
                    else
                    {
                        if (lastPutInBagItem)
                            PutItemInRightHand(lastPutInBagItem, true);
                        else
                            TakeOutLastItem();
                    }
                }
                else if (f < -0.0001)
                {
                    if (GetRightHandItem())
                    {
                        TakeOutNextItem();
                    }
                    else
                    {
                        if (lastPutInBagItem)
                            PutItemInRightHand(lastPutInBagItem, true);
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
                GameManager.Camera.ChangeOrthographicSize(f);
            }
        }

        private void CheckAIPathMaxSpeed()
        {
            if (Vector2.Distance(transform.position, AIDestinationSetter.target.transform.position) > runBeyondDistance)
            {
                AIPath.maxSpeed = DynamicData.physique.speed;
            }
            else
            {
                AIPath.maxSpeed = 2;
            }
        }
        private void CheckWatch()
        {
            Vector3 dirM = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dirM.z = 0;
            Vector3 dirA = dirM - transform.position;
            Vector3 dirB = new Vector3(1, 0, 0);
            float angle = Vector3.Angle(dirB, dirA);

            float y = 0.5f;
            if (dirM.y > transform.position.y)
            {
                if (angle > 90)
                {
                    angle = 180 - angle;
                }
                y = 0.5f + angle / 180;
            }
            else
            {
                if (angle > 90)
                {
                    angle = 180 - angle;
                }
                y = 0.5f - angle / 180;
            }

            //Debug.Log(y);
            Animator.SetFloat("Y", y);
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

    [Serializable]
    public class RoleDynamicData : EntityDynamicData
    {
        [Header("角色实体动态数据")]
        [Tooltip("生理素质")] public PhysiqueInfo physique = new PhysiqueInfo();
        [Tooltip("心理素质")] public MentalityInfo mentality = new MentalityInfo();
        [Tooltip("感官素质")] public SenseInfo sense = new SenseInfo();
        [Tooltip("身体状态")] public BodyState bodyState = new BodyState();

        [Tooltip("携带的人民币")] public int rmb = 100;
        [Tooltip("携带的浮泽币")] public int fzb = 0;
        [Tooltip("携带的物品的引用")] public List<NameAndID> items = new List<NameAndID>();
        [Tooltip("当前人际关系")] public List<Relationship> relationships = new List<Relationship>();
        [Tooltip("掌握的技能")] public List<Skill> skills = new List<Skill>();
        [Tooltip("拥有的增益")] public List<Buff> buffs = new List<Buff>();
        [Tooltip("接受的任务")] public List<Quest> acceptedQuests = new List<Quest>();
    }

    public enum RoleState
    {
        Idle = 0,
        Walk = 1,
        Run = 2,
        Rest = 3,
        Talk = 4,
        Dead = 5
    }
    public enum InteractiveMode
    {
        Survey = 0,
        Fight = 1
    }
}