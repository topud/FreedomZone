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

        private void OnEnable()
        {
            if (!IsShow) Show();
        }
        private void Start()
        {
            btnNew.onClick.AddListener(() => { GameManager.StartNewSave(); });
            btnContinue.onClick.AddListener(() => { GameManager.ContinueLastSave(); });
            btnSave.onClick.AddListener(() => {
                Hide();
                GameManager.UI.UISave.Show();
                GameManager.UI.UISave.OpenMode = OpenMode.Load;
            });
            btnSetting.onClick.AddListener(() => {
                Hide();
                GameManager.UI.UISetting.Show();
            });
            btnHelp.onClick.AddListener(() => {
                Hide();
                GameManager.UI.UIHelp.Show();
            });
            btnQuit.onClick.AddListener(() => { GameManager.QuitGame(); });

            btnContinue.gameObject.SetActive(GameManager.Save.GetLatestSaveFile() == null ? false : true);
            btnSave.gameObject.SetActive(GameManager.Save.GetSaveFiles().Count == 0 ? false : true);
        }
    }
}