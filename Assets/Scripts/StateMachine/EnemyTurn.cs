using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTurn : State 
{
    public EnemyTurn(TurnSystem turnSystem) : base(turnSystem)
    {
    }

    public override IEnumerator Start()
    {
        foreach (Button button in TurnSystem.TurnSelect.turnOptions)
        {
            button.interactable = false;
        }
        
        Debug.Log("Enemies Turn!");
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Enemy Attacks!");
        yield return new WaitForSeconds(1.5f);
        TurnSystem.SetState(new PlayerTurn(TurnSystem));
    }
}