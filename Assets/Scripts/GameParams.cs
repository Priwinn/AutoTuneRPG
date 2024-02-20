using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Hard-coded for now with fixed party comp
public class GameParams
{
    public const int MAX_MANA = 100;
    public readonly DPSParam dpsParam; // 7 params
    public readonly TankParam tankParam;    // 8 params
    public readonly HealerParam healerParam;    // 6 params
    public readonly BossParam bossParam;    // 4 params
    public const int paramCount = 25;

    public GameParams(int[] paramsArray)
    {
        if (paramsArray.Length != paramCount)
        {
            throw new System.Exception("GIVEN GAME PARAM ARRAY LENGTH IS INCORRECT");
        }
        dpsParam = new DPSParam(paramsArray[0], MAX_MANA, paramsArray[1], paramsArray[2], paramsArray[3], paramsArray[4], paramsArray[5], paramsArray[6]);
        tankParam = new TankParam(paramsArray[7], MAX_MANA, paramsArray[8], paramsArray[9], paramsArray[10], paramsArray[11], paramsArray[12], paramsArray[13], paramsArray[14]);
        healerParam = new HealerParam(paramsArray[15], MAX_MANA, paramsArray[16], paramsArray[17], paramsArray[18], paramsArray[19], paramsArray[20]);
        bossParam = new BossParam(paramsArray[21], paramsArray[22], paramsArray[23], paramsArray[24]);
    }
}
