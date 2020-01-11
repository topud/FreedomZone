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

    [SerializeField] private Button btnMove;
    [SerializeField] private Button btnUse;

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
            btnMove.gameObject.SetActive(true);
            btnUse.gameObject.SetActive(true);

            txtName.text = data.StaticData.Name;
            txtDes.text = data.StaticData.Describe;
            imgIcon.sprite = data.StaticData.Icon;
            RefreshFloatData();
            RefreshBtnMove();
            RefreshBtnUse();
        }
        else
        {
            imgIcon.enabled = false;
            btnMove.gameObject.SetActive(false);
            btnUse.gameObject.SetActive(false);

            txtName.text = "";
            txtDes.text = "";
            imgIcon.sprite = null;
            imgHealth.fillAmount = 0;
            imgPower.fillAmount = 0;
        }
    }
    private void RefreshFloatData()
    {
        imgHealth.fillAmount = data.DynamicData.Health.NowPercent;
        imgPower.fillAmount = data.DynamicData.Power.NowPercent;
    }
    private void RefreshBtnMove()
    {
        btnMove.GetComponentInChildren<Text>().text = CharacterManager.Player.IsInHandOrBag(data) ? "放回背包" : "拿在手上";
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
            case ItemType.Clothing:
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

    public void Move()
    {
        CharacterManager.Player.SwitchItemPosition(data);
        RefreshBtnMove();
    }
    public void Use()
    {
        CharacterManager.Player.Use(data);
        RefreshBtnUse();
    }
}
