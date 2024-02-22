using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("DPS Settings")]
    public int DPS_maxHP_min = 1;
    public int DPS_maxHP_max = 10;
    public int DPS_attackBuff_min = 1;
    public int DPS_attackBuff_max = 10;
    public int DPS_attackBuffDuration_min = 1;
    public int DPS_attackBuffDuration_max = 10;
    public int DPS_attackBuffCost_min = 1;
    public int DPS_attackBuffCost_max = 10;
    public int DPS_singleDamage_min = 1;
    public int DPS_singleDamage_max = 10;
    public int DPS_singleDamageCost_min = 1;
    public int DPS_singleDamageCost_max = 10;
    public int DPS_basicDamage_min = 1;
    public int DPS_basicDamage_max = 10;

    [Header("Tank Settings")]
    public int Tank_maxHP_min = 1;
    public int Tank_maxHP_max = 10;
    public int Tank_tauntAttackDebuff_min = 1;
    public int Tank_tauntAttackDebuff_max = 10;
    public int Tank_tauntDuration_min = 1;
    public int Tank_tauntDuration_max = 10;
    public int Tank_tauntCost_min = 1;
    public int Tank_tauntCost_max = 10;
    public int Tank_singleDamage_min = 1;
    public int Tank_singleDamage_max = 10;
    public int Tank_singleHeal_min = 1;
    public int Tank_singleHeal_max = 10;
    public int Tank_singleCost_min = 1;
    public int Tank_singleCost_max = 10;
    public int Tank_basicDamage_min = 1;
    public int Tank_basicDamage_max = 10;

    [Header("Healer Settings")]
    public int Healer_maxHP_min = 1;
    public int Healer_maxHP_max = 10;
    public int Healer_singleHeal_min = 1;
    public int Healer_singleHeal_max = 10;
    public int Healer_singleHealCost_min = 1;
    public int Healer_singleHealCost_max = 10;
    public int Healer_aoeHeal_min = 1;
    public int Healer_aoeHeal_max = 10;
    public int Healer_aoeHealCost_min = 1;
    public int Healer_aoeHealCost_max = 10;
    public int Healer_basicDamage_min = 1;
    public int Healer_basicDamage_max = 10;

    [Header("Boss Settings")]
    public int Boss_maxHP_min = 1;
    public int Boss_maxHP_max = 10;
    public int Boss_singleDamage_min = 1;
    public int Boss_singleDamage_max = 10;
    public int Boss_aoeDamage_min = 1;
    public int Boss_aoeDamage_max = 10;
    public int Boss_aoeProbability_min = 1;
    public int Boss_aoeProbability_max = 10;

    Game game;

    void Start()
    {
        GameParams gameParams = new GameParams(GenerateIndividual());
        game = new Game(gameParams, true);
        //StartCoroutine(GameCoroutine());
        game.Run(1);
        List<float[]> stats = game.GetStatistics();
        Debug.Log("Stats: " + string.Join(", ", stats[0]));
    }

    IEnumerator GameCoroutine()
    {
        game.Run(1);
        List<float[]> stats = game.GetStatistics();
        Debug.Log("Stats: " + string.Join(", ", stats[0]));
        yield return null;
    }

    public int[] GenerateIndividual()
    {
        int[] individual = new int[25];

        individual[0] = Random.Range(DPS_maxHP_min, DPS_maxHP_max + 1);
        individual[1] = Random.Range(DPS_attackBuff_min, DPS_attackBuff_max + 1);
        individual[2] = Random.Range(DPS_attackBuffDuration_min, DPS_attackBuffDuration_max + 1);
        individual[3] = Random.Range(DPS_attackBuffCost_min, DPS_attackBuffCost_max + 1);
        individual[4] = Random.Range(DPS_singleDamage_min, DPS_singleDamage_max + 1);
        individual[5] = Random.Range(DPS_singleDamageCost_min, DPS_singleDamageCost_max + 1);
        individual[6] = Random.Range(DPS_basicDamage_min, DPS_basicDamage_max + 1);
        individual[7] = Random.Range(Tank_maxHP_min, Tank_maxHP_max + 1);
        individual[8] = Random.Range(Tank_tauntAttackDebuff_min, Tank_tauntAttackDebuff_max + 1);
        individual[9] = Random.Range(Tank_tauntDuration_min, Tank_tauntDuration_max + 1);
        individual[10] = Random.Range(Tank_tauntCost_min, Tank_tauntCost_max + 1);
        individual[11] = Random.Range(Tank_singleDamage_min, Tank_singleDamage_max + 1);
        individual[12] = Random.Range(Tank_singleHeal_min, Tank_singleHeal_max + 1);
        individual[13] = Random.Range(Tank_singleCost_min, Tank_singleCost_max + 1);
        individual[14] = Random.Range(Tank_basicDamage_min, Tank_basicDamage_max + 1);
        individual[15] = Random.Range(Healer_maxHP_min, Healer_maxHP_max + 1);
        individual[16] = Random.Range(Healer_singleHeal_min, Healer_singleHeal_max + 1);
        individual[17] = Random.Range(Healer_singleHealCost_min, Healer_singleHealCost_max + 1);
        individual[18] = Random.Range(Healer_aoeHeal_min, Healer_aoeHeal_max + 1);
        individual[19] = Random.Range(Healer_aoeHealCost_min, Healer_aoeHealCost_max + 1);
        individual[20] = Random.Range(Healer_basicDamage_min, Healer_basicDamage_max + 1);
        individual[21] = Random.Range(Boss_maxHP_min, Boss_maxHP_max + 1);
        individual[22] = Random.Range(Boss_singleDamage_min, Boss_singleDamage_max + 1);
        individual[23] = Random.Range(Boss_aoeDamage_min, Boss_aoeDamage_max + 1);
        individual[24] = Random.Range(Boss_aoeProbability_min, Boss_aoeProbability_max + 1);

        return individual;
    }
}
