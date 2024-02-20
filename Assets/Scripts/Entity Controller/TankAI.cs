using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAI : EntityController
{
    // private float healThresholdHP = 0.2f;


    public TankAI(Tank tank)
    {
        entity = tank;
    }

    public override void Execute()
    {
        if (entity.GetDefaultTarget().GetTauntDuration() <= 0)
        {
            ((Tank)entity).Taunt(entity.GetDefaultTarget());
        }
        else
        {
            ((Tank)entity).SingleAttackAndHeal(entity.GetDefaultTarget());
        }
    }
}
