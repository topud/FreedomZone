using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public partial class UIHealthMana : UIBase
    {
        public Slider healthSlider;
        public Text healthStatus;
        public Slider mindSlider;
        public Text mindStatus;
        public Slider powerSlider;
        public Text powerStatus;

        void Update()
        {
            Player player = Player.Myself;
            if (player)
            {
                gameObject.SetActive(true);

                healthSlider.value = player.GetHealthPercentage();
                healthStatus.text = player.DynamicData.Health + " / " + player.DynamicData.MaxHealth;

                mindSlider.value = player.GetMindPercentage();
                mindStatus.text = player.DynamicData.Mind + " / " + player.DynamicData.MaxMind;

                powerSlider.value = player.GetPowerPercentage();
                powerStatus.text = player.DynamicData.Power + " / " + player.DynamicData.MaxPower;
            }
            else gameObject.SetActive(false);
        }
    }
}