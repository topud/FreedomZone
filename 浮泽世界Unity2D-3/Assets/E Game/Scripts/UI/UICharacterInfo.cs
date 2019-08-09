// Note: this script has to be on an always-active UI parent, so that we can
// always react to the hotkey.
using UnityEngine;
using UnityEngine.UI;
using System;

namespace E.Tool
{
    public class UICharacterInfo : UIBase
    {
        [Header("组件")]
        [SerializeField] private Text txtBirthday;
        [SerializeField] private Text txtGender;
        [SerializeField] private Text txtHeight;
        [SerializeField] private Text txtWeight;
        [Space(5)]
        [SerializeField] private Text txtStartYear;
        [SerializeField] private Text txtCollege;
        [SerializeField] private Text txtProfession;
        [SerializeField] private Text txtDegree;
        [SerializeField] private Text txtGrade;
        [SerializeField] private Text txtClass;
        [SerializeField] private Text txtStudentID;
        [Space(5)]
        [SerializeField] private Text txtHealth;
        [SerializeField] private Text txtMind;
        [SerializeField] private Text txtPower;
        [SerializeField] private Text txtIntelligence;
        [SerializeField] private Text txtSpeed;
        [SerializeField] private Text txtStrength;
        [SerializeField] private Text txtDefense;

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
                txtBirthday.text = Character.StaticData.Birthday.ToString("yyyy 年 M 月 d 日");
                txtGender.text = Character.StaticData.Gender.ToString();
                txtHeight.text = Character.StaticData.Height.ToString() + " cm";
                txtWeight.text = Character.StaticData.Weight.ToString() + " kg";

                txtStartYear.text = Character.StaticData.StartYear.ToString() + " 年";
                txtCollege.text = Character.StaticData.College.ToString();
                txtProfession.text = Character.StaticData.Profession.ToString();
                txtDegree.text = Character.StaticData.Degree.ToString();
                txtGrade.text = Character.StaticData.Grade.ToString();
                txtClass.text = Character.StaticData.Class.ToString();
                txtStudentID.text = Character.StaticData.StudentID.ToString();

                txtHealth.text = Character.DynamicData.MaxHealth.ToString();
                txtMind.text = Character.DynamicData.MaxMind.ToString();
                txtPower.text = Character.DynamicData.MaxPower.ToString();
                txtIntelligence.text = Character.DynamicData.Intelligence.ToString();
                txtSpeed.text = Character.DynamicData.MaxSpeed.ToString();
                txtStrength.text = Character.DynamicData.Strength.ToString();
                txtDefense.text = Character.DynamicData.Defense.ToString();
            }
        }
    }
}