using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum Orientation { _0, _90, _180, _270 }

public class PathEnabled : MonoBehaviour
{
    public Orientation orientation;
    public int cwRotatedDegrees;
    public PathPoint[] pathPointList;

    public void setOrientation(int newCwRotatedDegrees)
    {
        cwRotatedDegrees = newCwRotatedDegrees;
        if (cwRotatedDegrees == 0) orientation = Orientation._0;
        else if (cwRotatedDegrees == 90) orientation = Orientation._90;
        else if (cwRotatedDegrees == 180) orientation = Orientation._180;
        else if (cwRotatedDegrees == 270) orientation = Orientation._270;
        else throw new Exception("tiles can only be rotated by 90 degree intervals"); 
    }
}

[Serializable]
public class PathPoint
{
    public enum PathPointType { River, Trail };
    public PathPointType pathPointType;

    public enum PathPointEdge { top, right, bottom, left };
    public PathPointEdge edge;
}