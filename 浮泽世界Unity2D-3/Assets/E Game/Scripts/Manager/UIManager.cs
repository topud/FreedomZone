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
        [Space(5)]
        public UIListSave UISave;
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
                       Singleton.UIInventory.IsShow || Singleton.UIItemDetail.IsShow /*|| Singleton.UICharacterStatus.IsShow*/ || Singleton.UICharacterDetail.IsShow ||
                       Singleton.UISave.IsShow || Singleton.UISetting.IsShow || Singleton.UIHelp.IsShow || Singleton.UILoading.IsShow || Singleton.UIPopup.IsShow;
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
                if (UIItemDetail.IsEnable) UIItemDetail.IsEnable = false;
                if (UICharacterStatus.IsEnable) UICharacterStatus.IsEnable = false;
                if (UICharacterDetail.IsEnable) UICharacterDetail.IsEnable = false;

                if (!UILobbyMenu.IsShow) UILobbyMenu.Show();
            }
            else
            {
                if (UILobbyMenu.IsEnable) UILobbyMenu.IsEnable = false;
                if (!UIGameMenu.IsEnable) UIGameMenu.IsEnable = true;
                if (!UIItemDetail.IsEnable) UIItemDetail.IsEnable = true;
                if (!UICharacterStatus.IsEnable) UICharacterStatus.IsEnable = true;
                if (!UICharacterDetail.IsEnable) UICharacterDetail.IsEnable = true;

            }

            if (!UISave.IsEnable) UISave.IsEnable = true;
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
                        UIInventory.Show();
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
                    if (UIGameMenu.IsShow) UISave.Hide();
                    else UISave.Show();
                }

                if (UICharacterDetail.IsShow) UICharacterDetail.Hide();
                if (UISetting.IsShow) UISetting.Hide();
                if (UISave.IsShow) UISave.Hide();
            }
        }
    }
}