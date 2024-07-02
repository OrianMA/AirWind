using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public List<GameObject> parentPlayersUi;
    public List<Slider> sliders;
    public List<TMPro.TextMeshProUGUI> lifeTexts;
    public List<TMPro.TextMeshProUGUI> energyTexts;

    int maxLifePoint;
    int maxEnergy;

    public void Start()
    {
        GameEventSystem.instance.Listen(EEventType.PlayBegin, InitUI);
        GameEventSystem.instance.Listen(EEventType.Damage, EntityUpdateHealthUi);
        //GameEventSystem.instance.Listen(EEventType.CardUse, EntityUpdateEnergyUi);
        //GameEventSystem.instance.Listen(EEventType.RoundBegin, EntityUpdateRound);
        GameEventSystem.instance.Listen(EEventType.PlayEnd, EndGameUi);
        GameEventSystem.instance.Listen(EEventType.PlayerEnergy, EntityUpdateEnergyUi);
    }


    //startDeckSize, entityStartHealth, entityEnergyStart, entityMaxEnergy, entityMaxCardInGameSize
    void InitUI(object[] objs)
    {
        maxLifePoint = (int)objs[1];
        int energyStart = (int)objs[2];
        maxEnergy = (int)objs[3];

        foreach (GameObject obj in parentPlayersUi)
        {
            obj.SetActive(true);
        }

        foreach (Slider slider in sliders)
        {
            slider.value = 1;
        }

        foreach (TMPro.TextMeshProUGUI lifeText in lifeTexts)
        {
            lifeText.text = $"{maxLifePoint} : {maxLifePoint}";
        }

        foreach (TMPro.TextMeshProUGUI energyText in energyTexts)
        {
            energyText.text = $"{energyStart} : {maxEnergy}";
        }
    }


    //origin, target
    void EntityUpdateHealthUi(object[] objs)
    {
        AEntity target = (AEntity)objs[1];

        if (target == null || target.lifePoint <= 0)
        {
            parentPlayersUi[target.playerId].SetActive(false);
        } else
        {
            SetTextLifePoint(target);
        }
    }

    //origin, target, effect, card, origin.cardPosIndex
    void EntityUpdateEnergyUi(object[] objs)
    {
        AEntity origin = (AEntity)objs[0];
        SetTextEnergy(origin.playerId, origin.energy);
    }


    //currentPlayerId, energyAddEachRound, playerList, currentPlayer
    void EntityUpdateRound(object[] objs)
    {
        AEntity currentPlayer = (AEntity)objs[3];
        int energyAddEachRound = (int)objs[1];
        SetTextEnergy(currentPlayer.playerId, currentPlayer.energy + energyAddEachRound);
    }


    void SetTextLifePoint(AEntity entity)
    {
        lifeTexts[entity.playerId].text = $"{entity.lifePoint} / {maxLifePoint}";
        sliders[entity.playerId].value = (float)entity.lifePoint / (float)maxLifePoint;
    }

    void SetTextEnergy(int entityIndex, int energy)
    {
        energyTexts[entityIndex].text = $"{energy} / {maxEnergy}";
    }

    void EndGameUi(object[] objs)
    {
        foreach (GameObject go in parentPlayersUi)
        {
            go.SetActive(false);
        }
    } 
}
