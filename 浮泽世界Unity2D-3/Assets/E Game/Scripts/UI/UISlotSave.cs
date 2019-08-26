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

        [SerializeField] private Button btnSave;
        [SerializeField] private Button btnLoad;
        [SerializeField] private Button btnDelete;

        public override void SetData(FileInfo data)
        {
            base.SetData(data);
        }
        public override void UpdateData()
        {
            Save save = SaveManager.GetSave(Data);
            imgIcon = null;
            txtTitle.text = save.NodeID.ToString();
            txtTime.text = Data.LastWriteTime.ToLongDateString();
            
            switch (GetComponentInParent<UIListSave>().OpenMode)
            {
                case OpenMode.Save:
                    btnSave.gameObject.SetActive(true);
                    btnLoad.gameObject.SetActive(false);
                    btnDelete.gameObject.SetActive(true);
                    break;
                case OpenMode.Load:
                    btnSave.gameObject.SetActive(false);
                    btnLoad.gameObject.SetActive(true);
                    btnDelete.gameObject.SetActive(true);
                    break;
                case OpenMode.Both:
                    btnSave.gameObject.SetActive(true);
                    btnLoad.gameObject.SetActive(true);
                    btnDelete.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}