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
    private Role target;
    public Role Target
    {
        get => target;
        set => target = value;
    }

    private void Start()
    {
        if (!Target) Target = GameManager.Character.Player;
    }

    private void Update()
    {
        if (Target)
        {
            txtName.text = Target.StaticData.name;
            txtDescription.text = Target.StaticData.description;

            txtBirthday.text = Target.StaticData.Birthday.ToString("yyyy 年 M 月 d 日");
            txtGender.text = Target.StaticData.gender.ToString();
            txtHeight.text = Target.StaticData.height.ToString() + " cm";
            txtWeight.text = Target.StaticData.weight.ToString() + " kg";

            txtDegree.text = Target.StaticData.educational.degree.ToString();
            txtStartYear.text = Target.StaticData.educational.startYear.ToString() + " 年";
            txtCollege.text = Target.StaticData.educational.college.ToString();
            txtProfession.text = Target.StaticData.educational.profession.ToString();
            txtGrade.text = Target.StaticData.educational.grade.ToString();
            txtClass.text = Target.StaticData.educational.@class.ToString();
            txtStudentID.text = Target.StaticData.educational.studentID.ToString();

            txtHealth.text = Target.DynamicData.bodyState.health.Max.ToString();
            txtMind.text = Target.DynamicData.bodyState.mind.Max.ToString();
            txtPower.text = Target.DynamicData.bodyState.power.Max.ToString();
            //txtIntelligence.text = Target.DynamicData.iq.ToString();
            txtSpeed.text = Target.DynamicData.physique.speed.ToString();
            txtStrength.text = Target.DynamicData.physique.force.ToString();
            txtDefense.text = Target.DynamicData.physique.defense.ToString();
        }
    }
}
