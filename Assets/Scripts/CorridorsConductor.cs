using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CorridorType
{
    Up, Down
}

public class CorridorsConductor : MonoBehaviour
{ 
    public GameObject downPointsContainer;
    public GameObject upPointsContainer;

    Dictionary<CorridorType, Vector3[]> _positionsOfCorridorPoints;
    Dictionary<CorridorType,Dictionary<int, bool>> _occupiedPoints; 
       
    private void Start()
    {
        _positionsOfCorridorPoints = new Dictionary<CorridorType, Vector3[]>();
        _occupiedPoints = new Dictionary<CorridorType, Dictionary<int, bool>>();

        InitCorridorPath(downPointsContainer, CorridorType.Down);
        InitCorridorPath(upPointsContainer, CorridorType.Up); 
    }
     
    private void InitCorridorPath(GameObject pathContainer, CorridorType corridorType)
    {
        //check on correct count 
        Transform[] pointsInCorridorPath = pathContainer.GetComponentsInChildren<Transform>().Skip(1).ToArray();
         
        _positionsOfCorridorPoints.Add(corridorType, new Vector3[pointsInCorridorPath.Length]);

        _occupiedPoints.Add(corridorType, new Dictionary<int, bool>());

        for (int i = 0; i < _positionsOfCorridorPoints[corridorType].Length; i++)
        {
            _positionsOfCorridorPoints[corridorType][i] = pointsInCorridorPath[i].transform.position;
            _occupiedPoints[corridorType].Add(i, false);
        }
    }

    public bool IsPointOccupied(int pointIndex, CorridorType type)
    {
        if (_occupiedPoints[type].Count - 1 < pointIndex)
        {
            Debug.LogWarning("Index out of path.");
            return false;
        }
        return _occupiedPoints[type][pointIndex];
    }

    public int GetPathLength(CorridorType type)
    {
        return _positionsOfCorridorPoints[type].Length;
    }

    public Vector3 GetPointPosition(int pointIndex, CorridorType type)
    {
        if (_positionsOfCorridorPoints[type].Length - 1 < pointIndex)
        {
            Debug.LogWarning("Index out of points corridor path.");
            return Vector3.zero;
        } 
        return _positionsOfCorridorPoints[type][pointIndex];
    }

    public Vector3 GetLastPointInPath(CorridorType type)
    {
        int pathLength = _positionsOfCorridorPoints[type].Length;

        return _positionsOfCorridorPoints[type][pathLength - 1];
    }

    public void SetOccupiedPointInPath(int pointIndex, bool value, CorridorType type)
    {
        if (_occupiedPoints[type].Count - 1 < pointIndex)
        {
            Debug.LogWarning("To large pointIndex value.");
            return;
        }
        _occupiedPoints[type][pointIndex] = value;
    }

    public void ClearAllOccupidPoints()
    { 
        foreach (var corrType in _occupiedPoints.Keys)
        {
            for (int i = 0; i < _occupiedPoints[corrType].Count; i++)
            {
                SetOccupiedPointInPath(i, false, corrType);
            }
        }
    }
}
