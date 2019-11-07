using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using E.Tool;
using E.Game;

public class AppStudentCard : PhoneApp
{
    [Header("视图")]
    public GameObject scrInfo;
    public GameObject panInfoDetail;
    [Space(5)]
    public Text txtName;
    public Text txtDescription;
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

    [Header("数据")]
    private Character target;
    public Character Target
    {
        get => target;
        set => target = value;
    }

    private void Start()
    {
        if (!Target) Target = Character.Player;
    }

    private void Update()
    {
        if (Target)
        {
            txtName.text = Target.StaticData.Name;
            txtDescription.text = Target.StaticData.Describe;

            txtBirthday.text = Target.StaticData.Birthday.ToString("yyyy 年 M 月 d 日");
            txtGender.text = Target.StaticData.Gender.ToString();
            txtHeight.text = Target.StaticData.Height.ToString() + " cm";
            txtWeight.text = Target.StaticData.Weight.ToString() + " kg";

            txtStartYear.text = Target.StaticData.StartYear.ToString() + " 年";
            txtCollege.text = Target.StaticData.College.ToString();
            txtProfession.text = Target.StaticData.Profession.ToString();
            txtDegree.text = Target.StaticData.Degree.ToString();
            txtGrade.text = Target.StaticData.Grade.ToString();
            txtClass.text = Target.StaticData.Class.ToString();
            txtStudentID.text = Target.StaticData.StudentID.ToString();

            txtHealth.text = Target.DynamicData.Health.Max.ToString();
            txtMind.text = Target.DynamicData.Mind.Max.ToString();
            txtPower.text = Target.DynamicData.Power.Max.ToString();
            txtIntelligence.text = Target.DynamicData.IQ.ToString();
            txtSpeed.text = Target.DynamicData.Speed.ToString();
            txtStrength.text = Target.DynamicData.Strength.ToString();
            txtDefense.text = Target.DynamicData.Defense.ToString();
        }
    }
}
