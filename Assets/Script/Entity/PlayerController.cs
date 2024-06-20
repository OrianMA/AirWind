using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : AEntity
{
    public LayerMask mask;

    bool isPlaying;
    Card cardSelected;
    AEntity entitySelected;
    PlayerInput inputs;
    public override void Start()
    {
        base.Start();
        inputs = new PlayerInput();
        inputs.Player.Click.started += Click_started;
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
                cardSelected = hit.collider.GetComponent<Card>();
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                entitySelected = hit.collider.GetComponent<AEntity>();
                if (cardSelected != null)
                    cardSelected.UseLisnable(this, entitySelected);
            }
        }
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
        isPlaying = true;
    }
    public override void TakeDamage(int damage, object[] argV)
    {
        base.TakeDamage(damage, argV);
    }

    public override void UpdateEnergy(int energyPoints)
    {
        base.UpdateEnergy(energyPoints);
    }
}
