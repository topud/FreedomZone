using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

public class UIInventory : UIList<Item, UISlotItem>
{
    public override void LoadData()
    {
        if (CharacterManager.Player)
        {
            Datas.AddRange(CharacterManager.Player.Items);
        }
    }
}
