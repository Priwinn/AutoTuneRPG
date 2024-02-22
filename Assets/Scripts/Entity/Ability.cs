using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    public string abilityName;

    public int manaCost = 0;
    public int hpCost = 0;

    public bool buffAoE = false;
    public bool buffAll = false;
    public bool debuffAoE = false;
    public bool debuffAll = false;

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

    public int attackBuff = 0;

    public int attackDebuff = 0;

    public int defenseBuff = 0;

    public int defenseDebuff = 0;

    public bool taunt = false;
    public bool tauntAoE = false;
    public int tauntExtraDamage = 0;

    public bool stun = false;
    public bool stunAoE = false;
    public int stunExtraDamage = 0;

    public int duration = 0;
    //TODO: duration works with damage, heal, mana recovery
}
