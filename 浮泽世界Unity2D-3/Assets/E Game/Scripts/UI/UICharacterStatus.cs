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
        public Character Data;

        private void Start()
        {
            Data = Character.player;
        }
        private void Update()
        {
            if (Data)
            {
                sldHealth.value = Data.DynamicData.Health.NowPercent;
                sldMind.value = Data.DynamicData.Mind.NowPercent;
                sldPower.value = Data.DynamicData.Power.NowPercent;
            }
        }
    }
}