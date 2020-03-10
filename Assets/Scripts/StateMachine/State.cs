using System.Collections;

public abstract class State
{
    protected TurnSystem TurnSystem;

    public State(TurnSystem turnSystem)
    {
        TurnSystem = turnSystem;
    }
    
    public virtual IEnumerator Start()
    {
        yield break;
    }

    public virtual IEnumerator Attack()
    {
        yield break;
    }

    public virtual IEnumerator Defend()
    {
        yield break;
    } 

    public virtual IEnumerator Heal()
    {
        yield break;
    } 
}  