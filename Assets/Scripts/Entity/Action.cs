using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    public int manaCost = 0;
    public int hpCost = 0;

    // TODO consider fewer AoE variable? like this
    // public bool debuffAoE = false;   // debuff include stun and taunt
    // public bool buffAoE = false;     // buff include heal and mana recovery

    public int damage = 0;
    public bool damageAoE = false;
    public bool damageAll = false;
    public int hitCount = 0;

    public int heal = 0;
    public bool healAoE = false;

    //TODO implement heal debuff for healing and getting healed
    public int healDebuff = 0;

    public int manaRecover = 0;
    public bool manaRecoverAoE = false;

    //TODO
    public int hitCountBuff = 0;
    public bool hitCountBuffAoE = false;
    public bool hitCountBuffAll = false;

    public int attackBuff = 0;
    public bool attackBuffAoE = false;
    public bool attackBuffAll = false;

    public int attackDebuff = 0;
    public bool attackDebuffAoE = false;
    public bool attackDebuffAll = false;

    public int defenseBuff = 0;
    public bool defenseBuffAoE = false;
    public bool defenseBuffAll = false;

    public int defenseDebuff = 0;
    public bool defenseDebuffAoE = false;
    public bool defenseDebuffAll = false;

    public bool taunt = false;
    public bool tauntAoE = false;
    public int tauntExtraDamage = 0;

    public bool stun = false;
    public bool stunAoE = false;
    public int stunExtraDamage = 0;

    public int duration = 0;
    //TODO: duration works with damage, heal, mana recovery
}
