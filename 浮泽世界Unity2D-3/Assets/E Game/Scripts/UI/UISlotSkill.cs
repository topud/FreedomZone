using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UISlotSkill : UISlot<Skill>
    {
        public Image image;
        public Button button;
        public GameObject cooldownOverlay;
        public Text cooldownText;
        public Image cooldownCircle;
        public Text descriptionText;
        public Button upgradeButton;

        public override void SetData(Skill data)
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateData()
        {
            throw new System.NotImplementedException();
        }
    }
}