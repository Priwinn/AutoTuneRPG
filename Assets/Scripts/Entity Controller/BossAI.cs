using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : EntityController
{
    public BossAI(Boss boss)
    {
        entity = boss;
    }

    // Get lowest HP target
    private Entity SelectTarget()
    {
        Entity target = null;
        foreach (Entity member in targetParty.membersEntity)
        {
            if (target == null)
            {
                target = member;
            }
            else if (member.GetHP() < target.GetHP())
            {
                target = member;
            }
        }
        return target;
    }

    public override void Execute()
    {
        if (Random.Range(0f, 1f) <= ((Boss)entity).bossParam.aoeProbability)
        {
            ((Boss)entity).AoEAttack(targetParty);
        }
        else if (entity.IsTaunted())
        {
            ((Boss)entity).SingleTarget(entity.GetTauntTarget());
        }
        else
        {
            ((Boss)entity).SingleTarget(SelectTarget());
        }
    }
}
