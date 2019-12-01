using UnityEngine;
using UnityEngine.UI;
using System;

namespace E.Tool
{
    public class UIEquipment : UIBasePanel
    {
        [Header("视图")]
        [SerializeField] private Text txtName;

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
                txtName.text = Data.StaticData.Name;
            }
        }
    }
}