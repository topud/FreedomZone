﻿// Attach to the prefab for easier component access by the UI Scripts.
// Otherwise we would need slot.GetChild(0).GetComponentInChildren<Text> etc.
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UIQuestSlot : UIBaseSlot
    {
        //无Tooltip
        public Button nameButton;
        public Text descriptionText;
        public override void UpdateData()
        {
            throw new System.NotImplementedException();
        }
    }
}