using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

public class UINearby : UIList<Item, UISlotItem>
{
    protected override void OnEnable()
    {
        Refresh();
        Character.onNearbyItemChange.AddListener(Refresh);
    }
    protected override void OnDisable()
    {
        Character.onNearbyItemChange.RemoveListener(Refresh);
    }

    public override void LoadData()
    {
        if (GameManager.Character.Player)
        {
            datas.AddRange(GameManager.Character.Player.NearbyItems);
        }
    }
}
