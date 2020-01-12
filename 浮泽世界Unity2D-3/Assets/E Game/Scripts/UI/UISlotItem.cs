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
        [SerializeField] private Image imgHand;
        [SerializeField] private Color clrDefault;
        [SerializeField] private Color clrSelected;

        private UIInventory UIInventory { get => GetComponentInParent<UIInventory>(); }
        private UIItemDetail UIItemDetail { get => UIManager.Singleton.UIItemDetail; }

        private void Update()
        {
            if (Data)
            {
                UpdateData();
            }
        }

        public override void SetData(Item data)
        {
            Data = data;
            UpdateData();
        }
        public override void UpdateData()
        {
            imgIcon.sprite = Data.StaticData.Icon;
            imgHealth.fillAmount = Data.DynamicData.health.NowPercent;
            imgPower.fillAmount = Data.DynamicData.power.NowPercent;
            imgHand.enabled = CharacterManager.Player.IsInHandOrBag(Data);
        }

        public void Show()
        {
            if (UIItemDetail.IsShow)
            {
                if (UIItemDetail.data == Data)
                {
                }
                else
                {
                    UIItemDetail.SetData(Data);
                }
            }
            else
            {
                UIItemDetail.SetData(Data);
                UIItemDetail.Show();
            }
        }
    }
}