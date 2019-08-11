using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    public class UIEquipment : UIBase
    {
        [Header("组件")]
        [SerializeField] private UIEquipmentSlot slotBag;
        [SerializeField] private UIEquipmentSlot slotCoat;
        [SerializeField] private UIEquipmentSlot slotPants;
        [SerializeField] private UIEquipmentSlot slotHat;
        [SerializeField] private UIEquipmentSlot slotMask;
        [SerializeField] private UIEquipmentSlot slotShoes;
        [SerializeField] private UIEquipmentSlot slotGloves;
        [SerializeField] private UIEquipmentSlot slotHandheld;

        [Header("数据")]
        public Character Character;


    }
}