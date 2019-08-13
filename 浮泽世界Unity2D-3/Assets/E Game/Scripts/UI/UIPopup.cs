using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UIPopup : UIBase
    {
        public static UIPopup singleton;
        public Text messageText;

        public UIPopup()
        {
            if (singleton == null) singleton = this;
        }

        public void Show(string message)
        {
            if (gameObject.activeSelf) messageText.text += ";\n" + message;
            else messageText.text = message;
            gameObject.SetActive(true);
        }
    }
}