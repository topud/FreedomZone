using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public class UIBase : MonoBehaviour
    {
        protected Animator Animator
        {
            get => GetComponent<Animator>();
        }
        public CanvasGroup CanvasGroup
        {
            get => GetComponent<CanvasGroup>();
        }
        
        public bool IsShow
        {
            get => Animator.GetBool("IsShow");
        }
        public bool IsEnable
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        public virtual void Show()
        {
            Animator.SetBool("IsShow", true);
        }
        public virtual void Hide()
        {
            Animator.SetBool("IsShow", false);
        }
    }
}
