// ========================================================
// 作者：E Star
// 创建时间：2019-01-20 21:20:48
// 当前版本：1.0
// 作用描述：
// 挂载目标：
// ========================================================
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using E.Tool;

namespace E.Game
{
    public class PhoneController : SingletonClass<PhoneController>
    {
        [Header("视图")]
        [SerializeField, Tooltip("应用 主页")] private AppHome AppHome;
        [SerializeField, Tooltip("应用 笔记")] private AppMemo AppMemo;
        [SerializeField, Tooltip("应用 时间")] private AppTime AppTime;
        [SerializeField, Tooltip("应用 学生证")] private AppStudentCard AppStudentCard;
        [SerializeField] private Text txtMessage;
        [SerializeField] private Text txtTime;
        [SerializeField] private Image ImgPower;

        [Header("数据")]
        [SerializeField, ReadOnly, Tooltip("当前应用")] private PhoneApp CurrentApp;
        [SerializeField, Tooltip("剩余电量")] private float LeftPower;
        [SerializeField, Tooltip("电量消耗（每帧）")] private float PowerCost;

        public Animator Animator
        {
            get => GetComponentInParent<Animator>();
        }

        private void Start()
        {
        }
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.P))
            {
                if (Animator.GetBool("IsShowPhone"))
                {
                    SetPhoneUseState(false);
                }
                else
                {
                    SetPhoneUseState(true);
                }
            }

            //滚轮旋转手机
            if (Animator.GetBool("IsShowPhone"))
            {
                float mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
                transform.Rotate(new Vector3(0, 0, mouseScrollWheel * 100));
            }

            ShowTime();
            PowerUsing();
        }

        /// <summary>
        /// 拿出/放下手机
        /// </summary>
        public void SetPhoneUseState(bool IsShow)
        {
            Animator.SetBool("IsShowPhone", IsShow);
            //UIManager.SetCursor(IsShow);
        }

        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="str">消息内容</param>
        public void ShowMessage(string str)
        {
            if (str != string.Empty)
            {
                txtMessage.text = str;
                Animator.SetTrigger("MessageTrigger");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PowerUsing()
        {
            LeftPower -= PowerCost;
            if (LeftPower <= 0)
            {
                LeftPower = 0;
            }

            if (LeftPower <= 0.2 & LeftPower > 0)
            {
                ImgPower.color = Color.red;
            }
            else if (LeftPower <= 0.5 & LeftPower > 0.2)
            {
                ImgPower.color = Color.yellow;
            }
            else
            {
                ImgPower.color = Color.green;
            }

            ImgPower.fillAmount = LeftPower;
        }

        private void ShowTime()
        {
            string week;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    week = "日";
                    break;
                case DayOfWeek.Monday:
                    week = "月";
                    break;
                case DayOfWeek.Tuesday:
                    week = "火";
                    break;
                case DayOfWeek.Wednesday:
                    week = "水";
                    break;
                case DayOfWeek.Thursday:
                    week = "木";
                    break;
                case DayOfWeek.Friday:
                    week = "金";
                    break;
                case DayOfWeek.Saturday:
                    week = "土";
                    break;
                default:
                    week = "日";
                    break;
            }
            string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            txtTime.text = time + " " + week;
        }

        /// <summary>
        /// 点击按钮
        /// </summary>
        public void ClickButton()
        {
            ShowMessage("此功能正在开发中，暂时无法使用");
        }
        /// <summary>
        /// 打开应用
        /// </summary>
        /// <param name="app"></param>
        public void OpenApp(PhoneApp app)
        {
            app.transform.SetAsLastSibling();
            app.gameObject.SetActive(true);
            CurrentApp = app;
        }

        /// <summary>
        /// 关闭应用
        /// </summary>
        /// <param name="app"></param>
        public void CloseApp(PhoneApp app)
        {
            app.gameObject.SetActive(false);
            CurrentApp = null;
        }
        /// <summary>
        /// 关闭当前应用
        /// </summary>
        public void CloseCurrentApp()
        {
            ShowMessage("此功能正在开发中，暂时无法使用");
        }
        /// <summary>
        /// 回到主页
        /// </summary>
        public void BackHome()
        {
            AppHome.transform.SetAsLastSibling();
        }

        /// <summary>
        /// 显示手机
        /// </summary>
        public void ShowPhone()
        {
            gameObject.SetActive(true);
            //UIManager.SetCursor(true);
        }
        /// <summary>
        /// 隐藏手机
        /// </summary>
        public void HidePhone()
        {
            gameObject.SetActive(false);
            //UIManager.SetCursor(false);
        }

    }
}