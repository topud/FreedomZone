using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UIManager : SingletonPattern<UIManager>
    {
        [Header("组件")]
        public CanvasGroup LobbyUI;
        public CanvasGroup LoadUI;
        public CanvasGroup GameUI;
        public UICharacter UICharacter;
        public UIMinimap UIMinimap;

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

            if (Input.GetKeyUp(UICharacter.HotKey))
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
            else if (Input.GetKeyUp(UICharacter.HotKeyInventory))
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
            else if (Input.GetKeyUp(UICharacter.HotKeyEquipment))
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
            else if (Input.GetKeyUp(UICharacter.HotKeySkill))
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
            else if (Input.GetKeyUp(UICharacter.HotKeyQuest))
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
                UICharacter.gameObject.SetActive(false);
            }
        }
        private void Reset()
        {
            LobbyUI = transform.Find("LobbyUI").GetComponent<CanvasGroup>();
            LoadUI = transform.Find("LoadUI").GetComponent<CanvasGroup>();
            GameUI = transform.Find("GameUI").GetComponent<CanvasGroup>();
            UICharacter = GetComponentInChildren<UICharacter>(true);
            UIMinimap = GetComponentInChildren<UIMinimap>(true);
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