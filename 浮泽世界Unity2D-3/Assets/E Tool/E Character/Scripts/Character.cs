using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;
using E.Tool;

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

    [Header("组件")]
    [ReadOnly, SerializeField] protected Animator Animator;
    [ReadOnly, SerializeField] protected AudioSource AudioSource;
    [ReadOnly, SerializeField] protected Rigidbody2D Rigidbody;
    [ReadOnly, SerializeField] protected Collider2D Collider;
    [ReadOnly, SerializeField] protected CharacterUI CharacterUI;
    [ReadOnly, SerializeField] protected CharacterPartController SortByDepth;
    [ReadOnly, SerializeField] protected AIPath AIPath;
    [ReadOnly, SerializeField] protected AIDestinationSetter AIDestinationSetter;


    protected virtual void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        AudioSource = GetComponent<AudioSource>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();
        CharacterUI = GetComponentInChildren<CharacterUI>();
        SortByDepth = GetComponentInChildren<CharacterPartController>();
        AIPath = GetComponent<AIPath>();
        AIDestinationSetter = GetComponent<AIDestinationSetter>();
    }
    protected virtual void OnEnable()
    {
        CharacterUI.HideName();
        CharacterUI.HideTalk();
        CharacterUI.SetName(StaticData.Name);
        Rigidbody.mass = StaticData.Weight;
        gameObject.name = StaticData.Name;
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
                AIPath.maxSpeed = DynamicData.Speed;
            }
            else
            {
                AIPath.maxSpeed = 2;
            }
        }
    }
    protected virtual void FixedUpdate()
    {
        
    }
    protected virtual void OnDestroy()
    {
        
    }
    private void OnMouseEnter()
    {
        SortByDepth.SetColor(new Color(0.8f, 0.8f, 0.8f));
        CharacterUI.ShowName();
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(1))
        {
            UIManager.Singleton.UICharacterInfo.Character = this;
        }
    }
    private void OnMouseDown()
    {
        SortByDepth.SetColor(new Color(0.6f, 0.6f, 0.6f));
    }
    private void OnMouseDrag()
    {

    }
    private void OnMouseUp()
    {
        SortByDepth.SetColor(new Color(0.8f, 0.8f, 0.8f));
    }
    private void OnMouseExit()
    {
        SortByDepth.SetColor(new Color(1, 1, 1));
        CharacterUI.HideName();
    }

    /// <summary>
    /// 生机百分比
    /// </summary>
    /// <returns></returns>
    public float HealthPercent()
    {
        return (DynamicData.MaxHealth != 0) ? (float)DynamicData.Health / DynamicData.MaxHealth : 0;
    }
    /// <summary>
    /// 脑力百分比
    /// </summary>
    /// <returns></returns>
    public float MindPercent()
    {
        return (DynamicData.MaxMind != 0) ? (float)DynamicData.Mind / DynamicData.MaxMind : 0;
    }
    /// <summary>
    /// 体力百分比
    /// </summary>
    /// <returns></returns>
    public float PowerPercent()
    {
        return (DynamicData.MaxPower != 0) ? (float)DynamicData.Power / DynamicData.MaxPower : 0;
    }
    /// <summary>
    /// 复活
    /// </summary>
    /// <param name="healthP"></param>
    public void Revive(float healthP = 1, float mindP = 0, float powerP = 0)
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
    /// 是否存活
    /// </summary>
    /// <returns></returns>
    public bool IsAlive()
    {
        return DynamicData.Health > 0;
    }

    public int GetInventoryIndexByName(string itemName)
    {
        return 0;// inventory.FindIndex(slot => slot.amount > 0 && slot.item.Name == itemName);
    }
    public bool InventoryRemove(Item item, int amount)
    {
        //for (int i = 0; i < inventory.Count; ++i)
        {
            //ItemSlot slot = inventory[i];
            //// note: .Equals because name AND dynamic variables matter (petLevel etc.)
            //if (slot.amount > 0 && slot.item.Equals(item))
            //{
            //    // take as many as possible
            //    amount -= slot.DecreaseAmount(amount);
            //    inventory[i] = slot;

            //    // are we done?
            //    if (amount == 0) return true;
            //}
        }
        return false;
    }
    public bool InventoryCanAdd(Item item, int amount)
    {
        //// go through each slot
        //for (int i = 0; i < inventory.Count; ++i)
        //{
        //    // empty? then subtract maxstack
        //    if (inventory[i].amount == 0)
        //        amount -= item.MaxStack;
        //    // not empty. same type too? then subtract free amount (max-amount)
        //    // note: .Equals because name AND dynamic variables matter (petLevel etc.)
        //    else if (inventory[i].item.Equals(item))
        //        amount -= (inventory[i].item.MaxStack - inventory[i].amount);

        //    // were we able to fit the whole amount already?
        //    if (amount <= 0) return true;
        //}

        return false;
    }
    public bool InventoryAdd(Item item, int amount)
    {
        // we only want to add them if there is enough space for all of them, so
        // let's double check
        //if (InventoryCanAdd(item, amount))
        //{
        //    // add to same item stacks first (if any)
        //    // (otherwise we add to first empty even if there is an existing
        //    //  stack afterwards)
        //    for (int i = 0; i < inventory.Count; ++i)
        //    {
        //        // not empty and same type? then add free amount (max-amount)
        //        // note: .Equals because name AND dynamic variables matter (petLevel etc.)
        //        if (inventory[i].amount > 0 && inventory[i].item.Equals(item))
        //        {
        //            ItemSlot temp = inventory[i];
        //            amount -= temp.IncreaseAmount(amount);
        //            inventory[i] = temp;
        //        }

        //        // were we able to fit the whole amount already? then stop loop
        //        if (amount <= 0) return true;
        //    }

        //    // add to empty slots (if any)
        //    for (int i = 0; i < inventory.Count; ++i)
        //    {
        //        // empty? then fill slot with as many as possible
        //        if (inventory[i].amount == 0)
        //        {
        //            int add = Mathf.Min(amount, item.MaxStack);
        //            inventory[i] = new ItemSlot(item, add);
        //            amount -= add;
        //        }

        //        // were we able to fit the whole amount already? then stop loop
        //        if (amount <= 0) return true;
        //    }
        //    // we should have been able to add all of them
        //    if (amount != 0) Debug.LogError("inventory add failed: " + item.Name + " " + amount);
        //}
        return false;
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