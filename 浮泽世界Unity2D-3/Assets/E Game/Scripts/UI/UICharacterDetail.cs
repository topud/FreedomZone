using UnityEngine;
using UnityEngine.UI;
using System;

namespace E.Tool
{
    public class UICharacterDetail : UIBase
    {
        [Header("组件")]
        [SerializeField] private UICharacterInfo uiCharacterInfo;
        [SerializeField] private UIEquipment uiCharacterEquipment;
        [SerializeField] private UICharacterSkill uiCharacterSkill;
        [SerializeField] private UICharacterQuest uiCharacterQuest;

        [Header("数据")]
        public static Character Target;

        private void Start()
        {
            if (!Target) Target = Character.Player;
        }
        private void Update()
        {
            if (Target)
            {
                uiCharacterInfo.txtName.text = Target.StaticData.Name;
                uiCharacterInfo.txtDes.text = Target.StaticData.Describe;

                uiCharacterInfo.txtBirthday.text = Target.StaticData.Birthday.ToString("yyyy 年 M 月 d 日");
                uiCharacterInfo.txtGender.text = Target.StaticData.Gender.ToString();
                uiCharacterInfo.txtHeight.text = Target.StaticData.Height.ToString() + " cm";
                uiCharacterInfo.txtWeight.text = Target.StaticData.Weight.ToString() + " kg";

                uiCharacterInfo.txtStartYear.text = Target.StaticData.StartYear.ToString() + " 年";
                uiCharacterInfo.txtCollege.text = Target.StaticData.College.ToString();
                uiCharacterInfo.txtProfession.text = Target.StaticData.Profession.ToString();
                uiCharacterInfo.txtDegree.text = Target.StaticData.Degree.ToString();
                uiCharacterInfo.txtGrade.text = Target.StaticData.Grade.ToString();
                uiCharacterInfo.txtClass.text = Target.StaticData.Class.ToString();
                uiCharacterInfo.txtStudentID.text = Target.StaticData.StudentID.ToString();

                uiCharacterInfo.txtHealth.text = Target.DynamicData.Health.Max.ToString();
                uiCharacterInfo.txtMind.text = Target.DynamicData.Mind.Max.ToString();
                uiCharacterInfo.txtPower.text = Target.DynamicData.Power.Max.ToString();
                uiCharacterInfo.txtIntelligence.text = Target.DynamicData.IQ.ToString();
                uiCharacterInfo.txtSpeed.text = Target.DynamicData.Speed.ToString();
                uiCharacterInfo.txtStrength.text = Target.DynamicData.Strength.ToString();
                uiCharacterInfo.txtDefense.text = Target.DynamicData.Defense.ToString();
            }
        }

    }

    [Serializable]
    public class UICharacterInfo 
    {
        public GameObject scrInfo;
        public GameObject panInfoDetail;
        [Space(5)]
        public Text txtName;
        public Text txtDes;
        [Space(5)]
        public Text txtBirthday;
        public Text txtGender;
        public Text txtHeight;
        public Text txtWeight;
        [Space(5)]
        public Text txtStartYear;
        public Text txtCollege;
        public Text txtProfession;
        public Text txtDegree;
        public Text txtGrade;
        public Text txtClass;
        public Text txtStudentID;
        [Space(5)]
        public Text txtHealth;
        public Text txtMind;
        public Text txtPower;
        public Text txtIntelligence;
        public Text txtSpeed;
        public Text txtStrength;
        public Text txtDefense;
    }
    [Serializable]
    public class UIEquipment
    {
        public GameObject scrItems;
        public GameObject panEquipDetail;
        [Space(5)]
        public UISlotEquipment slotBag;
        public UISlotEquipment slotCoat;
        public UISlotEquipment slotPants;
        public UISlotEquipment slotHat;
        public UISlotEquipment slotMask;
        public UISlotEquipment slotGloves;
        public UISlotEquipment slotShoes;
        public UISlotEquipment slotHandheld;
    }
    [Serializable]
    public class UICharacterSkill
    {
        public GameObject scrSkills;
        public GameObject panSkillDetail;
        [Space(5)]
        public UISlotSkill slotPrefab;
        public Transform content;
        [Space(5)]
        public Text skillExperienceText;
    }
    [Serializable]
    public class UICharacterQuest
    {
        public GameObject scrQuests;
        public GameObject panQuestDetail;
        [Space(5)]
        public Transform trfQuest;
        public UISlotQuest slotQuest;
    }
}