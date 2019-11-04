using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    [RequireComponent(typeof(Animator))]
    public class UIBase : MonoBehaviour
    {
        private Animator animator;
        public bool IsShow = false;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Show()
        {
            animator.SetTrigger("Show");
            IsShow = true;
        }
        public void Hide()
        {
            animator.SetTrigger("Hide");
            IsShow = false;
        }
    }
}
