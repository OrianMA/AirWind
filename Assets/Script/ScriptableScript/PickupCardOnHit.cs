using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AEffect", menuName = "ScriptableObjects/AEffect/PickupCardOnHit", order = 1)]
public class PickupCardOnHit: AEffect
{
    public override void Use(object[] objs)
    {
        origin = (AEntity)objs[0];
        target = (AEntity)objs[1];
        GameEventSystem.instance.Listen(EEventType.Damage, PickupCard);
    }

    void PickupCard(object[] objs)
    {
        if (((AEntity)objs[1]).playerId == origin.playerId)
        {
            origin.PickupCard();
        }
    }
}
