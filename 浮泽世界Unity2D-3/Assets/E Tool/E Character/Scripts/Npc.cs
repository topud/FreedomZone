using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class Npc : Character
    {
        protected override void Awake()
        {
            base.Awake();
        }


        private void CheckMove()
        {
            Animator.SetTrigger("Walk");
            Animator.SetTrigger("Idle");
        }
    }
}