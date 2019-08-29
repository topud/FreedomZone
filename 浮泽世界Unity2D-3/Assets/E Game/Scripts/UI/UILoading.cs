using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class UILoading : MonoBehaviour
    {
        [SerializeField] private Slider loadingSlider;
        [SerializeField] private Text loadingText;

        private void Update()
        {
            float pro = GameManager.Singleton.SceneLoadProcess;
            if (pro < 0)
            {
                return;
            }
            if (pro != loadingSlider.value)
            {
                //插值运算  
                loadingSlider.value = Mathf.Lerp(loadingSlider.value, pro, Time.deltaTime * 1);
                if (Mathf.Abs(loadingSlider.value - pro) < 0.01f)
                {
                    loadingSlider.value = pro;
                }
                loadingText.text = ((int)(loadingSlider.value * 100)).ToString() + "%";
            }
        }
    }
}