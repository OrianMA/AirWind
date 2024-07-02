using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AEffect", menuName = "ScriptableObjects/AEffect/Heal", order = 1)]
public class Heal : AEffect
{
    public int heal;

    public override void Use(object[] objs)
    {
        origin = (AEntity)objs[0];
        target = (AEntity)objs[1];
        target.TakeDamage(-heal, new object[] { origin, target });
    }
}
