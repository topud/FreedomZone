﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace E.Tool
{
    public class UIConfirmation : UIBase
    {
        public static UIConfirmation singleton;
        public Text messageText;
        public Button confirmButton;

        public UIConfirmation()
        {
            // assign singleton only once (to work with DontDestroyOnLoad when
            // using Zones / switching scenes)
            if (singleton == null) singleton = this;
        }

        public void Show(string message, UnityAction onConfirm)
        {
            messageText.text = message;
            confirmButton.onClick.SetListener(onConfirm);
            gameObject.SetActive(true);
        }
    }
}