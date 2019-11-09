using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using E.Tool;

public class UIItemDetail : UIBasePanel
{
    [Header("视图")]
    [SerializeField] private Text txtName;
    [SerializeField] private Text txtDes;
    [SerializeField] private Image imgIcon;
    [SerializeField] private Image imgHealth;
    [SerializeField] private Image imgPower;

    [Header("数据")]
    public Item Item;

    private void Update()
    {
        if (Item)
        {
            Refresh();
        }
    }

    public void SetData(Item item)
    {
        Item = item;
        Refresh();
    }
    public void Refresh()
    {
        if (Item)
        {
            txtName.text = Item.StaticData.Name;
            txtDes.text = Item.StaticData.Describe;
            imgIcon.sprite = Item.StaticData.Icon;
            imgHealth.fillAmount = Item.DynamicData.Health.NowPercent;
            imgPower.fillAmount = Item.DynamicData.Power.NowPercent;

            imgIcon.enabled = true;
        }
        else
        {
            txtName.text = "";
            txtDes.text = "";
            imgIcon.sprite = null;
            imgHealth.fillAmount = 0;
            imgPower.fillAmount = 0;

            imgIcon.enabled = false;
        }
    }
}
