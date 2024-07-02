using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AEffect", menuName = "ScriptableObjects/AEffect/EnergyGain", order = 1)]
public class EnergyGain : AEffect
{
    public int energyGain;

    public override void Use(object[] objs)
    {
        origin = (AEntity)objs[0];
        target = (AEntity)objs[1];
        target.UpdateEnergy(energyGain);
    }
}
