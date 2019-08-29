using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UIManager : SingletonPattern<UIManager>
    {
        [Header("组件")]
        public GameObject LobbyUI;
        public GameObject GameUI;
        [Space(5)]
        public UILobbyMenu UILobbyMenu;
        [Space(5)]
        public UIGameMenu UIGameMenu;
        public UICharacterStatus UICharacterStatus;
        public UICharacterDetail UICharacterDetail;
        public UIMinimap UIMinimap;
        [Space(5)]
        public UIListSave UISave;
        public UISetting UISetting;
        public UIHelp UIHelp;
        public UILoading UILoading;
        public UIPopup UIPopup;

        protected override void Awake()
        {
            base.Awake();
        }
        private void Start()
        {
            if (GameManager.Singleton.IsInLobby)
            {
                LobbyUI.SetActive(true);
                GameUI.SetActive(false);
                UILobbyMenu.gameObject.SetActive(true);
            }
            else
            {
                LobbyUI.SetActive(false);
                GameUI.SetActive(true);
                UILobbyMenu.gameObject.SetActive(false);
            }
            UIGameMenu.gameObject.SetActive(false);
            UICharacterDetail.gameObject.SetActive(false);
            UIMinimap.gameObject.SetActive(false);
            UISave.gameObject.SetActive(false);
            UISetting.gameObject.SetActive(false);
            UIHelp.gameObject.SetActive(false);
            UIPopup.gameObject.SetActive(false);
        }
        private void Update()
        {
            if (GameManager.Singleton.IsInLobby)
            {
                if (!LobbyUI.gameObject.activeInHierarchy) LobbyUI.gameObject.SetActive(true);
                if (GameUI.gameObject.activeInHierarchy) GameUI.gameObject.SetActive(false);
            }
            else
            {
                if (LobbyUI.gameObject.activeInHierarchy) LobbyUI.gameObject.SetActive(false);
                if (!GameUI.gameObject.activeInHierarchy)GameUI.gameObject.SetActive(true);

                if (Input.GetKeyUp(KeyCode.I))
                {
                    if (UICharacterDetail.gameObject.activeInHierarchy)
                    {
                        if (!UICharacterDetail.TogInfo.isOn)
                        {
                            UICharacterDetail.TogInfo.isOn = true;

                            UICharacterDetail.TogGroup.NotifyToggleOn(UICharacterDetail.TogInfo);
                        }
                        else
                        {
                            UICharacterDetail.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        UICharacterDetail.gameObject.SetActive(true);
                        UICharacterDetail.TogInfo.isOn = true;
                    }
                }
                else if (Input.GetKeyUp(KeyCode.O))
                {
                    if (UICharacterDetail.gameObject.activeInHierarchy)
                    {
                        if (!UICharacterDetail.TogInventory.isOn)
                        {
                            UICharacterDetail.TogInventory.isOn = true;
                        }
                        else
                        {
                            UICharacterDetail.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        UICharacterDetail.gameObject.SetActive(true);
                        UICharacterDetail.TogInventory.isOn = true;
                    }
                }
                else if (Input.GetKeyUp(KeyCode.P))
                {
                    if (UICharacterDetail.gameObject.activeInHierarchy)
                    {
                        if (!UICharacterDetail.TogEquipment.isOn)
                        {
                            UICharacterDetail.TogEquipment.isOn = true;
                        }
                        else
                        {
                            UICharacterDetail.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        UICharacterDetail.gameObject.SetActive(true);
                        UICharacterDetail.TogEquipment.isOn = true;
                    }
                }
                else if (Input.GetKeyUp(KeyCode.K))
                {
                    if (UICharacterDetail.gameObject.activeInHierarchy)
                    {
                        if (!UICharacterDetail.TogSkill.isOn)
                        {
                            UICharacterDetail.TogSkill.isOn = true;
                        }
                        else
                        {
                            UICharacterDetail.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        UICharacterDetail.gameObject.SetActive(true);
                        UICharacterDetail.TogSkill.isOn = true;
                    }
                }
                else if (Input.GetKeyUp(KeyCode.L))
                {
                    if (UICharacterDetail.gameObject.activeInHierarchy)
                    {
                        if (!UICharacterDetail.TogQuest.isOn)
                        {
                            UICharacterDetail.TogQuest.isOn = true;
                        }
                        else
                        {
                            UICharacterDetail.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        UICharacterDetail.gameObject.SetActive(true);
                        UICharacterDetail.TogQuest.isOn = true;
                    }
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
                    if (UISave.gameObject.activeInHierarchy)
                    {
                        UISave.gameObject.SetActive(false);
                    }
                    UIGameMenu.gameObject.SetActive(!UIGameMenu.gameObject.activeInHierarchy);
                }
            }
        }
        private void Reset()
        {
            LobbyUI = transform.Find("LobbyUI").gameObject;
            GameUI = transform.Find("GameUI").gameObject;
            UICharacterDetail = GetComponentInChildren<UICharacterDetail>(true);
            UIMinimap = GetComponentInChildren<UIMinimap>(true);
            UIGameMenu = GetComponentInChildren<UIGameMenu>(true);
            UISave = GetComponentInChildren<UIListSave>(true);
            UISetting = GetComponentInChildren<UISetting>(true);
            UIPopup = GetComponentInChildren<UIPopup>(true);
            UILobbyMenu = GetComponentInChildren<UILobbyMenu>(true);
        }

        /// <summary>
        /// 设置光标状态
        /// </summary>
        /// <param name="isShow">是否展示</param>
        private void SetCursor(bool isShow)
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
            UISave.OpenMode = (OpenMode)mode;
            UISave.gameObject.SetActive(true);
        }
        /// <summary>
        /// 隐藏存档面板
        /// </summary>
        public void HideSave()
        {
            UISave.gameObject.SetActive(false);
        }
    }
}