using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEntity : MonoBehaviour, IHealth
{
    public int playerId;
    public int lifePoint;
    public int energy;
    public DeckSystem deck;
    public Transform cardSpawnTransform;
    public List<Card> cardsInGame = new();
    List<Status> allStatus = new();

    int maxEnergy;

    public virtual void Start()
    {
        GameEventSystem.instance.Listen(EEventType.PlayBegin, FillDeck);
        GameEventSystem.instance.Listen(EEventType.PlayBegin, InitialiseEntity);
        GameEventSystem.instance.Listen(EEventType.RoundBegin, RoundBegin);
    }

    public virtual void InitialiseEntity(object[] argV)
    {
        lifePoint = (int)argV[1];
        energy = 0;
        maxEnergy = (int)argV[3];

        UpdateEnergy((int)argV[2]);
    }

    public virtual void FillDeck(object[] argV)
    {
        int startDeckSize = (int)argV[0];
        for (int i = 0; i < startDeckSize; i++)
        {
            cardsInGame.Add(Instantiate(deck.cards[Random.Range(0, deck.cards.Count)].prefab, cardSpawnTransform));
        }
    }

    public virtual void RoundBegin(object[] argV)
    {
        if ((int)argV[0] != playerId)
            return;

        UpdateEnergy((int)argV[1]);
    }
    public virtual void TakeDamage(int damage, object[] argV)
    {
        lifePoint -= damage;
        GameEventSystem.instance.Send(EEventType.Damage, argV);
    }

    public virtual void UpdateEnergy(int energyPoints)
    {
        energy += Mathf.Clamp(energyPoints, 0, maxEnergy);
    }
}
