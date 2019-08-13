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
        public UICharacter UICharacter;
        public UIMinimap UIMinimap;
        public UIGameMenu UIGameMenu;
        public UISave UISave;
        public UISetting UISetting;
        public UIPopup UIPopup;
        public UILobbyMenu UILobbyMenu;

        protected override void Awake()
        {
            base.Awake();
            UICharacter.gameObject.SetActive(false);
            UIMinimap.gameObject.SetActive(false);
        }
        private void Update()
        {
            switch (GameManager.Singleton.UIAndIOState)
            {
                case UIAndIOState.ShowUIAndUseIO:
                    break;
                case UIAndIOState.ShowUIAndUnuseIO:
                    break;
                case UIAndIOState.HideUIAndUseIO:
                    break;
                case UIAndIOState.HideUIAndUnuseIO:
                    break;
                default:
                    break;
            }

            if (Input.GetKeyUp(KeyCode.I))
            {
                if (UICharacter.gameObject.activeInHierarchy)
                {
                    if (!UICharacter.TogInfo.isOn)
                    {
                        UICharacter.TogInfo.isOn = true;
                    }
                    else
                    {
                        UICharacter.gameObject.SetActive(false);
                    }
                }
                else
                {
                    UICharacter.gameObject.SetActive(true);
                    UICharacter.TogInfo.isOn = true;
                }
            }
            else if (Input.GetKeyUp(KeyCode.O))
            {
                if (UICharacter.gameObject.activeInHierarchy)
                {
                    if (!UICharacter.TogInventory.isOn)
                    {
                        UICharacter.TogInventory.isOn = true;
                    }
                    else
                    {
                        UICharacter.gameObject.SetActive(false);
                    }
                }
                else
                {
                    UICharacter.gameObject.SetActive(true);
                    UICharacter.TogInventory.isOn = true;
                }
            }
            else if (Input.GetKeyUp(KeyCode.P))
            {
                if (UICharacter.gameObject.activeInHierarchy)
                {
                    if (!UICharacter.TogEquipment.isOn)
                    {
                        UICharacter.TogEquipment.isOn = true;
                    }
                    else
                    {
                        UICharacter.gameObject.SetActive(false);
                    }
                }
                else
                {
                    UICharacter.gameObject.SetActive(true);
                    UICharacter.TogEquipment.isOn = true;
                }
            }
            else if (Input.GetKeyUp(KeyCode.K))
            {
                if (UICharacter.gameObject.activeInHierarchy)
                {
                    if (!UICharacter.TogSkill.isOn)
                    {
                        UICharacter.TogSkill.isOn = true;
                    }
                    else
                    {
                        UICharacter.gameObject.SetActive(false);
                    }
                }
                else
                {
                    UICharacter.gameObject.SetActive(true);
                    UICharacter.TogSkill.isOn = true;
                }
            }
            else if (Input.GetKeyUp(KeyCode.L))
            {
                if (UICharacter.gameObject.activeInHierarchy)
                {
                    if (!UICharacter.TogQuest.isOn)
                    {
                        UICharacter.TogQuest.isOn = true;
                    }
                    else
                    {
                        UICharacter.gameObject.SetActive(false);
                    }
                }
                else
                {
                    UICharacter.gameObject.SetActive(true);
                    UICharacter.TogQuest.isOn = true;
                }
            }
            else if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (UICharacter.gameObject.activeInHierarchy)
                {
                    UICharacter.gameObject.SetActive(false);
                }
                UIGameMenu.gameObject.SetActive(!UIGameMenu.gameObject.activeInHierarchy);
            }
        }
        private void Reset()
        {
            LobbyUI = transform.Find("LobbyUI").gameObject;
            LoadUI = transform.Find("LoadUI").gameObject;
            GameUI = transform.Find("GameUI").gameObject;
            UICharacter = GetComponentInChildren<UICharacter>(true);
            UIMinimap = GetComponentInChildren<UIMinimap>(true);
            UIGameMenu = GetComponentInChildren<UIGameMenu>(true);
            UISave = GetComponentInChildren<UISave>(true);
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