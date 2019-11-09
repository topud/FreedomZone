using UnityEngine;
using UnityEngine.UI;
using System;

namespace E.Tool
{
    public class UICharacterStatus : UIBasePanel
    {
        [Header("组件")]
        [SerializeField] private Slider sldHealth;
        [SerializeField] private Slider sldMind;
        [SerializeField] private Slider sldPower;

        [SerializeField] private UISlotBuff slotPrefab;

        [Header("数据")]
        public static Character Target;

        private void Start()
        {
            Target = Character.Player;
        }
        private void Update()
        {
            if (Target)
            {
                sldHealth.value = Target.DynamicData.Health.NowPercent;
                sldMind.value = Target.DynamicData.Mind.NowPercent;
                sldPower.value = Target.DynamicData.Power.NowPercent;
            }
        }
    }
}