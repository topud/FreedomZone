using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UISlotEquipment : UISlotBase<Item>
    {
        [SerializeField] private Text txtType;
        [SerializeField] private Image imgIcon;
        [SerializeField] private Image imgHealth;

        public override void SetData(Item data)
        {
            Data = data;
            UpdateData();
        }
        public override void UpdateData()
        {
            if (Data)
            {
                txtType.enabled = false;
                imgIcon.sprite = Data.StaticData.Icon;
                imgHealth.fillAmount = Data.DynamicData.Health.NowPercent;
            }
            else
            {
                txtType.enabled = true;
                imgIcon.sprite = null;
                imgHealth.fillAmount = 0;
            }
        }
    }
}