// a simple kill quest example.
// inherit from KillQuest and overwrite OnKilled for more advanced goals like
// 'kill a player 10 levels above you' or 'kill a pet in a guild war' etc.
using UnityEngine;
using System.Text;

[CreateAssetMenu(menuName = "E Quest/击杀", order = 2)]
public class KillQuest : ScriptableQuest
{
    [Header("执行")]
   // public Monster killTarget;
    public int killAmount;

}
