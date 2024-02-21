using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchSpace
{
    private int[][] _validValues; 
    private int _ndims;

    public SearchSpace(int[][] validValues)
    {
        _validValues = validValues;
        _ndims = validValues.Length;
    }
    public int GetNumDims()
    {
        return _ndims;
    }

    public int[] GetRandomPoint(){
        int[] point = new int[_ndims];
        for (int i = 0; i < _ndims; i++)
        {
            point[i] = _validValues[i][Random.Range(0, _validValues[i].Length)];
        }
        return point;
    }

    public int GetSize(){
        int size = 1;
        for (int i = 0; i < _ndims; i++)
        {
            size *= _validValues[i].Length;
            Debug.Log("validValues["+i+"].Length: " + _validValues[i].Length);
        }
        return size;
    }

    public int GetDimSize(int j){
        return _validValues[j].Length;
    }

    public int[] GetValidValuesInDim(int dim){
        return _validValues[dim];
    }
}