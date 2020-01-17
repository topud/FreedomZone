using UnityEngine;
using UnityEngine.UI;
using System;

namespace E.Tool
{
    public class UICharacterStatus : UIBase
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
            Data = GameManager.Character.Player;
        }
        private void Update()
        {
            if (Data)
            {
                sldHealth.value = Data.DynamicData.health.NowPercent;
                sldMind.value = Data.DynamicData.mind.NowPercent;
                sldPower.value = Data.DynamicData.power.NowPercent;
            }
        }
    }
}