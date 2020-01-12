using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UIManager : SingletonClass<UIManager>
    {
        public UILobbyMenu UILobbyMenu { get => GetComponentInChildren<UILobbyMenu>(true); }
        public UIGameMenu UIGameMenu { get => GetComponentInChildren<UIGameMenu>(true); }

        public UICharacterStatus UICharacterStatus { get => GetComponentInChildren<UICharacterStatus>(true); }
        public UIInventory UIInventory { get => GetComponentInChildren<UIInventory>(true); }
        public UIItemDetail UIItemDetail { get => GetComponentInChildren<UIItemDetail>(true); }
        public UINearby UINearby { get => GetComponentInChildren<UINearby>(true); }
        public UIHelp UIHelp { get => GetComponentInChildren<UIHelp>(true); }
        public UIListSave UISave { get => GetComponentInChildren<UIListSave>(true); }
        public UISetting UISetting { get => GetComponentInChildren<UISetting>(true); }
        public UILoading UILoading { get => GetComponentInChildren<UILoading>(true); }
        public UIPopup UIPopup { get => GetComponentInChildren<UIPopup>(true); }

        public static bool IsShowAnyUI
        {
            get
            {
                return IsShowAnyUIMenu || IsShowAnyUIInGame || IsShowAnyUIPublic;
            }
        }
        public static bool IsShowAnyUIMenu
        {
            get
            {
                return (Singleton.UILobbyMenu.IsEnable && Singleton.UILobbyMenu.IsShow) || 
                    (Singleton.UIGameMenu.IsEnable && Singleton.UIGameMenu.IsShow);
            }
        }
        public static bool IsShowAnyUIInGame
        {
            get
            {
                return (Singleton.UIInventory.IsEnable && Singleton.UIInventory.IsShow) ||
                    (Singleton.UIItemDetail.IsEnable && Singleton.UIItemDetail.IsShow) ||
                    //(Singleton.UICharacterStatus.IsEnable && Singleton.UICharacterStatus.IsShow) ||
                    (Singleton.UINearby.IsEnable && Singleton.UINearby.IsShow);
                    //(Singleton.UIHelp.IsEnable && Singleton.UIHelp.IsShow);
            }
        }
        public static bool IsShowAnyUIPublic
        {
            get
            {
                return (Singleton.UISave.IsEnable && Singleton.UISave.IsShow )||
                    (Singleton.UISetting.IsEnable && Singleton.UISetting.IsShow) ||
                    (Singleton.UILoading.IsEnable && Singleton.UILoading.IsShow) ||
                    (Singleton.UIPopup.IsEnable && Singleton.UIPopup.IsShow);
            }
        }

        protected override void Awake()
        {
            base.Awake();
        }
        private void Start()
        {
            CheckUIType();

            UIHelp.Show();
        }
        private void Update()
        {
            CheckUIType();

            CheckKeyUp_I();
            CheckKeyUp_Esc();
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

                if (UICharacterStatus.IsEnable) UICharacterStatus.IsEnable = false;
                if (UIInventory.IsEnable) UIInventory.IsEnable = false;
                if (UIItemDetail.IsEnable) UIItemDetail.IsEnable = false;
                if (UINearby.IsEnable) UINearby.IsEnable = false;

                //if (!UILobbyMenu.IsShow) UILobbyMenu.Show();
            }
            else
            {
                if (UILobbyMenu.IsEnable) UILobbyMenu.IsEnable = false;
                if (!UIGameMenu.IsEnable) UIGameMenu.IsEnable = true;

                if (!UICharacterStatus.IsEnable) UICharacterStatus.IsEnable = true;
                if (!UIInventory.IsEnable) UIInventory.IsEnable = true;
                if (!UIItemDetail.IsEnable) UIItemDetail.IsEnable = true;
                if (!UINearby.IsEnable) UINearby.IsEnable = true;
                if (!UIHelp.IsEnable) UIHelp.IsEnable = true;
                //if (!UIHelp.IsShow) UIHelp.Show();
            }

            if (!UISave.IsEnable) UISave.IsEnable = true;
            if (!UISetting.IsEnable) UISetting.IsEnable = true;
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
                    if (UIInventory.IsShow)
                    {
                        UIInventory.Hide();
                        if (UIItemDetail.IsShow) UIItemDetail.Hide();
                        if (UINearby.IsShow) UINearby.Hide();
                        UIHelp.Show();
                    }
                    else
                    {
                        UIInventory.Show();
                        UIItemDetail.SetData(CharacterManager.Player.GetRightHandItem());
                        if (!UIItemDetail.IsShow) UIItemDetail.Show();
                        if (!UINearby.IsShow) UINearby.Show();
                        UIHelp.Hide();
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
                    if (IsShowAnyUIPublic)
                    {
                        HideUIPublic();
                        return;
                    }

                    if (UILobbyMenu.IsShow) UILobbyMenu.Hide();
                    else UILobbyMenu.Show();
                }
                else
                {
                    if (IsShowAnyUIInGame)
                    {
                        HideUIInGame();
                        return;
                    }
                    if (IsShowAnyUIPublic)
                    {
                        HideUIPublic();
                        return;
                    }

                    if (UIGameMenu.IsShow) UIGameMenu.Hide();
                    else UIGameMenu.Show();
                }
            }
        }

        public void ShowCurrentUIMenu()
        {
            if (GameManager.IsInLobby)
            {
                if (!UILobbyMenu.IsShow) UILobbyMenu.Show();
            }
            else
            {
                if (!UIGameMenu.IsShow) UIGameMenu.Show();
            }
        }
        private void HideUIInGame()
        {
            if (UIInventory.IsShow) UIInventory.Hide();
            if (UIItemDetail.IsShow) UIItemDetail.Hide();
            if (UINearby.IsShow) UINearby.Hide();
            if (UICharacterStatus.IsShow) UICharacterStatus.Hide();
            if (UIHelp.IsShow) UIHelp.Hide();
        }
        private void HideUIPublic()
        {
            if (UISetting.IsShow) UISetting.Hide();
            if (UISave.IsShow) UISave.Hide();
            if (UILoading.IsShow) UILoading.Hide();
            if (UIPopup.IsShow) UIPopup.Hide();
        }
    }
}