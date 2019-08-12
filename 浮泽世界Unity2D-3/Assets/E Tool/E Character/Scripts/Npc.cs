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
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void Update()
        {
            base.Update();
            if (State != CharacterState.Dead && State != CharacterState.Talk)
            {
            }
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (State != CharacterState.Dead && State != CharacterState.Talk)
            {
                CheckMove();
            }
        }
        protected override void LateUpdate()
        {
            base.LateUpdate();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }


        private void CheckMove()
        {
            if (AIPath.canMove)
            {
                Animator.SetTrigger("Walk");
            }
            else
            {
                Animator.SetTrigger("Idle");
            }
        }
    }
}