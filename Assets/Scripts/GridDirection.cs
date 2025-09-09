using NUnit.Framework;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class GridDirection
{
    public readonly Vector2Int directionVector;

    public static readonly GridDirection None = new GridDirection(0, 0);
    public static readonly GridDirection North = new GridDirection(0, 1);
    public static readonly GridDirection East = new GridDirection(1, 0);
    public static readonly GridDirection South = new GridDirection(0, -1);
    public static readonly GridDirection West = new GridDirection(-1, 0);
    public static readonly GridDirection NorthEast = new GridDirection(1, 1);
    public static readonly GridDirection SouthEast = new GridDirection(1, -1);
    public static readonly GridDirection SouthWest = new GridDirection(-1, -1);
    public static readonly GridDirection NorthWest = new GridDirection(-1, 1);

    public static readonly List<GridDirection> cardinalDirections = new List<GridDirection>
    {
        North,       
        East,
        South,
        West,
    };

    public static readonly List<GridDirection> cardinalAndIntercardinalDirections = new List<GridDirection>
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    };

    public static readonly List<GridDirection> allDirections = new List<GridDirection>
    {
        None,
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    };


    //Store the vector when called.
    private GridDirection(int x, int y)
    {
        directionVector = new Vector2Int(x, y);
    }

    //Convert to vector2int without needing a typecast (implicit operator).
    public static implicit operator Vector2Int(GridDirection _direction)
    {
        return _direction.directionVector;
    }

    public static GridDirection GetGridDirectionFromV2I(Vector2Int _directionVector)
    {
        //Add cardinal direction method here.
        //Why default to None?
        return cardinalAndIntercardinalDirections.DefaultIfEmpty(None).FirstOrDefault(_direction => _direction == _directionVector);
    }
}
