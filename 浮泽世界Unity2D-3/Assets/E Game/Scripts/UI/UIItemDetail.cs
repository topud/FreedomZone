using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using E.Tool;

public class UIItemDetail : UIBase
{
    [SerializeField] private Text txtName;
    [SerializeField] private Text txtDes;
    [SerializeField] private Image imgIcon;

    [SerializeField] private Item Item;

    public void SetData(Item item)
    {
        Item = item;
        Show();
        Refresh();
    }
    public void Refresh()
    {
        if (Item)
        {
            txtName.text = Item.StaticData.Name;
            txtDes.text = Item.StaticData.Describe;
            imgIcon.sprite = Item.StaticData.Icon;

            imgIcon.enabled = true;
        }
        else
        {
            txtName.text = "";
            txtDes.text = "";
            imgIcon.sprite = null;

            imgIcon.enabled = false;
        }
    }
}
