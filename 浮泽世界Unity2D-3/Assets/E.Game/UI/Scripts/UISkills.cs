// Note: this script has to be on an always-active UI parent, so that we can
// always react to the hotkey.
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

namespace E.Tool
{
    public class UISkills : UIBase
    {
        public KeyCode hotKey = KeyCode.R;
        public UISkillSlot slotPrefab;
        public Transform content;
        public Text skillExperienceText;

        void Update()
        {
            Player player = Player.Myself;

        }
    }
}