using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreComparer : IComparer<Score>
{
    public int Compare(Score x, Score y)
    { 
        return x.Points.CompareTo(y.Points);
    }
}
