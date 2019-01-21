using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum Orientation { _0, _90, _180, _270 }

public class PathEnabled : MonoBehaviour
{
    public Orientation orientation;
    public PathPoint[] pathPointList;
}

[Serializable]
public class PathPoint
{
    public enum PathPointType { River, Trail };
    public PathPointType pathPointType;

    public enum PathPointEdge { top, right, bottom, left };
    public PathPointEdge edge;
}