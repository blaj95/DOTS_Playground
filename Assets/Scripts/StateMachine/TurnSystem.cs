using System;
using UnityEngine;

public class TurnSystem : StateMachine
{
    public TurnSelect TurnSelect;
    
    private void Start()
    {
        SetState(new Begin(this));
    }

    public void OnAttack()
    {
        Debug.Log("Start Attack");
        StartCoroutine(State.Attack());
    }

    public void OnDefend()
    {
        Debug.Log("Start Defend");
        StartCoroutine(State.Defend());
    }

    public void OnHeal()
    {
        Debug.Log("Start Heal");
        StartCoroutine(State.Heal());
    }
    
}
