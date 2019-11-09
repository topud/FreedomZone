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
            btnNew.onClick.AddListener(() => { GameManager.StartNewSave(); });
            btnContinue.onClick.AddListener(() => { GameManager.ContinueLastSave(); });
            btnSave.onClick.AddListener(() => {
                UIManager.Singleton.UISave.Show();
                UIManager.Singleton.UISave.OpenMode = OpenMode.Load;
            });
            btnSetting.onClick.AddListener(() => { UIManager.Singleton.UISetting.Show(); });
            btnHelp.onClick.AddListener(() => { UIManager.Singleton.UIHelp.Show(); });
            btnQuit.onClick.AddListener(() => { GameManager.QuitGame(); });

            btnContinue.gameObject.SetActive(SaveManager.GetLatestSaveFile() == null ? false : true);
            btnSave.gameObject.SetActive(SaveManager.GetSaveFiles().Count == 0 ? false : true);
        }
    }
}