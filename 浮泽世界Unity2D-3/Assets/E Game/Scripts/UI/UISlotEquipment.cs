using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UISlotEquipment : UISlotBase<Item>
    {
        public Image imgIcon;
        public Image imgHealth;

        [SerializeField, ReadOnly] private Item item;

        public override void SetData(Item item)
        {
            this.item = item;
            UpdateData();
        }
        public override void UpdateData()
        {
            imgIcon.sprite = item.StaticData.Icon;
            imgHealth.fillAmount = item.GetHealthPercentage();
        }
    }
}