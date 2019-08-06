using UnityEngine;
using UnityEngine.UI;
using E.Utility;

namespace E.Tool
{
    public partial class UITarget : UIBase
    {
        public Slider healthSlider;
        public Text nameText;
        public Transform buffsPanel;
        public UIBuffSlot buffSlotPrefab;
        public Button tradeButton;
        public Button guildInviteButton;
        public Button partyInviteButton;

        void Update()
        {
            Player player = Player.Myself;
            if (player != null)
            {
            }
            else gameObject.SetActive(false);
        }
    }
}