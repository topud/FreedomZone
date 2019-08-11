// Note: this script has to be on an always-active UI parent, so that we can
// always react to the hotkey.
using UnityEngine;
using UnityEngine.UI;
using System;

namespace E.Tool
{
    public class UICharacter : UIBase
    {
        [Header("组件")]
        [SerializeField] private GameObject scrInfo;
        [SerializeField] private GameObject scrItems;
        [SerializeField] private GameObject scrSkills;
        [SerializeField] private GameObject scrQuests;
        [SerializeField] private GameObject panInfoDetail;
        [SerializeField] private GameObject panItemDetail;
        [SerializeField] private GameObject panEquipDetail;
        [SerializeField] private GameObject panSkillDetail;
        [SerializeField] private GameObject panQuestDetail;

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
            }
        }

        public void ShowInfo()
        {
            scrInfo.SetActive(true);
            scrItems.SetActive(false);
            scrSkills.SetActive(false);
            scrQuests.SetActive(false);
            panInfoDetail.SetActive(true);
            panItemDetail.SetActive(false);
            panEquipDetail.SetActive(false);
            panSkillDetail.SetActive(false);
            panQuestDetail.SetActive(false);
        }
        public void ShowInventory()
        {
            scrInfo.SetActive(false);
            scrItems.SetActive(true);
            scrSkills.SetActive(false);
            scrQuests.SetActive(false);
            panInfoDetail.SetActive(false);
            panItemDetail.SetActive(true);
            panEquipDetail.SetActive(false);
            panSkillDetail.SetActive(false);
            panQuestDetail.SetActive(false);
        }
        public void ShowEquipment()
        {
            scrInfo.SetActive(false);
            scrItems.SetActive(true);
            scrSkills.SetActive(false);
            scrQuests.SetActive(false);
            panInfoDetail.SetActive(false);
            panItemDetail.SetActive(false);
            panEquipDetail.SetActive(true);
            panSkillDetail.SetActive(false);
            panQuestDetail.SetActive(false);
        }
        public void ShowSkills()
        {
            scrInfo.SetActive(false);
            scrItems.SetActive(false);
            scrSkills.SetActive(true);
            scrQuests.SetActive(false);
            panInfoDetail.SetActive(false);
            panItemDetail.SetActive(false);
            panEquipDetail.SetActive(false);
            panSkillDetail.SetActive(true);
            panQuestDetail.SetActive(false);
        }
        public void ShowQuests()
        {
            scrInfo.SetActive(false);
            scrItems.SetActive(false);
            scrSkills.SetActive(false);
            scrQuests.SetActive(true);
            panInfoDetail.SetActive(false);
            panItemDetail.SetActive(false);
            panEquipDetail.SetActive(false);
            panSkillDetail.SetActive(false);
            panQuestDetail.SetActive(true);
        }
    }
}