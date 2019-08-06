using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UIItemSlot : UIBaseSlot
    {
        public Button button;
        public UIDragAndDropable dragAndDropable;
        public Image image;
        public GameObject amountOverlay;
        public Text amountText;
    }
}