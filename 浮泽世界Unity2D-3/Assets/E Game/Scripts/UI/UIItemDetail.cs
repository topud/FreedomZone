using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using E.Tool;

public class UIItemDetail : UIBase
{
    [Header("视图")]
    [SerializeField] private Text txtName;
    [SerializeField] private Text txtDes;
    [SerializeField] private Image imgIcon;
    [SerializeField] private Image imgHealth;
    [SerializeField] private Image imgPower;

    [SerializeField] private Button btnUse;
    [SerializeField] private Button btnPickupToHand;
    [SerializeField] private Button btnPickupToBag;
    [SerializeField] private Button btnMoveToHand;
    [SerializeField] private Button btnMoveToBag;
    [SerializeField] private Button btnDrop;

    [Header("数据")]
    public Item data;

    private void Update()
    {
        if (data)
        {
            RefreshFloatData();
        }
    }

    public void SetData(Item item)
    {
        data = item;
        Refresh();
    }
    public void Refresh()
    {
        if (data)
        {
            imgIcon.enabled = true;
            btnUse.gameObject.SetActive(true);
            if (GameManager.Character.Player.IsHave(data))
            {
                btnPickupToHand.gameObject.SetActive(false);
                btnPickupToBag.gameObject.SetActive(false);
                if (GameManager.Character.Player.IsInHandOrBag(data))
                {
                    btnMoveToHand.gameObject.SetActive(false);
                    btnMoveToBag.gameObject.SetActive(true);
                }
                else
                {
                    btnMoveToHand.gameObject.SetActive(true);
                    btnMoveToBag.gameObject.SetActive(false);
                }
                btnDrop.gameObject.SetActive(true);
            }
            else
            {
                btnPickupToHand.gameObject.SetActive(true);
                btnPickupToBag.gameObject.SetActive(true);
                btnMoveToHand.gameObject.SetActive(false);
                btnMoveToBag.gameObject.SetActive(false);
                btnDrop.gameObject.SetActive(false);
            }

            txtName.text = data.StaticData.Name;
            txtDes.text = data.StaticData.Describe;
            imgIcon.sprite = data.StaticData.Icon;
            RefreshFloatData();
            RefreshBtnUse();
        }
        else
        {
            imgIcon.enabled = false;
            btnUse.gameObject.SetActive(false);
            btnPickupToHand.gameObject.SetActive(false);
            btnPickupToBag.gameObject.SetActive(false);
            btnMoveToHand.gameObject.SetActive(false);
            btnMoveToBag.gameObject.SetActive(false);
            btnDrop.gameObject.SetActive(false);

            txtName.text = "";
            txtDes.text = "";
            imgIcon.sprite = null;
            imgHealth.fillAmount = 0;
            imgPower.fillAmount = 0;
        }
    }
    private void RefreshFloatData()
    {
        imgHealth.fillAmount = data.DynamicData.health.NowPercent;
        imgPower.fillAmount = data.DynamicData.power.NowPercent;
    }
    private void RefreshBtnUse()
    {
        string str = "使用";
        switch (data.StaticData.Type)
        {
            case ItemType.Food:
                break;
            case ItemType.Weapon:
                break;
            case ItemType.Book:
                break;
            case ItemType.Ammo:
                break;
            case ItemType.Bag:
                break;
            case ItemType.Switch:
                str = data.IsUsing ? "关闭" : "打开";
                break;
            case ItemType.Other:
                break;
            default:
                break;
        }
        btnUse.GetComponentInChildren<Text>().text = str;
    }

    public void Use()
    {
        GameManager.Character.Player.Use(data); 
        Refresh();
    }
    public void PickupToHand()
    {
        GameManager.Character.Player.PickUp(data);
        Refresh();
    }
    public void PickupToBag()
    {
        GameManager.Character.Player.PickUp(data, false);
        Refresh();
    }
    public void MoveToHand()
    {
        GameManager.Character.Player.PutItemInRightHand(data, true);
        Refresh();
    }
    public void MoveToBag()
    {
        GameManager.Character.Player.PutRightHandItemInBag();
        Refresh();
    }
    public void Drop()
    {
        GameManager.Character.Player.Drop(data);
        Refresh();
    }
}
