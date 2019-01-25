using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Head

{
    public enum Edge { Top, Bottom, Left, Right };
    public enum HeadType { River, Trail };
    public HeadType type; // these should all probably be serialized privates.
    public Edge edge;
    public int location;
}