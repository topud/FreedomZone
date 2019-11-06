using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class EntityUI : MonoBehaviour
    {
        [SerializeField] private GameObject PanName;
        [SerializeField] private GameObject PanTalk;

        private void Awake()
        {
            HideAll();
        }

        public void SetName(string str)
        {
            PanName.GetComponentInChildren<Text>().text = str;
        }
        public void SetNameColor(Color color)
        {
            PanName.GetComponentInChildren<Text>().color = color;
        }
        public void ShowName()
        {
            PanName.SetActive(true);
            HideChat();
        }
        public void HideName()
        {
            PanName.SetActive(false);
        }

        public bool IsShowChat()
        {
            return PanTalk.gameObject.activeInHierarchy;
        }
        public void SetChat(string str)
        {
            PanTalk.GetComponentInChildren<Text>().text = str;
        }
        public void ShowChat()
        {
            PanTalk.SetActive(true);
            HideName();
        }
        public void HideChat()
        {
            PanTalk.SetActive(false);
        }

        public void HideAll()
        {
            HideName();
            HideChat();
        }
    }
}