using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurn : State
{
    public PlayerTurn(TurnSystem turnSystem): base(turnSystem)
    {
    }

    public override IEnumerator Start()
    {
        Debug.Log("Choose Action");
        foreach (Button button in TurnSystem.TurnSelect.turnOptions)
        {
            button.interactable = true;
        }
        yield break;
    }

    public override IEnumerator Attack()
    {
        Debug.Log("ATTACK!");
        TurnSystem.SetState(new EnemyTurn(TurnSystem));
        yield break;
    }

    public override IEnumerator Defend()
    {
        Debug.Log("DEFEND!");
        TurnSystem.SetState(new EnemyTurn(TurnSystem));
        yield break;
        //return base.Defend();
    }

    public override IEnumerator Heal()
    {
        Debug.Log("HEAL!");
        yield break;
        //return base.Heal();
    }
}