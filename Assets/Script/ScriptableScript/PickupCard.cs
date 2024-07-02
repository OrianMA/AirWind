using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AEffect", menuName = "ScriptableObjects/AEffect/PickupCard", order = 1)]
public class PickupCard : AEffect
{
    public override void Use(object[] objs)
    {
        origin = (AEntity)objs[0];
        target = (AEntity)objs[1];
        origin.PickupCard();
    }
}
