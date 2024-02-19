using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Array = System.Array;


public class Evolution : MonoBehaviour
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

    [Header("DPS Settings")]
    public int DPS_BA_min=1;
    public int DPS_BA_max=10;
    public int DPS_BuffAmount_min=1;
    public int DPS_BuffAmount_max=10;
    public int DPS_BuffTurns_min=1;
    public int DPS_BuffTurns_max=10;
    public int DPS_BuffMana_min=1;
    public int DPS_BuffMana_max=10;
    public int DPS_SpecialAttack_min=1;
    public int DPS_SpecialAttack_max=10;
    public int DPS_SpecialAttackMana_min=1;
    public int DPS_SpecialAttackMana_max=10;
    public int DPS_Health_min=1;
    public int DPS_Health_max=10;

    [Header("Healer Settings")]
    public int Healer_BA_min=1;
    public int Healer_BA_max=10;
    public int Healer_SingleHeal_min=1;
    public int Healer_SingleHeal_max=10;
    public int Healer_AOEHeal_min=1;
    public int Healer_AOEHeal_max=10;
    public int Healer_SingleHealMana_min=1;
    public int Healer_SingleHealMana_max=10;
    public int Healer_AOEHealMana_min=1;
    public int Healer_AOEHealMana_max=10;
    public int Healer_Health_min=1;
    public int Healer_Health_max=10;

    [Header("Tank Settings")]
    public int Tank_BA_min=1;
    public int Tank_BA_max=10;
    public int Tank_TauntDMGReduction_min=1;
    public int Tank_TauntDMGReduction_max=10;
    public int Tank_TauntDuration_min=1;
    public int Tank_TauntDuration_max=10;
    public int Tank_TauntMana_min=1;
    public int Tank_TauntMana_max=10;
    public int Tank_SpecialDMG_min=1;
    public int Tank_SpecialDMG_max=10;
    public int Tank_SpecialHeal_min=1;
    public int Tank_SpecialHeal_max=10;
    public int Tank_SpecialMana_min=1;
    public int Tank_SpecialMana_max=10;
    public int Tank_Health_min=1;
    public int Tank_Health_max=10;

    [Header("Boss Settings")]
    public int Boss_Single_min=1;
    public int Boss_Single_max=10;
    public int Boss_AOE_min=1;
    public int Boss_AOE_max=10;
    public int Boss_SingleProb_min=1;
    public int Boss_SingleProb_max=10;
    
    private int[] ParamMins;
    private int[] ParamMaxs;

    private string[] ParamNames = new string[] {"DPS_BA", "DPS_BuffAmount", "DPS_BuffTurns", "DPS_BuffMana", "DPS_SpecialAttack", "DPS_SpecialAttackMana", "DPS_Health", "Healer_BA", "Healer_SingleHeal", "Healer_AOEHeal", "Healer_SingleHealMana", "Healer_AOEHealMana", "Healer_Health", "Tank_BA", "Tank_TauntDMGReduction", "Tank_TauntDuration", "Tank_TauntMana", "Tank_SpecialDMG", "Tank_SpecialHeal", "Tank_SpecialMana", "Tank_Health", "Boss_Single", "Boss_AOE", "Boss_SingleProb"};
    private List<int[]> population; // Old population
    private List<int> fitPopulation; // Old population fitness scores
    private int generationFitness; // Total fitness for old population
    private int[] elite; // The max fitness individual in old generation

    
    // Generate random individuals within the parameter ranges
    public int[] GenerateIndividual()
    {
        int[] individual = new int[24];

        individual[0] = Random.Range(DPS_BA_min, DPS_BA_max + 1);
        individual[1] = Random.Range(DPS_BuffAmount_min, DPS_BuffAmount_max + 1);
        individual[2] = Random.Range(DPS_BuffTurns_min, DPS_BuffTurns_max + 1);
        individual[3] = Random.Range(DPS_BuffMana_min, DPS_BuffMana_max + 1);
        individual[4] = Random.Range(DPS_SpecialAttack_min, DPS_SpecialAttack_max + 1);
        individual[5] = Random.Range(DPS_SpecialAttackMana_min, DPS_SpecialAttackMana_max + 1);
        individual[6] = Random.Range(DPS_Health_min, DPS_Health_max + 1);
        individual[7] = Random.Range(Healer_BA_min, Healer_BA_max + 1);
        individual[8] = Random.Range(Healer_SingleHeal_min, Healer_SingleHeal_max + 1);
        individual[9] = Random.Range(Healer_AOEHeal_min, Healer_AOEHeal_max + 1);
        individual[10] = Random.Range(Healer_SingleHealMana_min, Healer_SingleHealMana_max + 1);
        individual[11] = Random.Range(Healer_AOEHealMana_min, Healer_AOEHealMana_max + 1);
        individual[12] = Random.Range(Healer_Health_min, Healer_Health_max + 1);
        individual[13] = Random.Range(Tank_BA_min, Tank_BA_max + 1);
        individual[14] = Random.Range(Tank_TauntDMGReduction_min, Tank_TauntDMGReduction_max + 1);
        individual[15] = Random.Range(Tank_TauntDuration_min, Tank_TauntDuration_max + 1);
        individual[16] = Random.Range(Tank_TauntMana_min, Tank_TauntMana_max + 1);
        individual[17] = Random.Range(Tank_SpecialDMG_min, Tank_SpecialDMG_max + 1);
        individual[18] = Random.Range(Tank_SpecialHeal_min, Tank_SpecialHeal_max + 1);
        individual[19] = Random.Range(Tank_SpecialMana_min, Tank_SpecialMana_max + 1);
        individual[20] = Random.Range(Tank_Health_min, Tank_Health_max + 1);
        individual[21] = Random.Range(Boss_Single_min, Boss_Single_max + 1);
        individual[22] = Random.Range(Boss_AOE_min, Boss_AOE_max + 1);
        individual[23] = Random.Range(Boss_SingleProb_min, Boss_SingleProb_max + 1);

        return individual;
    }

    // Initialize the population
    void InitializePopulation()
    {
        population = new List<int[]>(populationSize);

        for (int i = 0; i < populationSize; i++)
        {
            // Instantiate a new individual and set the individual's DNA randomly
            int[] individual = GenerateIndividual();
            // 
            // Add the individual to the population
            population.Add(individual);
        }
    }

    public void UpdateRanges()
    {
        ParamMins = new int[] {DPS_BA_min, DPS_BuffAmount_min, DPS_BuffTurns_min, DPS_BuffMana_min, DPS_SpecialAttack_min, DPS_SpecialAttackMana_min, DPS_Health_min, Healer_BA_min, Healer_SingleHeal_min, Healer_AOEHeal_min, Healer_SingleHealMana_min, Healer_AOEHealMana_min, Healer_Health_min, Tank_BA_min, Tank_TauntDMGReduction_min, Tank_TauntDuration_min, Tank_TauntMana_min, Tank_SpecialDMG_min, Tank_SpecialHeal_min, Tank_SpecialMana_min, Tank_Health_min, Boss_Single_min, Boss_AOE_min, Boss_SingleProb_min};
        ParamMaxs = new int[] {DPS_BA_max, DPS_BuffAmount_max, DPS_BuffTurns_max, DPS_BuffMana_max, DPS_SpecialAttack_max, DPS_SpecialAttackMana_max, DPS_Health_max, Healer_BA_max, Healer_SingleHeal_max, Healer_AOEHeal_max, Healer_SingleHealMana_max, Healer_AOEHealMana_max, Healer_Health_max, Tank_BA_max, Tank_TauntDMGReduction_max, Tank_TauntDuration_max, Tank_TauntMana_max, Tank_SpecialDMG_max, Tank_SpecialHeal_max, Tank_SpecialMana_max, Tank_Health_max, Boss_Single_max, Boss_AOE_max, Boss_SingleProb_max};
    }

    // Evolution process
    public void Evolve()
    {
        InitializePopulation();
        UpdateRanges();
        EvaluateFitness();
        int bestFitness = 0;
        for (int generation = 0; generation < numGenerations; generation++)
        {
            // Create a new population
            List<int[]> newPopulation = new List<int[]>(populationSize);

            // Elitism: Keep the best individual from the previous generation
            if (elite != null)
            {
                newPopulation.Add(elite);
            }
            // Generate the rest of the population through crossover and mutation
            for (int i = 1; i < populationSize; i++)
            {
                // Select parents for crossover
                int[] parent1 = SelectParent();
                int[] parent2 = SelectParent();

                // Perform crossover to create a child
                int[] childDNA = Crossover(parent1, parent2);

                // Mutate the child
                childDNA = Mutate(childDNA);

                // Add the child to the new population
                newPopulation.Add(childDNA);
            }

            // Replace the old population with the new population
            population = newPopulation;
            // Evaluate fitness of each individual
            bestFitness = EvaluateFitness();


        }
        Debug.Log("Evolution complete");
        Debug.Log("Best individual: " + bestFitness);
        Debug.Log("Elite: " + string.Join(", ", elite));
    }

    // Evaluate the fitness of each individual
    int EvaluateFitness()
    {
        int bestFitness = 0;
        fitPopulation = new List<int>(populationSize);
        foreach (int[] individual in population)
        {
            int fitness = FitnessFunction(individual);
            fitPopulation.Add(fitness);
            if (fitness > bestFitness)
            {
                bestFitness = fitness;
                elite = individual;
            }
        }
        return bestFitness;
    }

    int FitnessFunction(int[] individual)
    {
        //TODO define fitness function
        //Use sum of all params as fitness
        int fitness = 0;
        for (int i = 0; i < ParamNames.Length; i++)
        {
            fitness += individual[i];
        }
        return fitness;
    }

    // Select a parent for crossover
    int[] SelectParent(int type=0)
    {
        // Select a parent based on the selection type
        // 0 for roulette wheel selection
        // >0 for tournament selection
        // Note 1 is random selection
        if (selection_type==0)
        {
            return RouletteSelection();
        } else
        {
            return TournamentSelection(type);
        }
    }
    // Roulette wheel selection
    int[] RouletteSelection()
    {
        float totalFitness = 0f;

        for (int individual = 0; individual < populationSize; individual++)
        {
            totalFitness += fitPopulation[individual];
        }

        float randomValue = Random.Range(0f, totalFitness);
        float sum = 0f;

        for (int individual = 0; individual < populationSize; individual++)
        {
            sum += fitPopulation[individual];

            if (sum >= randomValue)
            {
                return population[individual];
            }
        }
        return null;
    }
    //Tourament selection
    int[] TournamentSelection(int k)
    {
        int best=0;
        int bestFitness=0;
        for (int i = 0; i < k; i++)
        {
            
            int index = Random.Range(0, populationSize);
            int fitness = fitPopulation[index];
            if (fitness>bestFitness)
            {
                best=index;
                bestFitness=fitness;
            }
        }
        return population[best];
    }

    // Perform crossover between two parents
    int[] Crossover(int[] parent1DNA, int[] parent2DNA)
    {
        int[] childDNA = new int[ParamNames.Length];

        // Perform crossover of DNA
        //uniform crossover
        if (crossover_type==0)
        {
            for (int i = 0; i < ParamNames.Length; i++)
            {
                if (Random.value < 0.5)
                {
                    childDNA[i] = parent1DNA[i];
                }
                else
                {
                    childDNA[i] = parent2DNA[i];
                }
            }
        }
        //n point crossover
        else
        {
            // Find n non repeating random points in the DNA
            int[] points = new int[crossover_type];
            for (int i = 0; i < crossover_type; i++)
            {
                int point = Random.Range(0, ParamNames.Length);
                // Ensure points are unique
                while (points.Contains(point))
                {
                    point = Random.Range(0, ParamNames.Length);
                }
                points[i] = point;
            }
            Array.Sort(points);
            // Perform crossover
            int j = 0;
            for (int i = 0; i < ParamNames.Length; i++)
            {
                if (j < points.Length && i == points[j])
                {
                    j++;
                }

                if (j % 2 == 0)
                {
                    childDNA[i] = parent1DNA[i];
                }
                else
                {
                    childDNA[i] = parent2DNA[i];
                }
            }
        // Debug.Log("Child DNA: " + string.Join(", ", childDNA));
        // Debug.Log("Parent 1 DNA: " + string.Join(", ", parent1DNA));
        // Debug.Log("Parent 2 DNA: " + string.Join(", ", parent2DNA));
        // Debug.Log("Points: " + string.Join(", ", points));
            
        }

        return childDNA;
    }

    // Mutate an individual
    int[] Mutate(int[] genotype)
    {
        // Mutate each gene in the DNA
        for (int i = 0; i < ParamNames.Length; i++)
        {
            if (Random.value < mutationRate)
            {
                genotype[i] = Random.Range(ParamMins[i], ParamMaxs[i] + 1);
            }
        }
        return genotype;
    }
}
