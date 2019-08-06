using UnityEngine;
using System.Text;

[CreateAssetMenu(menuName = "E Quest/收集", order = 0)]
public class GatherQuest : ScriptableQuest
{
    [Header("Fulfillment")]
    public InteractorStaticData gatherItem;
    public int gatherAmount;
}