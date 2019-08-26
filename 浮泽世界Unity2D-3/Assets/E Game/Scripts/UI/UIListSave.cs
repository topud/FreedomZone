using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    public class UIListSave : UIListBase<FileInfo, UISlotSave>
    {
        public override void LoadData()
        {
            //base.LoadData();
            Datas = SaveManager.GetSaveFiles();
        }

        public void Create()
        {
            SaveManager.CreateSaveFile();
            Refresh();
        }
        public void Load(UISlotSave slot)
        {
            SaveManager.LoadFrom(slot.Data);
        }
        public void Save(UISlotSave slot)
        {
            SaveManager.SaveTo(slot.Data);
        }
        public void Delete(UISlotSave slot)
        {
            SaveManager.RemoveSaveFile(slot.Data);
        }
    }
}