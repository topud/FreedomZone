using UnityEngine;
using UnityEngine.UI;
using System;

namespace E.Tool
{
    public class UICharacterDetail : UIBase
    {
        [Header("组件")]
        [SerializeField] private UICharacterInfo uiCharacterInfo;
        [SerializeField] private UICharacterInventory uiCharacterInventory;
        [SerializeField] private UICharacterEquipment uiCharacterEquipment;
        [SerializeField] private UICharacterSkill uiCharacterSkill;
        [SerializeField] private UICharacterQuest uiCharacterQuest;
        public Toggle TogInfo;
        public Toggle TogInventory;
        public Toggle TogEquipment;
        public Toggle TogSkill;
        public Toggle TogQuest;
        public ToggleGroup TogGroup;

        [Header("数据")]
        public static Character Target;

        private void Awake()
        {
            uiCharacterInventory.slotPrefab.SetActive(false);

            TogInfo.onValueChanged.AddListener((bool isOn) => { OnToggleClick(TogInfo); });
            TogInventory.onValueChanged.AddListener((bool isOn) => { OnToggleClick(TogInventory); });
            TogEquipment.onValueChanged.AddListener((bool isOn) => { OnToggleClick(TogEquipment); });
            TogSkill.onValueChanged.AddListener((bool isOn) => { OnToggleClick(TogSkill); });
            TogQuest.onValueChanged.AddListener((bool isOn) => { OnToggleClick(TogQuest); });
        }
        private void Start()
        {
            if (!Target) Target = Player.Myself;
            UpdateInventory();
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

                uiCharacterInfo.txtHealth.text = Target.DynamicData.MaxHealth.ToString();
                uiCharacterInfo.txtMind.text = Target.DynamicData.MaxMind.ToString();
                uiCharacterInfo.txtPower.text = Target.DynamicData.MaxPower.ToString();
                uiCharacterInfo.txtIntelligence.text = Target.DynamicData.Intelligence.ToString();
                uiCharacterInfo.txtSpeed.text = Target.DynamicData.MaxSpeed.ToString();
                uiCharacterInfo.txtStrength.text = Target.DynamicData.Strength.ToString();
                uiCharacterInfo.txtDefense.text = Target.DynamicData.Defense.ToString();
            }
        }
        private void OnToggleClick(Toggle toggle)
        {
            if (toggle.isOn)
            {
                if (toggle == TogInfo)
                {
                    ShowInfo();
                }
                else if (toggle == TogInventory)
                {
                    ShowInventory();
                }
                else if (toggle == TogEquipment)
                {
                    ShowEquipment();
                }
                else if (toggle == TogSkill)
                {
                    ShowSkills();
                }
                else if (toggle == TogQuest)
                {
                    ShowQuests();
                }
            }
        }

        public void ShowInfo()
        {
            uiCharacterInfo.scrInfo.SetActive(true);
            uiCharacterInfo.panInfoDetail.SetActive(true);
            uiCharacterInventory.scrItems.SetActive(false);
            uiCharacterInventory.panItemDetail.SetActive(false);
            uiCharacterEquipment.panEquipDetail.SetActive(false);
            uiCharacterSkill.scrSkills.SetActive(false);
            uiCharacterSkill.panSkillDetail.SetActive(false);
            uiCharacterQuest.scrQuests.SetActive(false);
            uiCharacterQuest.panQuestDetail.SetActive(false);
        }
        public void ShowInventory()
        {
            uiCharacterInfo.scrInfo.SetActive(false);
            uiCharacterInfo.panInfoDetail.SetActive(false);
            uiCharacterInventory.scrItems.SetActive(true);
            uiCharacterInventory.panItemDetail.SetActive(true);
            uiCharacterEquipment.panEquipDetail.SetActive(false);
            uiCharacterSkill.scrSkills.SetActive(false);
            uiCharacterSkill.panSkillDetail.SetActive(false);
            uiCharacterQuest.scrQuests.SetActive(false);
            uiCharacterQuest.panQuestDetail.SetActive(false);
        }
        public void ShowEquipment()
        {
            uiCharacterInfo.scrInfo.SetActive(false);
            uiCharacterInfo.panInfoDetail.SetActive(false);
            uiCharacterInventory.scrItems.SetActive(true);
            uiCharacterInventory.panItemDetail.SetActive(false);
            uiCharacterEquipment.panEquipDetail.SetActive(true);
            uiCharacterSkill.scrSkills.SetActive(false);
            uiCharacterSkill.panSkillDetail.SetActive(false);
            uiCharacterQuest.scrQuests.SetActive(false);
            uiCharacterQuest.panQuestDetail.SetActive(false);
        }
        public void ShowSkills()
        {
            uiCharacterInfo.scrInfo.SetActive(false);
            uiCharacterInfo.panInfoDetail.SetActive(false);
            uiCharacterInventory.scrItems.SetActive(false);
            uiCharacterInventory.panItemDetail.SetActive(false);
            uiCharacterEquipment.panEquipDetail.SetActive(false);
            uiCharacterSkill.scrSkills.SetActive(true);
            uiCharacterSkill.panSkillDetail.SetActive(true);
            uiCharacterQuest.scrQuests.SetActive(false);
            uiCharacterQuest.panQuestDetail.SetActive(false);
        }
        public void ShowQuests()
        {
            uiCharacterInfo.scrInfo.SetActive(false);
            uiCharacterInfo.panInfoDetail.SetActive(false);
            uiCharacterInventory.scrItems.SetActive(false);
            uiCharacterInventory.panItemDetail.SetActive(false);
            uiCharacterEquipment.panEquipDetail.SetActive(false);
            uiCharacterSkill.scrSkills.SetActive(false);
            uiCharacterSkill.panSkillDetail.SetActive(false);
            uiCharacterQuest.scrQuests.SetActive(true);
            uiCharacterQuest.panQuestDetail.SetActive(true);
        }

        
        public void UpdateInventory()
        {
            for (int i = 1; i < uiCharacterInventory.content.childCount; i++)
            {
                Destroy(uiCharacterInventory.content.GetChild(i).gameObject);
            }

            if (!Target)
            {
                return;
            }

            foreach (Item item in Target.DynamicData.Items)
            {
                GameObject go = Instantiate(uiCharacterInventory.slotPrefab, uiCharacterInventory.content);
                go.GetComponent<UISlotItem>().SetData(item);
                go.SetActive(true);
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
    public class UICharacterInventory
    {
        public GameObject scrItems;
        public GameObject panItemDetail;
        [Space(5)]
        public Transform content;
        public GameObject slotPrefab;
    }
    [Serializable]
    public class UICharacterEquipment
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