using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace E.Tool
{
    public class UIGameMenu : UIBase
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
                Hide();
                GameManager.UI.UISave.Show();
                GameManager.UI.UISave.OpenMode = OpenMode.Both;
            });
            btnSetting.onClick.AddListener(() => {
                Hide();
                GameManager.UI.UISetting.Show();
            });
            btnHelp.onClick.AddListener(() => {
                Hide();
                GameManager.UI.UIHelp.Show();
            });
            btnQuit.onClick.AddListener(() => { GameManager.BackToLobby(); });
        }
    }
}