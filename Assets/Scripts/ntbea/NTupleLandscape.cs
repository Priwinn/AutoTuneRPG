using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class NTupleLandscape
{
    private SearchSpace _searchSpace;
    private HashSet<int[]> _tuples;
    private int _ndims;
    private HashSet<int[]> _sampledPoints;
    private float _ucbEpsilon;
    private Dictionary<string, Dictionary<string, Dictionary<string, float>>> _tupleStats;


    public NTupleLandscape(SearchSpace searchSpace, int[] tupleConfig = null, float ucbEpsilon = 0.5f)
    {
        _searchSpace = searchSpace;
        _tuples = new HashSet<int[]>();
        _ndims = searchSpace.GetNumDims();
        _sampledPoints = new HashSet<int[]>();
        _ucbEpsilon = ucbEpsilon;
        _tupleStats = new Dictionary<string, Dictionary<string, Dictionary<string, float>>>();
        if (tupleConfig == null)
        {
            tupleConfig = new int[] {1,2};
        }

        foreach (int n in tupleConfig)
        {
            int[][] nTuples = GetTupleCombinations(n, _ndims);
            foreach (int[] tuple in nTuples)
            {
                _tuples.Add(tuple);
            }
        }
    }

    public void Reset()
    {
        _tupleStats = new Dictionary<string, Dictionary<string, Dictionary<string, float>>>();
    }

    public void Init()
    {
        foreach (int[] tuple in _tuples)
        {
            int[][] searchSpaceValues = GetTupleCombinations(tuple, _searchSpace);
            string stringTuple = string.Join(",", tuple);
            if (!_tupleStats.ContainsKey(stringTuple))
            {
                _tupleStats.Add(stringTuple, new Dictionary<string, Dictionary<string, float>>());
            }
        }
    }

    public void AddEvaluatedPoint(int[] point, float fitness)
    {
        _sampledPoints.Add(point);

        foreach (int[] tuple in _tuples)
        {
            int[] searchSpaceValue = GetSearchSpaceValue(point, tuple);
            string stringTuple = string.Join(",", tuple);
            string stringSearchSpaceValue = string.Join(",", searchSpaceValue);
            
            if (!_tupleStats[stringTuple].ContainsKey(stringSearchSpaceValue))
            {
                _tupleStats[stringTuple].Add(stringSearchSpaceValue, new Dictionary<string, float>(){
                    {"n", 0},
                    {"max", float.MinValue},
                    {"min", float.MaxValue},
                    {"sum", 0.0f},
                    {"sum_squared", 0.0f}
                });
            }
            _tupleStats[stringTuple][stringSearchSpaceValue]["n"] += 1;
            _tupleStats[stringTuple][stringSearchSpaceValue]["max"] = Mathf.Max(_tupleStats[stringTuple][stringSearchSpaceValue]["max"], fitness);
            _tupleStats[stringTuple][stringSearchSpaceValue]["min"] = Mathf.Min(_tupleStats[stringTuple][stringSearchSpaceValue]["min"], fitness);
            _tupleStats[stringTuple][stringSearchSpaceValue]["sum"] += fitness;
            _tupleStats[stringTuple][stringSearchSpaceValue]["sum_squared"] += Mathf.Pow(fitness, 2);
        }
    }

    public float GetMeanEstimate(int[] point)
    {
        float sum = 0.0f;
        int tupleCount = 0;

        foreach (int[] tuple in _tuples)
        {
            int[] searchSpaceValue = GetSearchSpaceValue(point, tuple);
            string stringTuple = string.Join(",", tuple);
            string stringSearchSpaceValue = string.Join(",", searchSpaceValue);
            //Check if the search space value has a dict
            if (!_tupleStats[stringTuple].ContainsKey(stringSearchSpaceValue))
            {
                continue;
            }
            if (_tupleStats[stringTuple][stringSearchSpaceValue]["n"] > 0)
            {
                sum += _tupleStats[stringTuple][stringSearchSpaceValue]["sum"] / _tupleStats[stringTuple][stringSearchSpaceValue]["n"];
                tupleCount++;
            }
        }

        if (tupleCount == 0)
        {
            return 0.0f;
        }

        return sum / tupleCount;
    }

    public float GetExplorationEstimate(int[] point)
    {
        float sum = 0.0f;
        int tupleCount = _tuples.Count;

        foreach (int[] tuple in _tuples)
        {
            int[] searchSpaceValue = GetSearchSpaceValue(point, tuple);
            string stringTuple = string.Join(",", tuple);
            string stringSearchSpaceValue = string.Join(",", searchSpaceValue);
            if ((!_tupleStats[stringTuple].ContainsKey(stringSearchSpaceValue)) || (_tupleStats[stringTuple][stringSearchSpaceValue]["n"] == 0))
            {
                float n = 0;
                foreach (Dictionary<string, float> value in _tupleStats[stringTuple].Values)
                {
                    n += value["n"];
                }
                
                sum += Mathf.Sqrt(Mathf.Log(1 + n) / _ucbEpsilon);
            }
            else
            {
                float n = _tupleStats[stringTuple][stringSearchSpaceValue]["n"];
                sum += Mathf.Sqrt(Mathf.Log(1 + n) / (n + _ucbEpsilon));
            }
        }

        return sum / tupleCount;
    }

    public int[] GetBestSampled()
    {
        float currentBestMean = float.MinValue;
        int[] currentBestPoint = null;

        foreach (int[] point in _sampledPoints)
        {
            float mean = GetMeanEstimate(point);

            if (mean > currentBestMean)
            {
                currentBestMean = mean;
                currentBestPoint = point;
            }
        }

        return currentBestPoint;
    }

    private int[][] GetTupleCombinations(int n, int ndims)
    {
        return GetUniqueCombinations(0, n, Enumerable.Range(0, ndims).ToArray());
    }

    private int[][] GetUniqueCombinations(int idx, int n, int[] sourceArray)
    {
        int[][] result = new int[0][];
        for (int i = idx; i < sourceArray.Length; i++)
        {
            if (n - 1 > 0)
            {
                int[][] nextLevel = GetUniqueCombinations(i + 1, n - 1, sourceArray);
                foreach (int[] x in nextLevel)
                {
                    int[] value = new int[] { sourceArray[i] };
                    value = value.Concat(x).ToArray();
                    result = result.Append(value).ToArray();
                }
            }
            else
            {
                result = result.Append(new int[] { sourceArray[i] }).ToArray();
            }
        }
        return result;
    }

    private int[][] GetTupleCombinations(int[] tuple, SearchSpace searchSpace)
    {
        List<int[]> result = new List<int[]>();
        GetTupleCombinations(0, tuple.Length, tuple, searchSpace, result);
        return result.ToArray();
    }

    private void GetTupleCombinations(int idx, int length, int[] tuple, SearchSpace searchSpace, List<int[]> result)
    {
        if (idx == length)
        {
            result.Add(tuple);
            return;
        }

        int dim = tuple[idx];
        int dimSize = searchSpace.GetDimSize(dim);
        int[] validValues = searchSpace.GetValidValuesInDim(dim);

        foreach (int value in validValues)
        {
            int[] newTuple = (int[])tuple.Clone();
            newTuple[idx] = value;
            GetTupleCombinations(idx + 1, length, newTuple, searchSpace, result);
        }
    }

    private int[] GetSearchSpaceValue(int[] point, int[] tuple)
    {
        int[] searchSpaceValue = new int[tuple.Length];
        for (int i = 0; i < tuple.Length; i++)
        {
            searchSpaceValue[i] = point[tuple[i]];
        }
        return searchSpaceValue;
    }
}
