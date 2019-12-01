using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace E.Tool
{
    public class UISlotItem : UISlotBase<Item>
    {
        [Header("组件")]
        [SerializeField] private Image imgIcon;
        [SerializeField] private Image imgFrame;
        [SerializeField] private Image imgHealth;
        [SerializeField] private Image imgPower;
        [SerializeField] private Color clrDefault;
        [SerializeField] private Color clrSelected;
        private UIInventory uiInventory;

        private void Awake()
        {
            uiInventory = GetComponentInParent<UIInventory>();
        }
        private void Update()
        {
            if (Data)
            {
                UpdateData();
            }
        }
        public void OnClickLeftMouse()
        {
            if (UIManager.Singleton.UIItemDetail.IsShow)
            {
                if (UIManager.Singleton.UIItemDetail.Item == Data)
                {
                    //UIManager.Singleton.UIItemDetail.Hide();
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
        public void OnClickMiddleMouse()
        {
            Item item = Character.player.GetRightHandItem();
            if (item)
            {
                if (item == Data)
                {
                    Character.player.PutRightHandItemInBag();
                }
                else
                {
                    Character.player.PutRightHandItemInBag();
                    Character.player.PutItemInRightHand(Data);
                }
            }
            else
            {
                Character.player.PutItemInRightHand(Data);
            }
            Character.onPlayerItemChange.Invoke();
        }
        public void OnClickRightMouse()
        {
            Character.player.Use(Data);
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
            imgFrame.color = Character.player.GetRightHandItem() == Data ? clrSelected : clrDefault;
            imgPower.fillAmount = Data.DynamicData.Power.NowPercent;
            imgHealth.fillAmount = Data.DynamicData.Health.NowPercent;
        }

    }
}