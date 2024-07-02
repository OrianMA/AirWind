using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Status : AEffect
{
    public override void Use(object[] objs)
    {
        throw new System.NotImplementedException();
    }

    public abstract void Bind();
}
