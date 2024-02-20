using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerAI : EntityController
{
    private int AoEThresholdCount = 2;
    private float AoEThresholdPercentHP = 0.7f;
    private float SingleThresholdPercentHP = 0.5f;

    public HealerAI (Healer healer)
    {
        entity = healer;
    }

    public override void Execute()
    {
        Entity healTarget = null;
        int count = 0;

        foreach (Entity member in party.membersEntity)
        {
            if (member.GetHPPercentage() <= AoEThresholdPercentHP)
            {
                count++;
            }
            if (member.GetHPPercentage() <= SingleThresholdPercentHP)
            {
                healTarget = member;
            }
        }

        if (count >= AoEThresholdCount)
        {
            ((Healer)entity).AOEHeal(party);
        }
        else if (healTarget != null)
        {
            ((Healer)entity).SingleHeal(healTarget);
        }
        else
        {
            ((Healer)entity).BasicAttack(entity.GetDefaultTarget());
        }
    }
}
