using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public partial class UIShortcuts : UIBase
    {
        public Button inventoryButton;
        public GameObject inventoryPanel;

        public Button skillsButton;
        public GameObject skillsPanel;

        public Button characterInfoButton;
        public GameObject characterInfoPanel;

        public Button questsButton;
        public GameObject questsPanel;

        public Button quitButton;

        void Update()
        {
            Player player = Player.Myself;

            if (player)
            {
                gameObject.SetActive(true);

                inventoryButton.onClick.SetListener(() =>
                {
                    inventoryPanel.SetActive(!inventoryPanel.activeSelf);
                });

                skillsButton.onClick.SetListener(() =>
                {
                    skillsPanel.SetActive(!skillsPanel.activeSelf);
                });

                characterInfoButton.onClick.SetListener(() =>
                {
                    characterInfoPanel.SetActive(!characterInfoPanel.activeSelf);
                });

                quitButton.onClick.SetListener(() =>
                {
                    Application.Quit();
                });
            }
            else gameObject.SetActive(false);
        }
    }
}