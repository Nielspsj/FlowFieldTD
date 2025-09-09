using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class FlowField
{
    //Test1: Test if world pos for the first cell is at 0,0,0.

    //Then we can access the cells in the grid. Other scripts can too.
    public Cell[,] grid {  get; private set; }
    public Vector2Int gridSize { get; private set; }
    public float cellRadius { get; private set; }
    public Cell destinationCell;

    private float cellDiameter;

    //Constructor for the class.
    public FlowField(Vector2Int _gridSize, float _cellRadius)
    {
        gridSize = _gridSize;
        cellRadius = _cellRadius;
        cellDiameter = cellRadius * 2;
    }

    //take cell values and set gridsize to create grid?
    public void CreateGrid()
    {        
        //Define the grid array size to have the same size as the gridsize value
        grid = new Cell[gridSize.x, gridSize.y];

        //Loop through each value in the 2D array and give it worldpos and gridindex.
        //Create new Cell object and pass these values to it.
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                //Move the pos by cellDiameter and add radius to center the cell. 
                Vector3 worldPos = new Vector3(cellDiameter * x + cellRadius, 0, cellDiameter * y + cellRadius);
                grid[x,y] = new Cell(worldPos, new Vector2Int(x, y));

                //Debug.Log(worldPos);
            }
        }
    }

    public void CreateCostField()
    {
        Vector3 cellHalfExtends = Vector3.one * cellRadius;
        int terrainMask = LayerMask.GetMask("Impassable", "RoughTerrain");
        foreach(Cell curCell in grid)
        {
            Collider[] obstacles = Physics.OverlapBox(curCell.worldPos, cellHalfExtends, Quaternion.identity, terrainMask);
            bool hasIncreasedCost = false;
            foreach(Collider col in obstacles)
            {
                if(col.gameObject.layer == 6)
                {
                    curCell.IncreaseCost(255);
                    continue;
                }
                else if(col.gameObject.layer == 7)
                {
                    curCell.IncreaseCost(3);
                    hasIncreasedCost = true;
                }
            }
        }
    }

    public void CreateIntegrationField(Cell _destinationCell)
    {
        destinationCell = _destinationCell;
        destinationCell.cost = 0;
        destinationCell.bestCost = 0;

        //Queue is first in first out sort of list.
        //We start there and calculate in all directions around the cell
        Queue<Cell> cellsToCheck = new Queue<Cell>();

        cellsToCheck.Enqueue(destinationCell);

        while (cellsToCheck.Count > 0)
        {
            Cell curCell = cellsToCheck.Dequeue();
            List<Cell> curNeighbours = GetNeighbourCells(curCell.gridIndex, GridDirection.cardinalDirections);

            foreach (Cell curNeighbour in curNeighbours)
            {
                //Skip this iteration if the cost is at max value.
                if (curNeighbour.cost == byte.MaxValue) { continue; }
                //Starts at 0, neighbours are 1, then we move to that cell and it starts at 1
                //
                if(curNeighbour.cost + curCell.bestCost < curNeighbour.bestCost)
                {
                    curNeighbour.bestCost = (ushort)(curNeighbour.cost + curCell.bestCost);
                    cellsToCheck.Enqueue(curNeighbour);
                }
            }
        }
    }

    public void CreateFlowField()
    {
        foreach(Cell curCell in grid)
        {
            List<Cell> curNeighbours = GetNeighbourCells(curCell.gridIndex, GridDirection.allDirections);

            int bestCost = curCell.bestCost;

            //Check neighbours to see if they have a lower bestcost. Update bestcost. Get direction.
            //Only time the cell will have lower bestcost than the neighbours is if it is the destination cell.
            foreach (Cell curNeighbour in curNeighbours)
            {
                if (curNeighbour.bestCost < bestCost)
                {
                    bestCost = curNeighbour.bestCost;
                    curCell.bestGridDirection = GridDirection.GetGridDirectionFromV2I(curNeighbour.gridIndex - curCell.gridIndex);
                }
            }
        }
    }

    //Add all the neighbours into a list with their position in the grid
    private List<Cell> GetNeighbourCells(Vector2Int _nodeIndex, List<GridDirection> _directions)
    {
        List<Cell> neighbourCells = new List<Cell>();

        foreach (Vector2Int curDirection in _directions)
        {
            Cell newNeighbourCell = GetCellAtRelativePos(_nodeIndex, curDirection);
            if (newNeighbourCell != null)
            {
                neighbourCells.Add(newNeighbourCell);
            }
        }

        return neighbourCells;
    }

    //Store the cells relative position as a position in the grid
    private Cell GetCellAtRelativePos (Vector2Int _originPos, Vector2Int _relativePos)
    {
        Vector2Int finalPos = _originPos + _relativePos;

        if(finalPos.x < 0 || finalPos.x >= gridSize.x || finalPos.y < 0 || finalPos.y >= gridSize.y)
        {
            return null;
        }
        else
        {
            return grid[finalPos.x, finalPos.y];
        }
    }

    //To get the clicked destination on the grid
    public Cell GetCellFromWorldPos(Vector3 _worldPos)
    {
        //Get "worldpos" inside the grid
        float percentX = _worldPos.x / (gridSize.x * cellDiameter);
        float percentY = _worldPos.z / (gridSize.y * cellDiameter);

        //Force it between 0 and 1.
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.Clamp(Mathf.FloorToInt((gridSize.x) * percentX), 0, gridSize.x - 1);
        int y = Mathf.Clamp(Mathf.FloorToInt((gridSize.y) * percentY), 0, gridSize.y - 1);
        
        //Patch to fix that the clicked cell was 1 y off in the grid.
        //Not needed, the problem is the GUI.
        //y += 1;
       
        return grid[x, y];
    }
}
