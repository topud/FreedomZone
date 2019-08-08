// ========================================================
// 作者：E Star 
// 创建时间：2018-12-21 13:33:17 
// 当前版本：1.0 
// 作用描述：UI管理员
// 挂载目标：UICanvas
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace E.Tool
{
    public class UIManager : SingletonPattern<UIManager>
    {
        [Header("组件")]
        public CanvasGroup LobbyUI;
        public CanvasGroup LoadUI;
        public CanvasGroup GameUI;
        public UICharacterInfo UICharacterInfo;
        public UIInventory UIInventory;
        public UISubItems UISubItems;
        public UISkills UISkills;
        public UIAcceptedQuests UIAcceptedQuests;
        public UIPublishedQuests UIPublishedQuests;
        public UIMinimap UIMinimap;

        protected override void Awake()
        {
            base.Awake();
            UICharacterInfo.gameObject.SetActive(false);
            UIInventory.gameObject.SetActive(false);
            UISubItems.gameObject.SetActive(false);
            UISkills.gameObject.SetActive(false);
            UIAcceptedQuests.gameObject.SetActive(false);
            UIPublishedQuests.gameObject.SetActive(false);
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
        }
        private void Reset()
        {
            LobbyUI = transform.Find("LobbyUI").GetComponent<CanvasGroup>();
            LoadUI = transform.Find("LoadUI").GetComponent<CanvasGroup>();
            GameUI = transform.Find("GameUI").GetComponent<CanvasGroup>();
            UICharacterInfo = GetComponentInChildren<UICharacterInfo>(true);
            UIInventory = GetComponentInChildren<UIInventory>(true);
            UISubItems = GetComponentInChildren<UISubItems>(true);
            UISkills = GetComponentInChildren<UISkills>(true);
            UIAcceptedQuests = GetComponentInChildren<UIAcceptedQuests>(true);
            UIPublishedQuests = GetComponentInChildren<UIPublishedQuests>(true);
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