using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UISlotEquipment : UISlotBase
    {
        public UIDragAndDropable dragAndDropable;
        public Image imgIcon;
        public Image imgHealth;

        [SerializeField, ReadOnly] private Item item;

        public void SetData(Item item)
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