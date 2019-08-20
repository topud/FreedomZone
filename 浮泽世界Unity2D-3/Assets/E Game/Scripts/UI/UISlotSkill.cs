using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UISlotSkill : UISlotBase
    {
        public Image image;
        public Button button;
        public GameObject cooldownOverlay;
        public Text cooldownText;
        public Image cooldownCircle;
        public Text descriptionText;
        public Button upgradeButton;
        public override void UpdateData()
        {
            throw new System.NotImplementedException();
        }
    }
}