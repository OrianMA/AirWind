using UnityEngine;

[CreateAssetMenu(fileName = "AEffect", menuName = "ScriptableObjects/AEffect/FireBall", order = 1)]
public class FireBall : AEffect
{
    public int damage;
    public override void Use(AEntity origin, AEntity target, object[] objs)
    {
        target.TakeDamage(damage, new object[] {origin, target});
    }
}
