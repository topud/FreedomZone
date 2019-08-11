using UnityEngine;
using UnityEngine.UI;
using System;

namespace E.Tool
{
    public class UIInventory : UIBase
    {
        [Header("组件")]
        [SerializeField] private Transform content;
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private Text txtRMB;
        [SerializeField] private Text txtFZB;
        [SerializeField] private Text txtWeight;
        [SerializeField] private Text txtVolume;

        [Header("数据")]
        public Character Character;

        private void Awake()
        {
            slotPrefab.SetActive(false);
        }
        private void OnEnable()
        {
        }
        private void Start()
        {
            RefreshContent();
        }
        private void Update()
        {
            if (!Character)
            {
                Character = Player.Myself;
            }

            txtWeight.text = Character.StaticData.Weight.ToString();
            txtVolume.text = Character.DynamicData.Volume.ToString();
            txtRMB.text = Character.DynamicData.RMB.ToString();
            txtFZB.text = Character.DynamicData.FZB.ToString();
        }

        public void RefreshContent()
        {
            for (int i = 1; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }

            if (!Character)
            {
                return;
            }

            foreach (Item item in Character.DynamicData.Items)
            {
                GameObject go = Instantiate(slotPrefab, content);
                go.GetComponent<UIItemSlot>().SetData(item);
                go.SetActive(true);
            }
        }
    }
}