using UnityEngine;
using System.Collections;
using System; //This allows the IComparable Interface

public class Tile : IComparable<Tile>
{
    public string tileType;
    public int rarity;

    public Tile(string newTileType, int newRarity)
    {
        tileType = newTileType;
        rarity = newRarity;
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