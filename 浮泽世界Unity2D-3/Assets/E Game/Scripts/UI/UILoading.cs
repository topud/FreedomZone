using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UILoading : UIBase
    {
        [SerializeField] private Text txtProcess;
        [SerializeField] private Slider sldProcess;
        [SerializeField] private Text txtTip;

        private void Update()
        {
            float pro = GameManager.Scene.SceneLoadProgress;
            if (pro < 0)
            {
                txtTip.enabled = false;
                return;
            }
            if (pro != sldProcess.value)
            {
                txtProcess.text = (int)(pro * 100) + "%";
                sldProcess.value = pro;
            }
        }
    }
}