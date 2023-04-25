using ML.Combat;
using UnityEngine;

public abstract class GuardBaseState 
{
    public abstract void EnterState(GuardAI guard);

    public abstract void UpdateState(GuardAI guard);

    public virtual void CancelState(GuardAI guard)
    {
        return;
    }

}
