using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UIManager : SingletonClass<UIManager>
    {
        [Header("视图")]
        public UILobbyMenu UILobbyMenu;
        [Space(5)]
        public UIGameMenu UIGameMenu;
        public UIInventory UIInventory;
        public UIItemDetail UIItemDetail;
        public UICharacterStatus UICharacterStatus;
        public UICharacterDetail UICharacterDetail;
        public UIMap UIMinimap;
        [Space(5)]
        public UIListSave UIListSave;
        public UISetting UISetting;
        public UIHelp UIHelp;
        public UILoading UILoading;
        public UIPopup UIPopup;

        //[Header("数据")]
        public static bool IsShowAnyUIPanel
        {
            get
            {
                return Singleton.UILobbyMenu.IsShow || Singleton.UIGameMenu.IsShow ||
                       Singleton.UIInventory.IsShow || Singleton.UIItemDetail.IsShow || Singleton.UICharacterStatus.IsShow || Singleton.UICharacterDetail.IsShow || Singleton.UIMinimap.IsShow ||
                       Singleton.UIListSave.IsShow || Singleton.UISetting.IsShow || Singleton.UIHelp.IsShow || Singleton.UILoading.IsShow || Singleton.UIPopup.IsShow;
            }
        }

        protected override void Awake()
        {
            base.Awake();
        }
        private void Start()
        {
            CheckUIType();
        }
        private void Update()
        {
            CheckUIType();

            CheckKeyUp_B();
            CheckKeyUp_I();
            CheckKeyUp_Esc();
        }
        private void Reset()
        {
            UICharacterDetail = GetComponentInChildren<UICharacterDetail>(true);
            UIMinimap = GetComponentInChildren<UIMap>(true);
            UIGameMenu = GetComponentInChildren<UIGameMenu>(true);
            UIListSave = GetComponentInChildren<UIListSave>(true);
            UISetting = GetComponentInChildren<UISetting>(true);
            UIPopup = GetComponentInChildren<UIPopup>(true);
            UILobbyMenu = GetComponentInChildren<UILobbyMenu>(true);
        }

        /// <summary>
        /// 设置光标状态
        /// </summary>
        /// <param name="isShow">是否展示</param>
        public static void SetCursor(bool isShow)
        {
            Cursor.visible = isShow;
            if (isShow)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private void CheckUIType()
        {
            if (GameManager.IsInLobby)
            {
                if (!UILobbyMenu.IsEnable) UILobbyMenu.IsEnable = true;
                if (UIGameMenu.IsEnable) UIGameMenu.IsEnable = false;
                if (UIItemDetail.IsEnable) UIItemDetail.IsEnable = false;
                if (UICharacterStatus.IsEnable) UICharacterStatus.IsEnable = false;
                if (UICharacterDetail.IsEnable) UICharacterDetail.IsEnable = false;
                if (UIMinimap.IsEnable) UIMinimap.IsEnable = false;

                if (!UILobbyMenu.IsShow) UILobbyMenu.Show();
            }
            else
            {
                if (UILobbyMenu.IsEnable) UILobbyMenu.IsEnable = false;
                if (UIGameMenu.IsEnable) UIGameMenu.IsEnable = false;
                if (!UIItemDetail.IsEnable) UIItemDetail.IsEnable = true;
                if (!UICharacterStatus.IsEnable) UICharacterStatus.IsEnable = true;
                if (!UICharacterDetail.IsEnable) UICharacterDetail.IsEnable = true;
                if (!UIMinimap.IsEnable) UIMinimap.IsEnable = true;
            }

            if (!UIListSave.IsEnable) UIListSave.IsEnable = true;
            if (!UISetting.IsEnable) UISetting.IsEnable = true;
            if (!UIHelp.IsEnable) UIHelp.IsEnable = true;
            if (!UILoading.IsEnable) UILoading.IsEnable = true;
            if (!UIPopup.IsEnable) UIPopup.IsEnable = true;
        }
        private void CheckKeyUp_I()
        {
            if (Input.GetKeyUp(KeyCode.I))
            {
                if (GameManager.IsInLobby)
                {
                }
                else
                {
                    if (UIItemDetail.IsShow)
                    {
                        UIItemDetail.Hide();
                    }
                    else
                    {
                        Character.Player.ShowDetail(Character.Player.GetRightHandItem());
                        UIItemDetail.Show();
                    }
                }
            }
        }
        private void CheckKeyUp_B()
        {
            if (Input.GetKeyUp(KeyCode.B))
            {
                if (GameManager.IsInLobby)
                {
                }
                else
                {
                    if (UIInventory.IsShow)
                    {
                        UIInventory.Hide();
                    }
                    else
                    {
                        UIItemDetail.Show();
                    }
                }
            }
        }
        private void CheckKeyUp_Esc()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (GameManager.IsInLobby)
                {
                }
                else
                {
                    if (UIGameMenu.IsShow) UIListSave.Hide();
                    else UIListSave.Show();
                }

                if (UICharacterDetail.IsShow) UICharacterDetail.Hide();
                if (UISetting.IsShow) UISetting.Hide();
                if (UIListSave.IsShow) UIListSave.Hide();
            }
        }


        /// <summary>
        /// 显示加载面板
        /// </summary>
        public void ShowLoading()
        {
            UILoading.gameObject.SetActive(true);
        }
        /// <summary>
        /// 隐藏加载面板
        /// </summary>
        public void HideLoading()
        {
            UILoading.gameObject.SetActive(false);
        }
        /// <summary>
        /// 显示当前菜单面板
        /// </summary>
        public void ShowMenu()
        {
            if (GameManager.IsInLobby)
            {
                UILobbyMenu.gameObject.SetActive(true);
            }
            else
            {
                UIGameMenu.gameObject.SetActive(true);
            }
        }
        /// <summary>
        /// 显示当前菜单面板
        /// </summary>
        public void HideMenu()
        {
            if (GameManager.IsInLobby)
            {
                UILobbyMenu.gameObject.SetActive(false);
            }
            else
            {
                UIGameMenu.gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// 显示大厅内菜单面板
        /// </summary>
        public void ShowLobbyMenu()
        {
            UILobbyMenu.gameObject.SetActive(true);
        }
        /// <summary>
        /// 隐藏大厅内菜单面板
        /// </summary>
        public void HideLobbyMenu()
        {
            UILobbyMenu.gameObject.SetActive(false);
        }
        /// <summary>
        /// 显示游戏内菜单面板
        /// </summary>
        public void ShowGameMenu()
        {
            UIGameMenu.gameObject.SetActive(true);
        }
        /// <summary>
        /// 隐藏游戏内菜单面板
        /// </summary>
        public void HideGameMenu()
        {
            UIGameMenu.gameObject.SetActive(false);
        }
        /// <summary>
        /// 显示设置面板
        /// </summary>
        public void ShowSetting()
        {
            UISetting.gameObject.SetActive(true);
        }
        /// <summary>
        /// 隐藏设置面板
        /// </summary>
        public void HideSetting()
        {
            UISetting.gameObject.SetActive(false);
        }
        /// <summary>
        /// 显示帮助面板
        /// </summary>
        public void ShowHelp()
        {
            UIHelp.gameObject.SetActive(true);
        }
        /// <summary>
        /// 隐藏帮助面板
        /// </summary>
        public void HideHelp()
        {
            UIHelp.gameObject.SetActive(false);
        }
        /// <summary>
        /// 显示存档面板
        /// </summary>
        public void ShowSave(int mode = 1)
        {
            UIListSave.OpenMode = (OpenMode)mode;
            UIListSave.gameObject.SetActive(true);
        }
        /// <summary>
        /// 隐藏存档面板
        /// </summary>
        public void HideSave()
        {
            UIListSave.gameObject.SetActive(false);
        }
    }
}