using UnityEngine;
using System.Collections;
using System; //This allows the IComparable Interface

public class Tile : IComparable<Tile>
{
    public string tileType;
    public int rarity;
    public string northPath;
    public string southPath;
    public string eastPath;
    public string westPath;

    public Tile(string newTileType, int newRarity, string newNorthPath, string newSouthPath, string newEastPath, string newWestPath )
    {
        tileType = newTileType;
        rarity = newRarity;
        northPath = newNorthPath;
        southPath = newSouthPath;
        eastPath = newEastPath;
        westPath = newWestPath;
    }

    //This method is required by the IComparable
    //interface. 
    public int CompareTo(Tile other)
    {
        if (other == null)
        {
            return 1;
        }

        //Return the difference in rarity. Total placeholder comparison.
        return rarity - other.rarity;
    }

}