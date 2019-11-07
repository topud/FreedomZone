using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UILobbyMenu : UIBasePanel
    {
        [SerializeField] private Button btnNew;
        [SerializeField] private Button btnContinue;
        [SerializeField] private Button btnSave;
        [SerializeField] private Button btnSetting;
        [SerializeField] private Button btnHelp;
        [SerializeField] private Button btnQuit;

        private void Start()
        {
            Refresh();
        }

        public void Refresh()
        {
            btnContinue.gameObject.SetActive(SaveManager.GetLatestSaveFile() == null ? false : true);
            btnSave.gameObject.SetActive(SaveManager.GetSaveFiles().Count == 0 ? false : true);
        }


        public enum MenuState
        {
            NoSave,
            HasSave
        }
    }
}