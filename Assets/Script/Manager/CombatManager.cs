using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public int startDeckSize;
    public int entityStartHealth;
    public int entityEnergyStart;
    public int entityMaxEnergy;
    public int energyAddEachRound;
    public int playerNumber; //sera utilisé pour chosir un player aléatoir au début

    public static CombatManager instance;

    //cache
    int currentPlayerId;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameEventSystem.instance.Listen(EEventType.CardPickup, UseCard);
    }

    public void UseCard(object[] argV)
    {
        AEntity origin = (AEntity)argV[0];
        AEntity target = (AEntity)argV[1];
        AEffect effect = (AEffect)argV[2];
        Card card = (Card)argV[3];

        if (origin.energy >= effect.energy)
        {
            effect.Use(origin,target,null);
            origin.energy -= effect.energy;
            card.DestroyCard();
        }
    }

    public void StartGame()
    {
        //currentPlayerId = Random.Range(0, startDeckSize);
        currentPlayerId = 0;
        GameEventSystem.instance.Send(EEventType.PlayBegin, new object[] {startDeckSize, entityStartHealth, entityEnergyStart, entityMaxEnergy });
        GameEventSystem.instance.Send(EEventType.RoundBegin, new object[] { currentPlayerId, energyAddEachRound });
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartGame();
        }
    }


}
