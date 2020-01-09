using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlotHelp : MonoBehaviour
{
    public Type type;

    public enum Type
    {
        LM_Survey,
        LM_Attack,
        RM_UseItem,
        MM_PutItem,
        Wheel_SwitchItem,

        Q_DiscardItem,
        E_PickUpItem,
        E_ChatWithCharacter,
        F_Survey,
        Tab_SwitchMode,
        I_Bag
    }
}
