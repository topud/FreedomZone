using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace E.Tool
{
    public class UIGameMenu : UIBasePanel
    {
        [SerializeField] private Button btnBack;
        [SerializeField] private Button btnSave;
        [SerializeField] private Button btnSetting;
        [SerializeField] private Button btnHelp;
        [SerializeField] private Button btnQuit;

        private void Start()
        {
            btnBack.onClick.AddListener(() => { Hide(); });
            btnSave.onClick.AddListener(() => {
                UIManager.Singleton.UISave.Show();
                UIManager.Singleton.UISave.OpenMode = OpenMode.Both;
            });
            btnSetting.onClick.AddListener(() => { UIManager.Singleton.UISetting.Show(); });
            btnHelp.onClick.AddListener(() => { UIManager.Singleton.UIHelp.Show(); });
            btnQuit.onClick.AddListener(() => { GameManager.BackToLobby(); });
        }
    }
}