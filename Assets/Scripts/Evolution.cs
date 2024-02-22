using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Array = System.Array;
using System.Threading;



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

    private string[] ParamNames = new string[] { "DPS_maxHP", "DPS_attackBuff", "DPS_attackBuffDuration", "DPS_attackBuffCost", "DPS_singleDamage", "DPS_singleDamageCost", "DPS_basicDamage",
                                                        "Tank_maxHP", "Tank_tauntAttackDebuff", "Tank_tauntDuration", "Tank_tauntCost", "Tank_singleDamage", "Tank_singleHeal", "Tank_singleCost", "Tank_basicDamage",
                                                        "Healer_maxHP", "Healer_singleHeal", "Healer_singleHealCost", "Healer_aoeHeal", "Healer_aoeHealCost", "Healer_basicDamage",
                                                        "Boss_maxHP", "Boss_singleDamage", "Boss_aoeDamage", "Boss_aoeProbability" };    private List<int[]> population; // Old population
    private string[] FitnessNames = new string[] { "Fitness", "HPFitness", "ManaFitness", "DeathPenalty", "RoundPenalty", "LosePenalty" };
    private List<float> fitPopulation; // Old population fitness scores
    private int generationFitness; // Total fitness for old population
    private int[] elite; // The max fitness individual in old generation

    //Play one game
    public void PlayGame(int[] individual)
    {
        Debug.Log("Individual: " + string.Join(", ", individual));
        GameParams Params = new GameParams(individual);
        Game game = new Game(Params,true);
        game.Run(1);
        List<float[]> stats = game.GetStatistics();
        Debug.Log("Stats: " + string.Join(", ", stats[0]));
    }

    // Generate random individuals within the parameter ranges
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
        ParamMins = new int[] { DPS_maxHP_min, DPS_attackBuff_min, DPS_attackBuffDuration_min, DPS_attackBuffCost_min, DPS_singleDamage_min, DPS_singleDamageCost_min, DPS_basicDamage_min,
                                Tank_maxHP_min, Tank_tauntAttackDebuff_min, Tank_tauntDuration_min, Tank_tauntCost_min, Tank_singleDamage_min, Tank_singleHeal_min, Tank_singleCost_min, Tank_basicDamage_min,
                                Healer_maxHP_min, Healer_singleHeal_min, Healer_singleHealCost_min, Healer_aoeHeal_min, Healer_aoeHealCost_min, Healer_basicDamage_min,
                                Boss_maxHP_min, Boss_singleDamage_min, Boss_aoeDamage_min, Boss_aoeProbability_min };
        ParamMaxs = new int[] { DPS_maxHP_max, DPS_attackBuff_max, DPS_attackBuffDuration_max, DPS_attackBuffCost_max, DPS_singleDamage_max, DPS_singleDamageCost_max, DPS_basicDamage_max,
                                Tank_maxHP_max, Tank_tauntAttackDebuff_max, Tank_tauntDuration_max, Tank_tauntCost_max, Tank_singleDamage_max, Tank_singleHeal_max, Tank_singleCost_max, Tank_basicDamage_max,
                                Healer_maxHP_max, Healer_singleHeal_max, Healer_singleHealCost_max, Healer_aoeHeal_max, Healer_aoeHealCost_max, Healer_basicDamage_max,
                                Boss_maxHP_max, Boss_singleDamage_max, Boss_aoeDamage_max, Boss_aoeProbability_max };
    }

    // Evolution process
    public void Evolve()
    {
        InitializePopulation();
        UpdateRanges();
        EvaluateFitness();
        float[] bestFitness = new float[] {0,0,0,0,0,0};
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
        PlayGame(elite);
        string fitnessString = "Best Fitness: ";
        for (int i = 0; i < bestFitness.Length; i++)
        {
            fitnessString += FitnessNames[i] + ": " + bestFitness[i] + ", ";
        }
        Debug.Log(fitnessString);
        string paramString = "Elite: ";
        for (int i = 0; i < ParamNames.Length; i++)
        {
            paramString += ParamNames[i] + ": " + elite[i] + ", ";
        }
        Debug.Log(paramString);
        
        // SaveIndividual(elite,"Assets/Scripts/Entity/EliteParam.json");
        // elite=LoadIndividual("Assets/Scripts/Entity/EliteParam.json");
        // Debug.Log("Loaded Elite: " + string.Join(", ", elite));
    }

    // Evaluate the fitness of each individual
    float[] EvaluateFitness()
    {
        float[] bestFitness = new float[] {0,0,0,0,0,0};
        fitPopulation = new List<float>(populationSize);
        foreach (int[] individual in population)
        {
            GameParams Params = new GameParams(individual);
            Game game = new Game(Params);
            game.Run(runsPerIndividual);
            List<float[]> stats = game.GetStatistics();
            float[] fitnessList = FitnessFunction(stats);
            float fitness = fitnessList[0];
            fitPopulation.Add(fitness);
            if (fitness >= bestFitness[0])
            {
                bestFitness = fitnessList;
                elite = individual;
            }
        }
        return bestFitness;
    }

    float[] FitnessFunction(List<float[]> stats)
    {
        //TODO define fitness function
        //Use sum of all params as fitness
        float fitness = 0f;
        float hpFitness = 0f;
        float manaFitness = 0f;
        float deathPenalty = 0f;
        float roundPenalty = 0f;
        float losePenalty = 0f;

        for (int i = 0; i < stats.Count; i++)
        {
            for (int j = 0; j < 6; j+=2)
            {
                hpFitness += (1-stats[i][j])/3;  
                manaFitness += (1-stats[i][j+1])/3;             
            }
            //Death penalty
            if (stats[i][0] == 0)
            {
                deathPenalty -= PlayerDeathWeight;
            }
            if (stats[i][2] == 0)
            {
                deathPenalty -= PlayerDeathWeight;
            }
            if (stats[i][4] == 0)
            {
                deathPenalty -= PlayerDeathWeight;
            }
            //Lose weight penalty
            if (stats[i][6] != 0)
            {
                losePenalty =  -PlayerLoseWeight;
            }
            //Round penalty as squared difference from target rounds
            roundPenalty += -Mathf.Pow(stats[i][7]-targetRounds,2);

            
        }
        hpFitness = HPWeight*hpFitness / stats.Count;
        manaFitness = ManaWeight*manaFitness / stats.Count;
        deathPenalty = deathPenalty / stats.Count;
        roundPenalty = roundPenalty / stats.Count;
        losePenalty = losePenalty / stats.Count;
        fitness = baseFitness + hpFitness + manaFitness + deathPenalty + roundPenalty + losePenalty;
        return new float[] {Mathf.Max(fitness,0),hpFitness,manaFitness,deathPenalty,roundPenalty,losePenalty};
    }

    // Select a parent for crossover
    int[] SelectParent()
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
            return TournamentSelection(selection_type);
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
        float bestFitness=0;
        for (int i = 0; i < k; i++)
        {
            
            int index = Random.Range(0, populationSize);
            float fitness = fitPopulation[index];
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
        for (int i = 0; i < genotype.Length; i++)
        {
            if (Random.value < mutationRate)
            {
                genotype[i] = Random.Range(ParamMins[i], ParamMaxs[i] + 1);
            }
        }
        return genotype;
    }
    public void SaveIndividual(int[] individual, string path)
    {
        jsonIO.SaveParam(individual, path);
    }
    public int[] LoadIndividual(string path)
    {
        return jsonIO.LoadParam(path);
    }
}
