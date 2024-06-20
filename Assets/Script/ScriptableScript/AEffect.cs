using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AEffect : ScriptableObject
{
    public int energy;
    public Card prefab;
    public abstract void Use(AEntity origin, AEntity target, object[] objs);
}
