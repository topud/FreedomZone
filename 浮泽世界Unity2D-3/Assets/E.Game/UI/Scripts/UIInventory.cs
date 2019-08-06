using UnityEngine;
using UnityEngine.UI;
using System;

namespace E.Tool
{
    public class UIInventory : UIBase
    {
        [Header("组件")]
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private Transform content;
        [SerializeField] private Text txtRMB;
        [SerializeField] private Text txtFZB;
        [SerializeField] private Text txtWeight;
        [SerializeField] private Text txtVolume;

        [Header("数据")]
        public Character Character;

        private void Start()
        {
            Character = Player.Myself;
        }
        private void Update()
        {
            if (Character)
            {
                txtRMB.text = Character.DynamicData.RMB.ToString();
                txtFZB.text = Character.DynamicData.FZB.ToString();
                txtWeight.text ="";
                txtVolume.text ="";
            }
        }
    }
}