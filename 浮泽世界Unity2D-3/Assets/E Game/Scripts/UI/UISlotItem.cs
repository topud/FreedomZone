using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UISlotItem : UISlotBase<Item>
    {
        [Header("组件")]
        public Image imgIcon;
        public Image imgFrame;
        public Color clrDefault;
        public Color clrSelected;
        private UIInventory uiInventory;

        private void Awake()
        {
            uiInventory = GetComponentInParent<UIInventory>();
        }
        public override void SetData(Item data)
        {
            Data = data;
            UpdateData();
        }
        public override void UpdateData()
        {
            imgIcon.sprite = Data.StaticData.Icon;
            imgFrame.color = Character.Player.GetRightHandItem() == Data ? clrSelected : clrDefault;
        }

        public void OnClick()
        {
            Item item = Character.Player.GetRightHandItem();
            if (item)
            {
                if (item == Data)
                {
                    Character.Player.PutRightHandItemInBag();
                }
                else
                {
                    Character.Player.PutRightHandItemInBag();
                    Character.Player.PutItemInRightHand(Data);
                }
            }
            else
            {
                Character.Player.PutItemInRightHand(Data);
            }
            Character.OnPlayerItemChange.Invoke();
        }
        public override void OnBeginDrag()
        {
        }
        public override void OnDrag()
        {
        }
        public override void OnEndDrag()
        {
        }
        public override void OnDrop()
        {
        }
    }
}