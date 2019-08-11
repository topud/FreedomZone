using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class TargetUI : MonoBehaviour
    {
        [SerializeField] private GameObject PanName;
        [SerializeField] private GameObject PanTalk;

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
        }
        public void HideName()
        {
            PanName.SetActive(false);
        }

        public void SetTalk(string str)
        {
            PanTalk.GetComponentInChildren<Text>().text = str;
        }
        public void ShowTalk()
        {
            PanTalk.SetActive(true);
        }
        public void HideTalk()
        {
            PanTalk.SetActive(false);
        }

    }
}