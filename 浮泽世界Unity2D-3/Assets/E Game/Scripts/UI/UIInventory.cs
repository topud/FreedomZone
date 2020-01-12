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
    protected override void OnDisable()
    {
        Character.onPlayerItemChange.RemoveListener(Refresh);
    }

    public override void LoadData()
    {
        if (CharacterManager.Player)
        {
            Datas.AddRange(CharacterManager.Player.Items);
        }
    }
}
