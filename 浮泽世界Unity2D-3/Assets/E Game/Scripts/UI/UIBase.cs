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

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Show()
        {
            animator.SetTrigger("Show");
        }
        public void Hide()
        {
            animator.SetTrigger("Hide");
        }
    }
}
