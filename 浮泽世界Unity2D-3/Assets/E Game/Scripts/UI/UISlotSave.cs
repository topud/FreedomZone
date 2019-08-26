using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UISlotSave : UISlotBase<FileInfo>
    {
        [SerializeField] private Image imgIcon;
        [SerializeField] private Text txtTitle;
        [SerializeField] private Text txtTime;

        public override void SetData(FileInfo data)
        {
            base.SetData(data);
        }
        public override void UpdateData()
        {
            Save save = SaveManager.GetSave(Data);
            imgIcon = null;
            txtTitle.text = save.ToString();
            txtTime.text = save.Time.ToString();
        }
    }
}