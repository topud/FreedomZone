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
using E.Utility;

namespace E.Tool
{
    public class UIManager : SingletonPattern<UIManager>
    {
        [Header("【组件】")]
        [SerializeField] private CanvasGroup cvgStatic = null;
        [SerializeField] private CanvasGroup cvgWindows = null;
        [SerializeField] private CanvasGroup cvgPopups = null;

        [Header("【UI脚本】")]
        public UICharacterInfo UICharacterInfo;
        public UIInventory UIInventory;
        public UISubItems UISubItems;
        public UISkills UISkills;
        public UIAcceptedQuests UIAcceptedQuests;
        public UIPublishedQuests UIPublishedQuests;
        public UIMinimap UIMinimap;

        [Header("【运行时变量】")]
        public UIDisplayMode uiDisplayMode = UIDisplayMode.Default;
        public EntityInfoDisplayMode entityInfoDisplayMode = EntityInfoDisplayMode.HoverShowAndHitShow;

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
            switch (uiDisplayMode)
            {
                case UIDisplayMode.Default:
                    SetCursor(true);
                    cvgStatic.alpha = 1;
                    cvgWindows.alpha = 1;
                    cvgPopups.alpha = 1;
                    break;
                case UIDisplayMode.Debug:
                    SetCursor(true);
                    cvgStatic.alpha = 1;
                    cvgWindows.alpha = 1;
                    cvgPopups.alpha = 1;
                    break;
                case UIDisplayMode.Hide:
                    SetCursor(false);
                    cvgStatic.alpha = 0;
                    cvgWindows.alpha = 0;
                    cvgPopups.alpha = 0;
                    break;
                default:
                    break;
            }
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

        public enum UIDisplayMode
        {
            Default = 0,
            Debug = 1,
            Hide = 2,
        }
        public enum EntityInfoDisplayMode
        {
            AlwaysShow = 0,
            HoverShowOnly = 1,
            HitShowOnly = 2,
            HoverShowAndHitShow = 3,
            AlwaysHide = 4
        }
    }
}