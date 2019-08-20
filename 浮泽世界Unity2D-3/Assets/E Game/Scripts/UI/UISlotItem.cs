using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UISlotItem : UISlotBase
    {
        public Text txtName;
        public Image imgIcon;
        public GameObject panCapacity;
        public Image imgCapacity;
        public Toggle togFolder;

        [SerializeField, ReadOnly] private Item item;

        public void SetData(Item item)
        {
            this.item = item;
            UpdateData();
        }
        public override void UpdateData()
        {
            if (item.StaticData.Accommodatable)
            {
                togFolder.gameObject.SetActive(true);
                panCapacity.SetActive(true);
                imgCapacity.fillAmount = item.GetCapacityPercentage();
            }
            else
            {
                togFolder.gameObject.SetActive(false);
                panCapacity.SetActive(false);
            }
            imgIcon.sprite = item.StaticData.Icon;
            txtName.text = item.StaticData.Name;
        }
    }
}