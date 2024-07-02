using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AEffect : ScriptableObject
{
    public int energy;
    public Card prefab;
    public ECardType cardType;

    public AEntity origin;
    public AEntity target;
    public Card card;

    //Origin, Target, Effect, this (card)
    public abstract void Use(object[] objs);
}
