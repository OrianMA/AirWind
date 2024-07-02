using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : AEntity
{
    public LayerMask mask;

    Card cardSelected;
    AEntity entitySelected;
    PlayerInput inputs;
    public override void Start()
    {
        base.Start();
        inputs = new PlayerInput();
        inputs.Player.Click.started += Click_started;
        inputs.Player.PassRound.started += PassRound;
        inputs.Player.Enable();
    }

    private void Click_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!isPlaying)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, mask))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Card"))
            {
                Card cardClick = hit.collider.GetComponent<Card>();
                foreach (Card card in cardsInGame)
                {
                    if (cardClick == card)
                    {
                        if (card.effect.energy <= energy)
                        {
                            cardSelected = card;
                            SelectCard(card);
                        } else
                        {
                            card.transform.DOShakeRotation(.1f,45,10,0,true,ShakeRandomnessMode.Harmonic);
                        }
                    } 
                }
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                entitySelected = hit.collider.GetComponent<AEntity>();

                if (cardSelected != null)
                {
                    cardSelected.UseLisnable(this, entitySelected);
                }
            }
        }
    }

    private void PassRound(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!isPlaying)
            return;

        ResetCardPos();
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
