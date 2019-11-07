// ========================================================
// 作者：E Star
// 创建时间：2019-01-27 19:47:38
// 当前版本：1.0
// 作用描述：
// 挂载目标：
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Game
{
    public class PhoneApp : MonoBehaviour
    {
        [Header("视图")]
        [SerializeField] protected GameObject MainPage;
        [SerializeField] protected List<GameObject> Pages = new List<GameObject>();

        [Header("数据")]
        protected Stack PageHistory;

        protected PhoneController PhoneManager
        {
            get => GetComponentInParent<PhoneController>();
        }

        private void Awake()
        {
            PageHistory = new Stack();
        }
        private void OnEnable()
        {
            OpenNewPage(MainPage);
        }
        private void Start()
        {
        }

        /// <summary>
        /// 打开新的一页
        /// </summary>
        /// <param name="page"></param>
        public virtual void OpenNewPage(GameObject page)
        {
            if (page == null)
            {
                Debug.Log("此页不存在");
                return;
            }
            page.SetActive(true);
            page.transform.SetAsLastSibling();
            PageHistory.Push(page);
        }
        /// <summary>
        /// 回到上一页
        /// </summary>
        public virtual void BackLastPage()
        {
            if (PageHistory.Count > 1)
            {
                GameObject currentPage = PageHistory.Peek() as GameObject;
                currentPage.SetActive(true);
                PageHistory.Pop();
                GameObject lastPage = PageHistory.Peek() as GameObject;
                lastPage.transform.SetAsLastSibling();
            }
            else
            {
                PageHistory.Clear();
                PhoneManager.CloseApp(this);
            }
        }
    }
}