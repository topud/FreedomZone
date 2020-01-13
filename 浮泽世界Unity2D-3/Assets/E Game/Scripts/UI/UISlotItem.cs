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

        [Header("数据")]
        [SerializeField] private KeyCode hotKey = KeyCode.None;

        public KeyCode HotKey 
        { 
            get => hotKey;
            set 
            {
                if (hotKey == value)
                {
                    hotKey = KeyCode.None;
                }
                else
                {
                    hotKey = value;
                }
                UpdateData();
            }
        }
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
            imgHotKey.enabled = HotKey != KeyCode.None;
            txtHotKey.enabled = HotKey != KeyCode.None;

            int key = -1;
            switch (HotKey)
            {
                case KeyCode.Keypad0:
                    key = 0;
                    break;
                case KeyCode.Keypad1:
                    key = 1;
                    break;
                case KeyCode.Keypad2:
                    key = 2;
                    break;
                case KeyCode.Keypad3:
                    key = 3;
                    break;
                case KeyCode.Keypad4:
                    key = 4;
                    break;
                case KeyCode.Keypad5:
                    key = 5;
                    break;
                case KeyCode.Keypad6:
                    key = 6;
                    break;
                case KeyCode.Keypad7:
                    key = 7;
                    break;
                case KeyCode.Keypad8:
                    key = 8;
                    break;
                case KeyCode.Keypad9:
                    key = 9;
                    break;
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
            UIInventory.selectedSlot = this;
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
        private void CheckHotKey()
        {
            if (Input.GetKeyUp(HotKey))
            {

            }
        }
    }
}