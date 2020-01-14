using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace E.Tool
{
    public class UISlotItem : UISlot<Item>
    {
        [Header("组件")]
        [SerializeField] private Image imgIcon;
        [SerializeField] private Image imgFrame;
        [SerializeField] private Image imgHealth;
        [SerializeField] private Image imgPower;
        [SerializeField] private Image imgHand;
        [SerializeField] private Image imgHotKey;
        [SerializeField] private Text txtHotKey;
        [SerializeField] private Color clrDefault;
        [SerializeField] private Color clrSelected;

        public KeyCode HotKey 
        { 
            get => Data.DynamicData.hotKey;
            set 
            {
                if (Data.DynamicData.hotKey == value)
                {
                    Data.DynamicData.hotKey = KeyCode.None;
                }
                else
                {
                    Data.DynamicData.hotKey = value;
                }
                UpdateData();
            }
        }
        private UIInventory UIInventory { get => UIManager.Singleton.UIInventory; }
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
            imgHotKey.enabled = HotKey != KeyCode.None;
            txtHotKey.enabled = HotKey != KeyCode.None;

            int key = -1;
            switch (HotKey)
            {
                case KeyCode.Alpha0:
                    key = 0;
                    break;
                case KeyCode.Alpha1:
                    key = 1;
                    break;
                case KeyCode.Alpha2:
                    key = 2;
                    break;
                case KeyCode.Alpha3:
                    key = 3;
                    break;
                case KeyCode.Alpha4:
                    key = 4;
                    break;
                case KeyCode.Alpha5:
                    key = 5;
                    break;
                case KeyCode.Alpha6:
                    key = 6;
                    break;
                case KeyCode.Alpha7:
                    key = 7;
                    break;
                case KeyCode.Alpha8:
                    key = 8;
                    break;
                case KeyCode.Alpha9:
                    key = 9;
                    break;
                default:
                    break;
            }
            txtHotKey.text = key.ToString();
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

            if (CharacterManager.Player.IsHave(Data))
            {
                UIInventory.selectedSlot = this;
            }
        }
    }
}