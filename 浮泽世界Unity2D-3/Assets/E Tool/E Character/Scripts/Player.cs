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
    }
    protected override void Start()
    {
        base.Start();

        AIPath.enabled = false;
    }
    protected override void Update()
    {
        base.Update();
        if (State != CharacterState.Dead && State != CharacterState.Talk)
        {
            CheckNearby();
            CheckAttack();
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

    private void CheckMove()
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
                currentSpeed = DynamicData.MaxSpeed;
            }
            else
            {
                currentSpeed = DynamicData.BaseSpeed;
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
    private void CheckNearby()
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
            foreach (Item it in NearbyItems)
            {
                if (nearistItem != it)
                {
                    it.TargetUI.HideHelp();
                }
            }
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

        float ni = nearistItem ? Vector2.Distance(nearistItem.transform.position, transform.position) : -1;
        float nc = nearistCharacter ? Vector2.Distance(nearistCharacter.transform.position, transform.position) : -1;
        if (ni > nc)
        {
            if (nearistItem)
            {
                if (!nearistItem.TargetUI.IsShowChat() && !TargetUI.IsShowChat())
                {
                    nearistItem.TargetUI.ShowHelp();
                }
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
            if (nearistItem)
            {
                nearistItem.TargetUI.HideHelp();
            }
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
    private void CheckAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }
}
