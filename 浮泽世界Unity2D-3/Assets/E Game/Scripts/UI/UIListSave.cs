using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UIListSave : UIList<FileInfo, UISlotSave>
    {
        [SerializeField] private Button btnCreate;
        [SerializeField] private Button btnClear;
        [SerializeField] private Text txtTip;

        private OpenMode openMode = OpenMode.Load;
        public OpenMode OpenMode
        {
            get => openMode;
            set
            {
                openMode = value;
                switch (openMode)
                {
                    case OpenMode.Save:
                        btnCreate.gameObject.SetActive(true);
                        break;
                    case OpenMode.Load:
                        btnCreate.gameObject.SetActive(false);
                        break;
                    case OpenMode.Both:
                        btnCreate.gameObject.SetActive(true);
                        break;
                    default:
                        break;
                }
            }
        }

        private void Update()
        {
        }

        public override void LoadData()
        {
            //base.LoadData();
            Datas = SaveManager.GetSaveFiles();
            Debug.Log("存档数量 " + Datas.Count);
        }
        public override void SetPanel()
        {
            base.SetPanel();
            txtTip.enabled = Datas.Count == 0 ? true : false;
            btnClear.gameObject.SetActive(Datas.Count == 0 ? false : true);
        }

        public void Create()
        {
            SaveManager.SaveTo(SaveManager.CreateSaveFile());
            Refresh();
        }
        public void Load(UISlotSave slot)
        {
            if (GameManager.IsInLobby)
            {
                GameManager.ContinueSelectSave(slot.Data);
            }
            else
            {
                SaveManager.LoadFrom(slot.Data);
            }
            Hide();
        }
        public void Save(UISlotSave slot)
        {
            SaveManager.SaveTo(slot.Data);
            Refresh();
        }
        public void Delete(UISlotSave slot)
        {
            SaveManager.RemoveSaveFile(slot.Data);
            Refresh();
        }
        public void DeleteAll()
        {
            SaveManager.RemoveAllSaveFile();
            Refresh();
        }
    }

    public enum OpenMode
    {
        Save,
        Load,
        Both
    }
}