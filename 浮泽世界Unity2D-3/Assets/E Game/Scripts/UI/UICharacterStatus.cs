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
        //[SerializeField] private Text txtHealth;
        //[SerializeField] private Text txtMind;
        //[SerializeField] private Text txtPower;

        [SerializeField] private UISlotBuff slotPrefab;

        [Header("数据")]
        public static Character Target;

        private void Awake()
        {

        }
        private void Start()
        {
            Target = Character.Player;
        }
        private void Update()
        {
            if (Target)
            {
                sldHealth.value = Target.GetHealthPercentage();
                sldMind.value = Target.GetMindPercentage();
                sldPower.value = Target.GetPowerPercentage();
                //txtHealth.text = Target.DynamicData.Health + " / " + Target.DynamicData.MaxHealth;
                //txtMind.text = Target.DynamicData.Mind + " / " + Target.DynamicData.MaxMind;
                //txtPower.text = Target.DynamicData.Power + " / " + Target.DynamicData.MaxPower;
            }
        }
    }
}