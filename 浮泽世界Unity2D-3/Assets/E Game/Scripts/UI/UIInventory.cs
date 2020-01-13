using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

public class UIInventory : UIList<Item, UISlotItem>
{
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
        CheckKeyUp(KeyCode.Keypad0);
        CheckKeyUp(KeyCode.Keypad1);
        CheckKeyUp(KeyCode.Keypad2);
        CheckKeyUp(KeyCode.Keypad3);
        CheckKeyUp(KeyCode.Keypad4);
        CheckKeyUp(KeyCode.Keypad5);
        CheckKeyUp(KeyCode.Keypad6);
        CheckKeyUp(KeyCode.Keypad7);
        CheckKeyUp(KeyCode.Keypad8);
        CheckKeyUp(KeyCode.Keypad9);
    }
    protected override void OnDisable()
    {
        Character.onPlayerItemChange.RemoveListener(Refresh);
    }

    public override void LoadData()
    {
        if (CharacterManager.Player)
        {
            datas.AddRange(CharacterManager.Player.Items);
        }
    }
    private void CheckKeyUp(KeyCode key)
    {
        if (Input.GetKeyUp(key))
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
    }
}
