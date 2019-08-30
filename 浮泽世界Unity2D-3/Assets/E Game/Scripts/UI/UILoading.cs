using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UILoading : MonoBehaviour
    {
        [SerializeField] private Text txtProcess;
        [SerializeField] private Slider sldProcess;
        [SerializeField] private Text txtTip;

        private void Update()
        {
            float pro = GameManager.Singleton.SceneLoadProcess;
            if (pro < 0)
            {
                txtTip.enabled = false;
                return;
            }
            if (pro != sldProcess.value)
            {
                //插值运算  
                //sldProcess.value = Mathf.Lerp(sldProcess.value, pro, Time.deltaTime * 1);
                //if (Mathf.Abs(sldProcess.value - pro) < 0.01f)
                //{
                //    sldProcess.value = pro;
                //}
                //txtProcess.text = ((int)(sldProcess.value * 100)).ToString() + "%";
                float v = pro * (10 / 9);
                txtProcess.text = (int)(v * 100) + "%";
                sldProcess.value = v;
            }
        }
    }
}