using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

public class UIInventory : UIListBase<Item, UISlotItem>
{
    public override void LoadData()
    {
        if (Character.player)
        {
            Datas.AddRange(Character.player.Items);
        }
    }
}
