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
        public UIGameMenu UIGameMenu;
        [Space(5)]
        public UIInventory UIInventory;
        public UIItemDetail UIItemDetail;
        public UICharacterStatus UICharacterStatus;
        public UIEquipment UIEquipment;
        public UIHelp UIHelp;
        [Space(5)]
        public UIListSave UISave;
        public UISetting UISetting;
        public UILoading UILoading;
        public UIPopup UIPopup;

        //[Header("数据")]
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
                    (Singleton.UIEquipment.IsEnable && Singleton.UIEquipment.IsShow);
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
        private void Reset()
        {
            UIEquipment = GetComponentInChildren<UIEquipment>(true);
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

                if (UIInventory.IsEnable) UIInventory.IsEnable = false;
                if (UIItemDetail.IsEnable) UIItemDetail.IsEnable = false;
                if (UICharacterStatus.IsEnable) UICharacterStatus.IsEnable = false;
                if (UIEquipment.IsEnable) UIEquipment.IsEnable = false;

                //if (!UILobbyMenu.IsShow) UILobbyMenu.Show();
            }
            else
            {
                if (UILobbyMenu.IsEnable) UILobbyMenu.IsEnable = false;
                if (!UIGameMenu.IsEnable) UIGameMenu.IsEnable = true;

                if (!UIInventory.IsEnable) UIInventory.IsEnable = true;
                if (!UIItemDetail.IsEnable) UIItemDetail.IsEnable = true;
                if (!UICharacterStatus.IsEnable) UICharacterStatus.IsEnable = true;
                if (!UIEquipment.IsEnable) UIEquipment.IsEnable = true;
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
                        if (UIEquipment.IsShow) UIEquipment.Hide();
                        UIHelp.Show();
                    }
                    else
                    {
                        UIInventory.Show();
                        UIItemDetail.SetData(CharacterManager.Player.GetRightHandItem());
                        if (!UIItemDetail.IsShow) UIItemDetail.Show();
                        if (!UIEquipment.IsShow) UIEquipment.Show();
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
            if (UIEquipment.IsShow) UIEquipment.Hide();
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