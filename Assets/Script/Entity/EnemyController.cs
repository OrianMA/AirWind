using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : AEntity
{
    Card cardSelected;
    AEntity entitySelected;

    bool stopPlay;

    IEnumerator EnemyAttack(object[] argV)
    {
        while(!stopPlay)
        {
            yield return new WaitForSeconds(1);
            List<Card> cardShuffe = new List<Card>();

            foreach(Card card in cardsInGame)
                cardShuffe.Add(card);

            cardShuffe.Shuffle();
            foreach(Card card in cardShuffe)
            {
                if (card.effect.energy <= energy)
                {
                    cardSelected = card;

                    break;
                }
            }
            if (cardSelected == null)
            {

                yield return new WaitForSeconds(1f);
                GameEventSystem.instance.Send(EEventType.RoundEnd, new object[] { this });
                stopPlay = true;
                yield break;
            }
            if (cardSelected.effect.cardType == ECardType.Self)
            {
                entitySelected = this;
            } else
            {
                List<EntityWithId> entities = new();
                foreach(EntityWithId entity in (List<EntityWithId>)argV[2])
                {
                    entities.Add(entity);
                }

                for (int i = 0; i < entities.Count; i++)
                {
                    if (entities[i].id == playerId)
                        entities.RemoveAt(i);
                }
                entitySelected = entities[Random.Range(0,entities.Count)].entity;
            }
        
            yield return new WaitForSeconds(.5f);
            SelectCard(cardSelected);
            yield return new WaitForSeconds(1f);

            cardSelected.UseLisnable(this, entitySelected);

            yield return new WaitForSeconds(1);
            cardSelected = null;

            ResetCardPos();

            
        }

        GameEventSystem.instance.Send(EEventType.RoundEnd, new object[] { this });
    }

    public override void InitialiseEntity(object[] argV)
    {
        base.InitialiseEntity(argV);
    }

    public override void FillDeck(object[] argV)
    {
        base.FillDeck(argV);
    }

    public override void RoundBegin(object[] argV)
    {
        base.RoundBegin(argV);
        if (isPlaying)
        {
            stopPlay = false;
            StartCoroutine(EnemyAttack(argV));
        }
    }

    public override void RoundEnd(object[] argV)
    {

    }
    public override void TakeDamage(int damage, object[] argV)
    {
        base.TakeDamage(damage, argV);
    }

    public override void UpdateEnergy(int energyPoints)
    {
        base.UpdateEnergy(energyPoints);
    }

    public override void SelectCard(Card card)
    {
        base.SelectCard(card);
    }

    public override void ResetCardPos()
    {
        base.ResetCardPos(); 
    }
}
