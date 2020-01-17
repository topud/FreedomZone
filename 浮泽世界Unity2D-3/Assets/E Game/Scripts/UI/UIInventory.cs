using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

public class UIInventory : UIList<Item, UISlotItem>
{
    private UIItemDetail UIItemDetail { get => GameManager.UI.UIItemDetail; }

    protected override void OnEnable()
    {
        Refresh();
        Character.onPlayerItemChange.AddListener(Refresh);
    }
    private void Update()
    {
        CheckKeyUp(KeyCode.Alpha0);
        CheckKeyUp(KeyCode.Alpha1);
        CheckKeyUp(KeyCode.Alpha2);
        CheckKeyUp(KeyCode.Alpha3);
        CheckKeyUp(KeyCode.Alpha4);
        CheckKeyUp(KeyCode.Alpha5);
        CheckKeyUp(KeyCode.Alpha6);
        CheckKeyUp(KeyCode.Alpha7);
        CheckKeyUp(KeyCode.Alpha8);
        CheckKeyUp(KeyCode.Alpha9);
    }
    protected override void OnDisable()
    {
        Character.onPlayerItemChange.RemoveListener(Refresh);
    }

    public override void LoadData()
    {
        if (GameManager.Character.Player)
        {
            datas.AddRange(GameManager.Character.Player.Items);
        }
    }
    private void CheckKeyUp(KeyCode key)
    {
        if (Input.GetKeyUp(key))
        {
            if (IsShow && UIItemDetail.IsShow)
            {
                selectedSlot.HotKey = key;
                foreach (UISlotItem item in Slots)
                {
                    if (item != selectedSlot && item.HotKey == key)
                    {
                        item.HotKey = KeyCode.None;
                        break;
                    }
                }
            }
            else
            {
                foreach (UISlotItem item in Slots)
                {
                    if (item.HotKey == key)
                    {
                        GameManager.Character.Player.PutItemInRightHand(item.Data, true);
                        return;
                    }
                }
                GameManager.Character.Player.PutRightHandItemInBag();
            }
        }
    }
}
