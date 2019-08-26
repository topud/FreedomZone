using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UILobbyMenu : UIBase
    {
        [SerializeField] private Button btnNew;
        [SerializeField] private Button btnContinue;
        [SerializeField] private Button btnSave;
        [SerializeField] private Button btnSetting;
        [SerializeField] private Button btnHelp;
        [SerializeField] private Button btnQuit;

        private void Start()
        {
            btnContinue.gameObject.SetActive(SaveManager.GetLatestSaveFile() == null ? false : true);
        }
        public enum MenuState
        {
            NoSave,
            HasSave
        }
    }
}