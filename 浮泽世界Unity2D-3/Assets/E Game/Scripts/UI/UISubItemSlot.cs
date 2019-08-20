using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UISubItemSlot : UISlotBase
    {
        public Image imgIcon;
        public Text txtName;

        [SerializeField, ReadOnly] private Item item;

        public void SetData(Item item)
        {
            this.item = item;
            UpdateData();
        }
        public override void UpdateData()
        {
            imgIcon.sprite = item.StaticData.Icon;
            txtName.text = item.StaticData.Name;
        }
    }
}