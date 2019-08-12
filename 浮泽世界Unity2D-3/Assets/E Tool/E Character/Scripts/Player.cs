using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using E.Tool;

public class Player : Character
{
    public static Player Myself;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        Myself = this;
        AIPath.enabled = false;
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        if (State != CharacterState.Dead && State != CharacterState.Talk)
        {
            CheckPickUp();
            CheckTalk();
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
        Myself = null;
    }

    public void UsedItem(Item item)
    {
    }
    public void UseInventoryItem(int index)
    {
        //// validate
        //if (InventoryOperationsAllowed() &&
        //    0 <= index && index < inventory.Count && inventory[index].amount > 0 &&
        //    inventory[index].item.Data is UsableItem)
        //{
        //    // use item
        //    // note: we don't decrease amount / destroy in all cases because
        //    // some items may swap to other slots in .Use()
        //    UsableItem itemData = (UsableItem)inventory[index].item.Data;
        //    if (itemData.CanUse(this, index))
        //    {
        //        // .Use might clear the slot, so we backup the Item first for the Rpc
        //        Item item = inventory[index].item;
        //        itemData.Use(this, index);
        //        RpcUsedItem(item);
        //    }
        //}
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
            int currentSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = DynamicData.MaxSpeed;
            }
            else
            {
                currentSpeed = DynamicData.BaseSpeed;
            }
            Rigidbody.velocity = direction * currentSpeed;

            Animator.SetTrigger("Walk");

            // 绘制动线
            Debug.DrawLine(transform.position, transform.position + (Vector3)direction * currentSpeed, Color.green, 0, false);
        }
        else
        {
            Rigidbody.velocity = Vector2.zero;

            Animator.SetTrigger("Idle");
        }
    }
    private void CheckPickUp()
    {
        if (NearbyItems.Count == 0) return;

        List<float> diss = new List<float>();
        for (int i = 0; i < NearbyItems.Count; i++)
        {
            diss.Add(Vector2.Distance(NearbyItems[i].GetComponent<Transform>().position, transform.position));
        }
        Item item = NearbyItems[Utility.Min(diss)];
        foreach (Item it in NearbyItems)
        {
            it.TargetUI.SetNameColor(Color.white);
        }
        item.TargetUI.SetNameColor(Color.yellow);

        if (Input.GetKeyUp(KeyCode.F))
        {
            if (Vector2.Distance(item.GetComponent<Transform>().position, transform.position) <= 1.4)
            {
                item.gameObject.SetActive(false);
                DynamicData.Items.Add(item);
                //UIManager.Singleton.UIInventory.RefreshContent();
                Debug.Log(string.Format("已拾取 {0}", item.StaticData.Name));
            }
        }
    }
    private void CheckTalk()
    {
        if (NearbyCharacters.Count == 0) return;

        List<float> diss = new List<float>();
        for (int i = 0; i < NearbyCharacters.Count; i++)
        {
            diss.Add(Vector2.Distance(NearbyCharacters[i].GetComponent<Transform>().position, transform.position));
        }
        Character target = NearbyCharacters[Utility.Min(diss)];
        foreach (Character it in NearbyCharacters)
        {
            it.TargetUI.SetNameColor(Color.white);
        }
        target.TargetUI.SetNameColor(Color.yellow);

        if (Input.GetKeyUp(KeyCode.F))
        {
            if (Vector2.Distance(target.GetComponent<Transform>().position, transform.position) <= 1.4)
            {
                //target.gameObject.SetActive(false);
                //DynamicData.Inventory.Add(target);
                Debug.Log(string.Format("与 {0} 交谈", target.StaticData.Name));
            }
        }
    }
}
