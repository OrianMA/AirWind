using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public int startDeckSize;
    public int entityStartHealth;
    public int entityEnergyStart;
    public int entityMaxEnergy;
    public int entityMaxCardInGameSize;
    public int energyAddEachRound;
    public int playerNumber; //sera utilisé pour chosir un player aléatoir au début
    public List<EntityWithId> playerList;
    public List<AEntity> playerToInstance;
    public Transform playerPos;
    public List<Transform> enemyPos;
    public TMPro.TextMeshPro energyAddEachRoundText;


    bool gameStart = false;

    //cache
    int currentPlayerId;
    int currentIndexEnergyAdd;
    private void Start()
    {
        GameEventSystem.instance.Listen(EEventType.CardUse, UseCard);
        GameEventSystem.instance.Listen(EEventType.Damage, EntityGetDamage);
        GameEventSystem.instance.Listen(EEventType.PlayEnd, EndGame);
        GameEventSystem.instance.Listen(EEventType.RoundEnd, ChangeRound);

        int enemyPosIndex = 0;
        int entityId = 0;
        foreach(AEntity entity in playerToInstance)
        {
            AEntity newEntity;
            if (entity.GetComponent<PlayerController>() != null)
            {
                newEntity = Instantiate(entity, playerPos);
            } else
            {
                newEntity = Instantiate(entity, enemyPos[enemyPosIndex]);
                enemyPosIndex++;
            }
            newEntity.playerId = entityId;

            EntityWithId entityWithId = new EntityWithId();
            entityWithId.id = newEntity.playerId;
            entityWithId.entity = newEntity;

            playerList.Add(entityWithId);

            entityId++;
        }
    }

    public void UseCard(object[] argV)
    {
        AEntity origin = (AEntity)argV[0];
        AEntity target = (AEntity)argV[1];
        AEffect effect = (AEffect)argV[2];
        Card card = (Card)argV[3];

        if (origin.energy >= effect.energy)
        {
            effect.Use(new object[] { origin, target, null });
            origin.UpdateEnergy(-effect.energy);
            GameEventSystem.instance.Send(EEventType.CardDestroy, new object[] { origin, target, effect, card, origin.cardPosIndex });
        }
    }

    public void StartGame()
    {
        if (!gameStart)
        {
            currentIndexEnergyAdd = playerList.Count;
            currentPlayerId = 0;
            GameEventSystem.instance.Send(EEventType.PlayBegin, new object[] {startDeckSize, entityStartHealth, entityEnergyStart, entityMaxEnergy, entityMaxCardInGameSize });
            StartCoroutine(WaitBoardBegin());
        }
    }

    IEnumerator WaitBoardBegin()
    {
        yield return new WaitForSeconds(2);
        GameEventSystem.instance.Send(EEventType.RoundBegin, new object[] { currentPlayerId, energyAddEachRound, playerList, playerList[currentPlayerId].entity });
        gameStart = true;
    }

    public void ChangeRound(object[] objs)
    {
        if (playerList.Count <= 1)
            return;

        currentIndexEnergyAdd--;
        if (currentIndexEnergyAdd <= 0)
        {
            energyAddEachRound++;
            currentIndexEnergyAdd = playerList.Count;
        }
        energyAddEachRoundText.text = energyAddEachRound.ToString();

        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].id == currentPlayerId)
            {
                currentPlayerId = playerList[(i+1)%playerList.Count].id;
                GameEventSystem.instance.Send(EEventType.RoundBegin, new object[] { currentPlayerId, energyAddEachRound, playerList, playerList[(i + 1) % playerList.Count].entity });
                return;
            }
        }
    }
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartGame();
        }
    }

    public AEntity GetEntity(int playerId)
    {
        foreach(EntityWithId entity in playerList)
        {
            if (entity.id == playerId)
            {
                return entity.entity;
            }
        }
        return null;
    }

    public void EntityGetDamage(object[] objs)
    {
        AEntity origin = (AEntity)objs[1];

        if (origin.lifePoint <= 0)
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                if (playerList[i].id == origin.playerId)
                {
                    playerList.RemoveAt(i);
                    GameEventSystem.instance.Send(EEventType.PlayerDie, new object[] { origin.playerId });
                }
            }


            if (playerList.Count <= 1) {
                GameEventSystem.instance.Send(EEventType.PlayEnd, new object[] { playerList });
            }
        }
    }

    public void EndGame(object[] objs)
    {
        if (playerList.Count > 0)
        {
            print("END GAME, winner is : " + playerList[0].entity.name);
            foreach (EntityWithId en in playerList)
            {
                en.entity.isPlaying = false;
            }
        } else
        {
            print("Equality !!!");
        }

    }
}


[System.Serializable]
public class EntityWithId
{
    public int id;
    public AEntity entity;
}