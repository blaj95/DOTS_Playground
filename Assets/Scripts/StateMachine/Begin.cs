using System.Collections;
using UnityEngine;

public class Begin : State
{
    public Begin(TurnSystem turnSystem) : base(turnSystem)
    {
    }

    public override IEnumerator Start()
    {
        Debug.Log("It's now the players turn");
        yield return new WaitForSeconds(2);
        TurnSystem.SetState(new PlayerTurn(TurnSystem));
    }
}