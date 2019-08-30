using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    public class UIListSave : UIListBase<FileInfo, UISlotSave>
    {
        public OpenMode OpenMode = OpenMode.Load;

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.C))
            {
                Create();
            }
            else if(Input.GetKeyUp(KeyCode.L))
            {
                SaveManager.Load();
            }
        }

        public override void LoadData()
        {
            //base.LoadData();
            Datas = SaveManager.GetSaveFiles();
            Debug.Log("存档数量 " + Datas.Count);
        }
        
        public void Create()
        {
            SaveManager.SaveTo(SaveManager.CreateSaveFile());
            Refresh();
        }
        public void Load(UISlotSave slot)
        {
            SaveManager.LoadFrom(slot.Data);
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
    }

    public enum OpenMode
    {
        Save,
        Load,
        Both
    }
}