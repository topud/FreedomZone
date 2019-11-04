using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UISlotItem : UISlotBase<Item>
    {
        [Header("组件")]
        [SerializeField] private Image imgIcon;
        [SerializeField] private Image imgFrame;
        [SerializeField] private Image imgHealth;
        [SerializeField] private Color clrDefault;
        [SerializeField] private Color clrSelected;
        private UIInventory uiInventory;

        private void Awake()
        {
            uiInventory = GetComponentInParent<UIInventory>();
        }
        public void OnClickLeftMouse()
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
        public void OnClickRightMouse()
        {
            if (UIManager.Singleton.UIItemDetail.IsShow)
            {
                if (UIManager.Singleton.UIItemDetail.Item == Data)
                {
                    UIManager.Singleton.UIItemDetail.Hide();
                }
                else
                {
                    UIManager.Singleton.UIItemDetail.SetData(Data);
                }
            }
            else
            {
                UIManager.Singleton.UIItemDetail.SetData(Data);
                UIManager.Singleton.UIItemDetail.Show();
            }
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

        public override void SetData(Item data)
        {
            Data = data;
            UpdateData();
        }
        public override void UpdateData()
        {
            imgIcon.sprite = Data.StaticData.Icon;
            imgFrame.color = Character.Player.GetRightHandItem() == Data ? clrSelected : clrDefault;
            imgHealth.fillAmount = Data.DynamicData.Health.NowPercent;
        }

    }
}