using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UIManager : MonoBehaviour
    {
        //ui menu
        public UILobbyMenu UILobbyMenu { get => GetComponentInChildren<UILobbyMenu>(true); }
        public UIGameMenu UIGameMenu { get => GetComponentInChildren<UIGameMenu>(true); }
        //ui in game
        public UIInventory UIInventory { get => GetComponentInChildren<UIInventory>(true); }
        public UIItemDetail UIItemDetail { get => GetComponentInChildren<UIItemDetail>(true); }
        public UINearby UINearby { get => GetComponentInChildren<UINearby>(true); }
        //ui in gamehud
        public UICharacterStatus UICharacterStatus { get => GetComponentInChildren<UICharacterStatus>(true); }
        public UIHelp UIHelp { get => GetComponentInChildren<UIHelp>(true); }
        //ui public
        public UIListSave UISave { get => GetComponentInChildren<UIListSave>(true); }
        public UISetting UISetting { get => GetComponentInChildren<UISetting>(true); }
        public UILoading UILoading { get => GetComponentInChildren<UILoading>(true); }
        public UIPopup UIPopup { get => GetComponentInChildren<UIPopup>(true); }

        public bool IsShowAny
        {
            get
            {
                return IsShowAnyUIMenu || IsShowAnyUIInGame || IsShowAnyUIPublic;
            }
        }
        public bool IsShowAnyUIForInteraction
        {
            get
            {
                return IsShowAnyUIMenu || IsShowAnyUIInGameWithoutHUD || IsShowAnyUIPublic;
            }
        }
        public bool IsShowAnyUIMenu
        {
            get
            {
                return (UILobbyMenu.IsEnable && UILobbyMenu.IsShow) || 
                    (UIGameMenu.IsEnable && UIGameMenu.IsShow);
            }
        }
        public bool IsShowAnyUIInGame
        {
            get
            {
                return IsShowAnyUIInGameWithoutHUD || IsShowAnyUIInGameHUD;
            }
        }
        public bool IsShowAnyUIInGameWithoutHUD
        {
            get
            {
                return (UIInventory.IsEnable && UIInventory.IsShow) ||
                    (UIItemDetail.IsEnable && UIItemDetail.IsShow) ||
                    (UINearby.IsEnable && UINearby.IsShow);
            }
        }
        public bool IsShowAnyUIInGameHUD
        {
            get
            {
                return (UICharacterStatus.IsEnable && UICharacterStatus.IsShow) ||
                    (UIHelp.IsEnable && UIHelp.IsShow);
            }
        }
        public bool IsShowAnyUIPublic
        {
            get
            {
                return (UISave.IsEnable && UISave.IsShow )||
                    (UISetting.IsEnable && UISetting.IsShow) ||
                    (UILoading.IsEnable && UILoading.IsShow) ||
                    (UIPopup.IsEnable && UIPopup.IsShow);
            }
        }
        
        private void Update()
        {
            CheckKeyUp_I();
            CheckKeyUp_B();
            CheckKeyUp_Esc();
        }

        /// <summary>
        /// 设置光标状态
        /// </summary>
        /// <param name="isShow">是否展示</param>
        public void SetCursor(bool isShow)
        {
            Cursor.visible = isShow;
            Cursor.lockState = isShow ? CursorLockMode.None : CursorLockMode.Locked;
        }

        public void RefreshMode()
        {
            if (GameManager.Scene.IsInLobby)
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
                if (GameManager.Scene.IsInLobby)
                {
                }
                else
                {
                    //if (UIInventory.IsShow)
                    //{
                    //    UIInventory.Hide();
                    //    if (UIItemDetail.IsShow) UIItemDetail.Hide();
                    //    if (UINearby.IsShow) UINearby.Hide();
                    //    UIHelp.Show();
                    //}
                    //else
                    //{
                    //    UIInventory.Show();
                    //    UIItemDetail.SetData(CharacterManager.Player.GetRightHandItem());
                    //    if (!UIItemDetail.IsShow) UIItemDetail.Show();
                    //    if (!UINearby.IsShow) UINearby.Show();
                    //    UIHelp.Hide();
                    //}
                }
            }
        }
        private void CheckKeyUp_B()
        {
            if (Input.GetKeyUp(KeyCode.B))
            {
                if (GameManager.Scene.IsInLobby)
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
                        UIItemDetail.SetData(GameManager.Character.Player.GetRightHandItem());
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
                if (GameManager.Scene.IsInLobby)
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
                    if (IsShowAnyUIInGameWithoutHUD)
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
            if (GameManager.Scene.IsInLobby)
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