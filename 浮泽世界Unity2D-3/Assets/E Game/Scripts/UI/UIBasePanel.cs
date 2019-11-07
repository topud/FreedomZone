using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public class UIBasePanel : MonoBehaviour
    {
        private Animator Animator
        {
            get => GetComponent<Animator>();
        }
        public CanvasGroup CanvasGroup
        {
            get => GetComponent<CanvasGroup>();
        }

        private bool isShow = false;
        public bool IsShow
        {
            get => isShow;
            private set => isShow = value;
        }
        public bool IsEnable
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }


        public void Show()
        {
            Animator.SetTrigger("Show");
            IsShow = true;
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
        }
        public void Hide()
        {
            Animator.SetTrigger("Hide");
            IsShow = false;
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
        }
    }
}
