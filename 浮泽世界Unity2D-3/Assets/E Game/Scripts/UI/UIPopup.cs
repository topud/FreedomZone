using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UIPopup : UIBasePanel
    {
        [SerializeField] private Text txtMessage;
        [SerializeField] private GameObject btnsOnlyYes;
        [SerializeField] private GameObject btnsYesOrNo;

        public void ShowMessage(string message, PopupType popupType = PopupType.OnlyYes)
        {
            if (!IsShow) Show();

            switch (popupType)
            {
                case PopupType.OnlyYes:
                    btnsOnlyYes.SetActive(true);
                    btnsYesOrNo.SetActive(false);
                    break;
                case PopupType.YesOrNo:
                    btnsOnlyYes.SetActive(false);
                    btnsYesOrNo.SetActive(true);
                    break;
                default:
                    break;
            }

            txtMessage.text += ";\n" + message;
        }

        public enum PopupType
        {
            OnlyYes,
            YesOrNo,
        }
    }
}