// Note: this script has to be on an always-active UI parent, so that we can
// always find it from other code. (GameObject.Find doesn't find inactive ones)
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public partial class UIRespawn : UIBase
    {
        public Button button;

        void Update()
        {
            Player player = Player.Myself;

            if (player != null && player.DynamicData.Health == 0)
            {
                gameObject.SetActive(true);
                //button.onClick.SetListener(() => { player.Respawn(); });
            }
            else gameObject.SetActive(false);
        }
    }
}