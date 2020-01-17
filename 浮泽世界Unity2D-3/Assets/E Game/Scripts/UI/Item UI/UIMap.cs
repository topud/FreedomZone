using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace E.Tool
{
    public class UIMap : UIBase
    {
        public float zoomMin = 5;
        public float zoomMax = 50;
        public float zoomStepSize = 5;
        public Text sceneText;
        public Button plusButton;
        public Button minusButton;
        public Camera minimapCamera;

        void Start()
        {
            plusButton.onClick.AddListener(() =>
            {
                minimapCamera.orthographicSize = Mathf.Max(minimapCamera.orthographicSize - zoomStepSize, zoomMin);
            });
            minusButton.onClick.AddListener(() =>
            {
                minimapCamera.orthographicSize = Mathf.Min(minimapCamera.orthographicSize + zoomStepSize, zoomMax);
            });
        }

        void Update()
        {
            Character player = GameManager.Character.Player;
            if (player)
            {
                gameObject.SetActive(true);
                //sceneText.text = SceneManager.GetActiveScene().name;
            }
            else gameObject.SetActive(false);
        }
    }
}