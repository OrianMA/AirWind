using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AEffect
{
    public int energy;
    public abstract void Use(AEntity origin, AEntity target, object[] objs);
}
