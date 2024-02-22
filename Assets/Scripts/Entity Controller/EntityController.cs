using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityController
{
    protected Entity entity;
    protected Party party;
    protected Party targetParty;

    public Party GetParty()
    {
        return party;
    }

    public void SetParty(Party p)
    {
        party = p;
    }

    public Party GetTargetParty()
    {
        return targetParty;
    }

    public void SetTargetParty(Party p)
    {
        targetParty = p;
    }

    public Entity GetEntity()
    {
        return entity;
    }

    public abstract void Execute();
    public virtual void ResolveTurn()
    {
        if (entity == null)
        {
            Debug.Log(this + " entity is NULL WHAT THE FUCK AAAAAAAAAA");
            return;
        }
        entity.ResolveTurn();
    }
}
