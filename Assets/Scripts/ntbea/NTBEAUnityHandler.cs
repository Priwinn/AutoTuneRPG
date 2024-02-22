using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NTBEAUnityHandler : MonoBehaviour
{
        [Header("Evolution Settings")]
    // Population size
    public int populationSize = 100;
    // Number of generations
    public int numGenerations = 100;
    // Mutation rate
    public float mutationRate = 0.01f;
    public int crossover_type=0; //0 for uniform crossover, 1 for n point crossover
    public int selection_type=0; //0 for roulette wheel selection, >0 for tournament selection
    public int PlayerLoseWeight=1;
    public int PlayerDeathWeight=1;
    public int HPWeight=1;
    public int ManaWeight=1;
    public int RoundWeight=1;
    public int targetRounds=15;
    public int baseFitness=10000;
    public int runsPerIndividual=10;


    [Header("DPS Settings")]
    public int DPS_maxHP_min=1;
    public int DPS_maxHP_max=10;
    public int DPS_attackBuff_min=1;
    public int DPS_attackBuff_max=10;
    public int DPS_attackBuffDuration_min=1;
    public int DPS_attackBuffDuration_max=10;
    public int DPS_attackBuffCost_min=1;
    public int DPS_attackBuffCost_max=10;
    public int DPS_singleDamage_min=1;
    public int DPS_singleDamage_max=10;
    public int DPS_singleDamageCost_min=1;
    public int DPS_singleDamageCost_max=10;
    public int DPS_basicDamage_min=1;
    public int DPS_basicDamage_max=10;

    [Header("Tank Settings")]
    public int Tank_maxHP_min=1;
    public int Tank_maxHP_max=10;
    public int Tank_tauntAttackDebuff_min=1;
    public int Tank_tauntAttackDebuff_max=10;
    public int Tank_tauntDuration_min=1;
    public int Tank_tauntDuration_max=10;
    public int Tank_tauntCost_min=1;
    public int Tank_tauntCost_max=10;
    public int Tank_singleDamage_min=1;
    public int Tank_singleDamage_max=10;
    public int Tank_singleHeal_min=1;
    public int Tank_singleHeal_max=10;
    public int Tank_singleCost_min=1;
    public int Tank_singleCost_max=10;
    public int Tank_basicDamage_min=1;
    public int Tank_basicDamage_max=10;

    [Header("Healer Settings")]
    public int Healer_maxHP_min=1;
    public int Healer_maxHP_max=10;
    public int Healer_singleHeal_min=1;
    public int Healer_singleHeal_max=10;
    public int Healer_singleHealCost_min=1;
    public int Healer_singleHealCost_max=10;
    public int Healer_aoeHeal_min=1;
    public int Healer_aoeHeal_max=10;
    public int Healer_aoeHealCost_min=1;
    public int Healer_aoeHealCost_max=10;
    public int Healer_basicDamage_min=1;
    public int Healer_basicDamage_max=10;

    [Header("Boss Settings")]
    public int Boss_maxHP_min=1;
    public int Boss_maxHP_max=10;
    public int Boss_singleDamage_min=1;
    public int Boss_singleDamage_max=10;
    public int Boss_aoeDamage_min=1;
    public int Boss_aoeDamage_max=10;
    public int Boss_aoeProbability_min=1;
    public int Boss_aoeProbability_max=10;
    
    private int[] ParamMins;
    private int[] ParamMaxs;
    private JsonIO jsonIO = new JsonIO();
    private NTupleEvolutionaryAlgorithm ntbea;
    private NTupleLandscape landscape;
    private SearchSpace searchSpace;
    private Evaluator evaluator;
    private Mutator mutator;

    private string[] ParamNames = new string[] { "DPS_maxHP", "DPS_attackBuff", "DPS_attackBuffDuration", "DPS_attackBuffCost", "DPS_singleDamage", "DPS_singleDamageCost", "DPS_basicDamage",
                                                        "Tank_maxHP", "Tank_tauntAttackDebuff", "Tank_tauntDuration", "Tank_tauntCost", "Tank_singleDamage", "Tank_singleHeal", "Tank_singleCost", "Tank_basicDamage",
                                                        "Healer_maxHP", "Healer_singleHeal", "Healer_singleHealCost", "Healer_aoeHeal", "Healer_aoeHealCost", "Healer_basicDamage",
                                                        "Boss_maxHP", "Boss_singleDamage", "Boss_aoeDamage", "Boss_aoeProbability" };    private List<int[]> population; // Old population
    private string[] FitnessNames = new string[] { "Fitness", "HPFitness", "ManaFitness", "DeathPenalty", "RoundPenalty", "LosePenalty" };
    public void UpdateRanges()
    {
        ParamMins = new int[] { DPS_maxHP_min, DPS_attackBuff_min, DPS_attackBuffDuration_min, DPS_attackBuffCost_min, DPS_singleDamage_min, DPS_singleDamageCost_min, DPS_basicDamage_min,
                                Tank_maxHP_min, Tank_tauntAttackDebuff_min, Tank_tauntDuration_min, Tank_tauntCost_min, Tank_singleDamage_min, Tank_singleHeal_min, Tank_singleCost_min, Tank_basicDamage_min,
                                Healer_maxHP_min, Healer_singleHeal_min, Healer_singleHealCost_min, Healer_aoeHeal_min, Healer_aoeHealCost_min, Healer_basicDamage_min,
                                Boss_maxHP_min, Boss_singleDamage_min, Boss_aoeDamage_min, Boss_aoeProbability_min };
        ParamMaxs = new int[] { DPS_maxHP_max, DPS_attackBuff_max, DPS_attackBuffDuration_max, DPS_attackBuffCost_max, DPS_singleDamage_max, DPS_singleDamageCost_max, DPS_basicDamage_max,
                                Tank_maxHP_max, Tank_tauntAttackDebuff_max, Tank_tauntDuration_max, Tank_tauntCost_max, Tank_singleDamage_max, Tank_singleHeal_max, Tank_singleCost_max, Tank_basicDamage_max,
                                Healer_maxHP_max, Healer_singleHeal_max, Healer_singleHealCost_max, Healer_aoeHeal_max, Healer_aoeHealCost_max, Healer_basicDamage_max,
                                Boss_maxHP_max, Boss_singleDamage_max, Boss_aoeDamage_max, Boss_aoeProbability_max };
    }

    private void UpdateParams()
    {
        UpdateRanges();

        int[][] validParam = new int[ParamMins.Length][];
        for (int i = 0; i < ParamMins.Length; i++)
        {
            //Discretise the parameter space
            validParam[i] = Enumerable.Range(ParamMins[i], ParamMaxs[i] - ParamMins[i] + 1).ToArray();
            Debug.Log("validParam["+i+"]: " + string.Join(", ", validParam[i]));
        }
        evaluator = new Evaluator(runsPerIndividual, HPWeight, ManaWeight, PlayerDeathWeight, PlayerLoseWeight, baseFitness, targetRounds);
        searchSpace = new SearchSpace(validParam);
        landscape = new NTupleLandscape(searchSpace);
        mutator = new Mutator(searchSpace, false, false, mutationRate, true);
        ntbea = new NTupleEvolutionaryAlgorithm(landscape, evaluator, searchSpace, mutator, 2, 1, 5);
    }
    public void Run(){
        UpdateParams();
        ntbea.Run(populationSize);
    }
}
