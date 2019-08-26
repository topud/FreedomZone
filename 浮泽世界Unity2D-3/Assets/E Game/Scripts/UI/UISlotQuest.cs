using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UISlotQuest : UISlotBase<Quest>
    {
        //无Tooltip
        public Button nameButton;
        public Text descriptionText;

        public override void SetData(Quest data)
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateData()
        {
            throw new System.NotImplementedException();
        }
    }
}