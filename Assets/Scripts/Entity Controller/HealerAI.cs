using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerAI : EntityController
{
    private int AoEThresholdCount = 2;
    private int AoEThresholdPercentHP = 70;
    private int SingleThresholdPercentHP = 50;

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
            if (member.GetPercentHP() <= AoEThresholdPercentHP)
            {
                count++;
            }
            if (member.GetPercentHP() <= SingleThresholdPercentHP)
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
