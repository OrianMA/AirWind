using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public AEffect effect;
    public void DestroyCard()
    {
        GameEventSystem.instance.Send(EEventType.CardDestroy, new object[] {this});
        Destroy(this);
    }

    public void UseLisnable(AEntity origin, AEntity target)
    {
        GameEventSystem.instance.Send(EEventType.CardUse, new object[] {origin, target, effect, this});
    }
}
