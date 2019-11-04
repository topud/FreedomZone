using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UIManager : SingletonClass<UIManager>
    {
        [Header("组件")]
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
            if (GameManager.IsInLobby)
            {
                UILobbyMenu.gameObject.SetActive(true);
                UICharacterStatus.gameObject.SetActive(false);
            }
            else
            {
                UILobbyMenu.gameObject.SetActive(false);
                UICharacterStatus.gameObject.SetActive(true);
            }
            UIGameMenu.gameObject.SetActive(false);
            UICharacterDetail.gameObject.SetActive(false);
            UIMinimap.gameObject.SetActive(false);

            UIListSave.gameObject.SetActive(false);
            UISetting.gameObject.SetActive(false);
            UIHelp.gameObject.SetActive(false);
            UILoading.gameObject.SetActive(false);
            UIPopup.gameObject.SetActive(false);
        }
        private void Update()
        {
            if (GameManager.IsInLobby)
            {
            }
            else
            {
                if (Input.GetKeyUp(KeyCode.I))
                {
                    UIInventory.Show();
                }
                else if (Input.GetKeyUp(KeyCode.O))
                {
                }
                else if (Input.GetKeyUp(KeyCode.P))
                {
                }
                else if (Input.GetKeyUp(KeyCode.K))
                {
                }
                else if (Input.GetKeyUp(KeyCode.L))
                {
                }
                else if (Input.GetKeyUp(KeyCode.Escape))
                {
                    if (UICharacterDetail.gameObject.activeInHierarchy)
                    {
                        UICharacterDetail.gameObject.SetActive(false);
                    }
                    if (UISetting.gameObject.activeInHierarchy)
                    {
                        UISetting.gameObject.SetActive(false);
                    }
                    if (UIListSave.gameObject.activeInHierarchy)
                    {
                        UIListSave.gameObject.SetActive(false);
                    }
                    UIGameMenu.gameObject.SetActive(!UIGameMenu.gameObject.activeInHierarchy);
                }
            }
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