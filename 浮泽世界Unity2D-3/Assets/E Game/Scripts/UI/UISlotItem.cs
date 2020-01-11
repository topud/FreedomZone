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

        public override void SetData(Item data)
        {

            Data = data;
            UpdateData();
        }
        public override void UpdateData()
        {
            imgIcon.sprite = Data.StaticData.Icon;
            imgHealth.fillAmount = Data.DynamicData.Health.NowPercent;
            imgPower.fillAmount = Data.DynamicData.Power.NowPercent;
            imgHand.enabled = CharacterManager.Player.IsInHandOrBag(Data);
        }

        public void Show()
        {
            if (UIManager.Singleton.UIItemDetail.IsShow)
            {
                if (UIManager.Singleton.UIItemDetail.data == Data)
                {
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
    }
}