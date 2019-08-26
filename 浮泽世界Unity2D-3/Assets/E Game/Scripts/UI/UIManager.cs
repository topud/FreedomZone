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
        public GameObject LoadUI;
        public GameObject GameUI;
        public GameObject PublicUI;
        [Space(5)]
        public UILobbyMenu UILobbyMenu;
        [Space(5)]
        public UIGameMenu UIGameMenu;
        public UICharacterStatus UICharacterStatus;
        public UICharacterDetail UICharacterDetail;
        public UIMinimap UIMinimap;
        [Space(5)]
        public UIPopup UIPopup;
        public UIListSave UISave;
        public UISetting UISetting;

        protected override void Awake()
        {
            base.Awake();
        }
        private void Start()
        {
            if (GameManager.Singleton.IsInLobby)
            {
                LobbyUI.SetActive(true);
                LoadUI.SetActive(false);
                GameUI.SetActive(false);
                PublicUI.SetActive(true);
            }
            else
            {
                LobbyUI.SetActive(false);
                LoadUI.SetActive(false);
                GameUI.SetActive(true);
                PublicUI.SetActive(true);
            }

            UICharacterDetail.gameObject.SetActive(false);
            UIMinimap.gameObject.SetActive(false);
        }
        private void Update()
        {
            if (GameManager.Singleton.IsInLobby)
            {
                if (!LobbyUI.gameObject.activeInHierarchy) LobbyUI.gameObject.SetActive(true);
                if (LoadUI.gameObject.activeInHierarchy) LoadUI.gameObject.SetActive(false);
                if (GameUI.gameObject.activeInHierarchy) GameUI.gameObject.SetActive(false);
                if (!PublicUI.gameObject.activeInHierarchy) PublicUI.gameObject.SetActive(true);
            }
            else
            {
                if (LobbyUI.gameObject.activeInHierarchy) LobbyUI.gameObject.SetActive(false);
                if (LoadUI.gameObject.activeInHierarchy) LoadUI.gameObject.SetActive(false);
                if (!GameUI.gameObject.activeInHierarchy)GameUI.gameObject.SetActive(true);
                if (!PublicUI.gameObject.activeInHierarchy) PublicUI.gameObject.SetActive(true);

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
            LoadUI = transform.Find("LoadUI").gameObject;
            GameUI = transform.Find("GameUI").gameObject;
            PublicUI = transform.Find("PublicUI").gameObject;
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
    }
}