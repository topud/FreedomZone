using UnityEngine;
using System.Text;

namespace E.Tool
{
    [CreateAssetMenu(menuName = "E Quest/收集", order = 0)]
    public class GatherQuest : ScriptableQuest
    {
        [Header("Fulfillment")]
        public InteractorStaticData gatherItem;
        public int gatherAmount;
    }
}