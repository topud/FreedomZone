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

        private void OnEnable()
        {
            if (Data != null)
            {
                UpdateData();
            }
        }

        public override void SetData(FileInfo data)
        {
            base.SetData(data);
        }
        public override void UpdateData()
        {
            Save save = SaveManager.GetSave(Data);
            if (save == null) return;

            imgIcon = null;
            NodeID id = save.NodeID;
            txtTitle.text =string.Format("{0}-{1}-{2}-{3}-{4}",id.Round,id.Chapter,id.Scene,id.Part,id.Branch);
            txtTime.text = Data.LastWriteTime.ToString();
            
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