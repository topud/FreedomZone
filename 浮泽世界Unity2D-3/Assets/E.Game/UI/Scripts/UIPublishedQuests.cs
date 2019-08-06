using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UIPublishedQuests : UIBase
    {
        public KeyCode hotKey = KeyCode.Q;
        public Transform content;
        public UIQuestSlot slotPrefab;

        public string expandPrefix = "[+] ";
        public string hidePrefix = "[-] ";

        [Obsolete]
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