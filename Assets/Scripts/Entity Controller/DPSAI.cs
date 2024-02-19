using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPSAI : EntityController
{
    public DPSAI(DPS dps)
    {
        entity = dps;
    }

    public override void Execute()
    {
        if (entity.GetAttackBuffDuration() <= 0)
        {
            ((DPS)entity).AOEAttackBuff(party);
        }
        else
        {
            ((DPS)entity).SingleDamage(entity.GetDefaultTarget());
        }
    }
}
