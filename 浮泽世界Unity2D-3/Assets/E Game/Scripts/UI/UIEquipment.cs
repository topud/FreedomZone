using UnityEngine;
using UnityEngine.UI;
using System;

namespace E.Tool
{
    public class UIEquipment : UIBase
    {
        [Header("视图")]
        [SerializeField] private Text txtName;

        [Header("数据")]
        public Character Data;

        private void Start()
        {
            Data = CharacterManager.Player;
        }
        private void Update()
        {
        }
    }
}