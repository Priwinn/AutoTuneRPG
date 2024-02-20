//Save DPS, Tank, Healer, Boss Params to JSON

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonIO : MonoBehaviour
{
    public static string[] ParamNames = new string[] { "DPS_maxHP", "DPS_attackBuff", "DPS_attackBuffDuration", "DPS_attackBuffCost", "DPS_singleDamage", "DPS_singleDamageCost", "DPS_basicDamage",
                                                        "Tank_maxHP", "Tank_tauntAttackDebuff", "Tank_tauntDuration", "Tank_tauntCost", "Tank_singleDamage", "Tank_singleHeal", "Tank_singleCost", "Tank_basicDamage",
                                                        "Healer_maxHP", "Healer_singleHeal", "Healer_singleHealCost", "Healer_aoeHeal", "Healer_aoeHealCost", "Healer_basicDamage",
                                                        "Boss_maxHP", "Boss_singleDamage", "Boss_aoeDamage", "Boss_aoeProbability" };
    public void SaveParam(int[] param, string path)
    {
        string json = "{";
        for (int i = 0; i < ParamNames.Length; i++)
        {
            json += "\"" + ParamNames[i] + "\":" + param[i];
            if (i != ParamNames.Length - 1)
            {
                json += ",";
            }
        }
        json += "}";
        File.WriteAllText(path, json);
    }

    public int[] LoadParam(string path)
    {
        string json = File.ReadAllText(path);
        int[] param = new int[ParamNames.Length];
        for (int i = 0; i < ParamNames.Length; i++)
        {
            string[] valueSplit = json.Split(':')[i + 1].Split(',');
            string value = valueSplit[0].Trim('}');
            param[i] = int.Parse(value);
        }
        return param;
    }
}
    