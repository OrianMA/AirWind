using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Card : MonoBehaviour
{
    public AEffect effect;
    public int indexPos;
    public List<int> allCardPosRef;
    public Vector3 originTransformPos;
    public Quaternion originTransformRot;
    public Vector3 originTransformScale;

    [Header("UI")]
    public TMPro.TextMeshProUGUI energyText;

    private void Start()
    {
        GameEventSystem.instance.Listen(EEventType.CardDestroy, DestroyCard);
        energyText.text = effect.energy.ToString();
    }

    public void DestroyCard(object[] objs)
    {
        if ((Card)objs[3] == this)
        {
            AEntity origin = (AEntity)objs[0];
            //origin.cardsInGame.Remove(this);
            AEntity target = (AEntity)objs[1];
            //allCardPosRef.Remove(indexPos);
            DOTween.Sequence().Append(transform.DOMove(target.transform.position + (target.transform.position - transform.position).normalized, .2f))
                .SetEase(Ease.Linear)
                .OnComplete(() =>
             {

                 Destroy(gameObject);
             });
        }
    }

    public void UseLisnable(AEntity origin, AEntity target)
    {
        origin.cardsInGame.Remove(this);
        allCardPosRef.Remove(indexPos);
        GameEventSystem.instance.Send(EEventType.CardUse, new object[] {origin, target, effect, this});
    }

    public void ResetPos()
    {
        transform.DOKill();
        transform.position = originTransformPos;
        transform.rotation = originTransformRot;
        transform.localScale = originTransformScale;
    }
}
