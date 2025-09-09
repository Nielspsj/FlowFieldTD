using UnityEngine;


public class Cell
{
    //Test1: Test IncreaseCost method. When/where to run it?

    public Vector3 worldPos;
    public Vector2Int gridIndex;
    public byte cost;
    public ushort bestCost; //max value: 65535
    public GridDirection bestGridDirection;


    //Constructor used to intilize each cell
    public Cell(Vector3 _worldPos, Vector2Int _gridIndex)
    {
        worldPos = _worldPos;
        gridIndex = _gridIndex;
        cost = 1;
        bestCost = ushort.MaxValue; //max value: 65535
        bestGridDirection = GridDirection.None;
    }

    
    //Keep our max starting cell cost to max byte value 255.
    public void IncreaseCost(int amount)
    {
        if (cost == byte.MaxValue)
        {
            return;
        }
        if (amount + cost >= byte.MaxValue)
        {
            cost = byte.MaxValue;
        }
        else
        {
            cost += (byte)amount;
        }
    }
}
