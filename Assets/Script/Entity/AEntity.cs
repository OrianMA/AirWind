using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AEntity : MonoBehaviour, IHealth
{
    public int playerId;
    public int lifePoint;
    public int energy;
    public DeckSystem deck;
    public Transform cardSpawnTransform;
    public List<Card> cardsInGame = new();
    List<Status> allStatus = new();

    [Header("Visual of card")]
    public float xOffsetSideCard;
    public float ScaleOnSelect;
    
    

    public bool isPlaying;

    int maxEnergy;
    int maxCardInGameSize;
    public List<int> cardPosIndex = new();

    public virtual void Start()
    {
        GameEventSystem.instance.Listen(EEventType.PlayBegin, InitialiseEntity);
        GameEventSystem.instance.Listen(EEventType.PlayBegin, FillDeck);
        GameEventSystem.instance.Listen(EEventType.RoundBegin, RoundBegin);
        GameEventSystem.instance.Listen(EEventType.RoundEnd, RoundEnd);
        GameEventSystem.instance.Listen(EEventType.PlayerDie, PlayerDie);
    }

    public virtual void InitialiseEntity(object[] argV)
    {
        lifePoint = (int)argV[1];
        energy = 0;
        maxEnergy = (int)argV[3];
        maxCardInGameSize = (int)argV[4];

        UpdateEnergy((int)argV[2]);
    }

    public virtual void FillDeck(object[] argV)
    {
        int startDeckSize = (int)argV[0];
        StartCoroutine(FillDeckSmooth(startDeckSize));
    }

    IEnumerator FillDeckSmooth(int startDeckSize)
    {
        for (int i = 0; i < startDeckSize; i++)
        {
            PickupCard();
            yield return new WaitForSeconds(.5f);
        }
    }

    public virtual void RoundBegin(object[] argV)
    {
        if ((int)argV[0] != playerId)
        {
            isPlaying = false;
            return;
        }

        UpdateEnergy((int)argV[1]);
        PickupCard();

        isPlaying = true;
    }

    public virtual void RoundEnd(object[] argV)
    {

    }
    public virtual void TakeDamage(int damage, object[] argV)
    {
        lifePoint -= damage;
        GameEventSystem.instance.Send(EEventType.Damage, argV);
    }

    public virtual void UpdateEnergy(int energyPoints)
    {
        energy = Mathf.Clamp(energy + energyPoints, 0, maxEnergy);
        GameEventSystem.instance.Send(EEventType.PlayerEnergy, new object[] {this});
    }

    public virtual void PickupCard()
    {
        if (cardsInGame.Count >= maxCardInGameSize)
            return;

        Card newCard = Instantiate(deck.cards[Random.Range(0, deck.cards.Count)].prefab, cardSpawnTransform);

        newCard.effect = newCard.effect.Clone();

        newCard.allCardPosRef = cardPosIndex;
        cardsInGame.Add(newCard);
        newCard.transform.position = Vector3.one * .5f;
        GameEventSystem.instance.Send(EEventType.CardPickup, new object[] { this, newCard });

        //Visual
        int indexChoose = 0;
        for (int i = 0 ; i < cardsInGame.Count; i++)
        {
            if (!cardPosIndex.Contains(i))
            {
                if (i % 2 == 0)
                {
                    newCard.transform.DOMove(cardSpawnTransform.position + cardSpawnTransform.right * (xOffsetSideCard * i / 2), 1).OnComplete(() =>
                    {
                        newCard.transform.localPosition = Vector3.right * (xOffsetSideCard * i / 2);
                    });

                    newCard.originTransformPos = cardSpawnTransform.position + cardSpawnTransform.right * (xOffsetSideCard * i / 2);
                    indexChoose = i;
                } else
                {
                    newCard.transform.DOMove(cardSpawnTransform.position + cardSpawnTransform.right * (-xOffsetSideCard * (Mathf.FloorToInt(i / 2) + 1)),1).OnComplete(() =>
                    {
                        newCard.transform.localPosition = Vector3.right * (-xOffsetSideCard * (Mathf.FloorToInt(i / 2)+1));
                    });

                    newCard.originTransformPos = cardSpawnTransform.position + cardSpawnTransform.right * (-xOffsetSideCard * (Mathf.FloorToInt(i / 2) + 1));

                    indexChoose = i;
                }
                newCard.originTransformScale = newCard.transform.localScale;
                newCard.originTransformRot = newCard.transform.rotation;

                cardPosIndex.Add(indexChoose);
                newCard.indexPos = indexChoose;



                return;
            }
        }
    }

    public virtual void SelectCard(Card card) 
    {
        ResetCardPos();
        card.transform.DOLocalMoveY(.5f, .3f);
        card.transform.DOScale(2, .3f);
    }

    public virtual void ResetCardPos()
    {
        foreach (Card allCard in cardsInGame)
        {
            allCard.ResetPos();
        }
    }

    public virtual void PlayerDie(object[] objs)
    {
        if ((int)objs[0] != playerId)
        {
            return;
        }


        foreach (Card card in cardsInGame)
        {
            Destroy(card.gameObject);
        }

        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.AddForce(-transform.forward * 6,ForceMode.Impulse);
        rb.AddTorque(transform.right * 45,ForceMode.Impulse);
        StartCoroutine(WaitAndDestroy());
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
