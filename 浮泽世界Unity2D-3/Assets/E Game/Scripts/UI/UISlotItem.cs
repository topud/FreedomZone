using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UISlotItem : UISlotBase<Item>
    {
        [Header("组件")]
        public Image imgIcon;
        //public Text txtName;
        public Text txtStack;
        //public GameObject panCapacity;
        //public Image imgCapacity;

        public override void SetData(Item data)
        {
            Data = data;
            UpdateData();
        }

        public override void UpdateData()
        {
            //if (Data.StaticData.Accommodatable)
            //{
            //    panCapacity.SetActive(true);
            //    imgCapacity.fillAmount = Data.GetCapacityPercentage();
            //}
            //else
            //{
            //    panCapacity.SetActive(false);
            //}
            imgIcon.sprite = Data.StaticData.Icon;
            //txtName.text = Data.StaticData.Name;
            int stack = Data.DynamicData.Stack;
            txtStack.text = stack == 1 ? "" : stack.ToString();
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
    }
}