using UnityEngine;

[CreateAssetMenu(fileName = "AEffect", menuName = "ScriptableObjects/AEffect/FireBall", order = 1)]
public class FireBall : AEffect
{
    public int damage;

    public override void Use(object[] objs)
    {
        origin = (AEntity)objs[0];
        target = (AEntity)objs[1];

        target.TakeDamage(damage, new object[] { origin, target });
    }
}
