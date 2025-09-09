using UnityEditor;
using UnityEngine;

public class DebugGrid : MonoBehaviour
{
    public GridController gridController;
    public bool displayGrid = false;
    public enum FlowFieldDisplayType
    {
        none, costField, integrationField, allIcons, destinationIcon
    };
    public FlowFieldDisplayType flowFieldDisplayType;

    private Vector2Int gridSize;
    private float cellRadius;
    private FlowField curFlowField;
    private Sprite[] ffIcons;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ffIcons = Resources.LoadAll<Sprite>("Sprites/FFIcons");
    }

    public void SetFlowField(FlowField newFlowField)
    {
        curFlowField = newFlowField;
        gridSize = newFlowField.gridSize;
        cellRadius = newFlowField.cellRadius;
    }

    public void DrawFlowField()
    {
        ClearCellDisplay();

        switch(flowFieldDisplayType)
        {
            case FlowFieldDisplayType.allIcons:
                DisplayAllCells();
                break;
            case FlowFieldDisplayType.destinationIcon:
                DisplayDestinationCell();
                break;

            default:
                break;
        }
    }

    private void DisplayAllCells()
    {
        if(curFlowField == null)
        {
            return;
        }
        foreach(Cell curCell in curFlowField.grid)
        {
            DisplayCell(curCell);
        }
    }

    private void DisplayDestinationCell()
    {
        if (curFlowField == null)
        {
            return;
        }
        DisplayCell(curFlowField.destinationCell);
    }

    private void DisplayCell(Cell cell)
    {
        GameObject iconGO = new GameObject();
        SpriteRenderer iconSR = iconGO.AddComponent<SpriteRenderer>();
        iconGO.transform.parent = transform;
        iconGO.transform.position = cell.worldPos;

        if (cell.cost == 0)
        {
            //icon 3 is destination cel
            iconSR.sprite = ffIcons[3];
            Quaternion newRot = Quaternion.Euler(90, 0, 0);
            iconGO.transform.rotation = newRot;
        }
        else if (cell.cost == byte.MaxValue)
        {
            //icon 2 is impassable cell
            iconSR.sprite = ffIcons[2];
            Quaternion newRot = Quaternion.Euler(90, 0, 0);
            iconGO.transform.rotation = newRot;
        }
        else if (cell.bestGridDirection == GridDirection.North)
        {
            iconSR.sprite = ffIcons[0];
            Quaternion newRot = Quaternion.Euler(90, 0, 0);
            iconGO.transform.rotation = newRot;
        }
        else if (cell.bestGridDirection == GridDirection.East)
        {
            iconSR.sprite = ffIcons[0];
            Quaternion newRot = Quaternion.Euler(90, 90, 0);
            iconGO.transform.rotation = newRot;
        }
        else if (cell.bestGridDirection == GridDirection.South)
        {
            iconSR.sprite = ffIcons[0];
            Quaternion newRot = Quaternion.Euler(90, 180, 0);
            iconGO.transform.rotation = newRot;
        }
        else if (cell.bestGridDirection == GridDirection.West)
        {
            iconSR.sprite = ffIcons[0];
            Quaternion newRot = Quaternion.Euler(90, 270, 0);
            iconGO.transform.rotation = newRot;
        }
        else if (cell.bestGridDirection == GridDirection.NorthEast)
        {
            iconSR.sprite = ffIcons[1];
            Quaternion newRot = Quaternion.Euler(90, 0, 0);
            iconGO.transform.rotation = newRot;
        }
        else if (cell.bestGridDirection == GridDirection.SouthEast)
        {
            iconSR.sprite = ffIcons[1];
            Quaternion newRot = Quaternion.Euler(90, 90, 0);
            iconGO.transform.rotation = newRot;
        }
        else if (cell.bestGridDirection == GridDirection.SouthWest)
        {
            iconSR.sprite = ffIcons[1];
            Quaternion newRot = Quaternion.Euler(90, 180, 0);
            iconGO.transform.rotation = newRot;
        }
        else if (cell.bestGridDirection == GridDirection.NorthWest)
        {
            iconSR.sprite = ffIcons[1];
            Quaternion newRot = Quaternion.Euler(90, 270, 0);
            iconGO.transform.rotation = newRot;
        }
        else
        {
            //Why add this? Failsafe? Test to see if you caught all the directions?
            iconSR.sprite = ffIcons[0];
        }
    }

    private void ClearCellDisplay()
    {
        foreach (Transform t in transform)
        {
            GameObject.Destroy(t.gameObject);
        }
    }
       

    private void OnDrawGizmos()
    {
        if(displayGrid == true)
        {
            if(curFlowField == null)
            {
                DrawGrid(gridController.gridSize, Color.yellow, gridController.cellRadius);
            }
            else
            {
                DrawGrid(gridSize, Color.green, cellRadius);
            }
        }

        if (curFlowField == null)
        {
            return;
        }

        GUIStyle guiStyle = new GUIStyle(GUI.skin.label);
        guiStyle.alignment = TextAnchor.MiddleCenter;

        switch (flowFieldDisplayType)
        {
            case FlowFieldDisplayType.costField:

                foreach(Cell curCell in curFlowField.grid)
                {
                    Handles.Label(curCell.worldPos, curCell.cost.ToString(), guiStyle);
                }
                break;

            case FlowFieldDisplayType.integrationField:

                foreach(Cell curCell in curFlowField.grid)
                {
                    Handles.Label(curCell.worldPos, curCell.bestCost.ToString(), guiStyle);
                    //Debug.Log("integration cells showed");
                }
                break;

            default:
                //Debug.Log("switch default");
                break;
        }
    }

    private void DrawGrid(Vector2Int drawGridSize, Color drawColor, float drawCellRadius)
    {
        Gizmos.color = drawColor;
        for(int x = 0; x < drawGridSize.x; x++)
        {
            for(int y = 0; y < drawGridSize.y; y++)
            {
                Vector3 center = new Vector3(drawCellRadius * 2 * x + drawCellRadius, 0, drawCellRadius * 2 * y + drawCellRadius);
                Vector3 size = Vector3.one * drawCellRadius * 2;
                Gizmos.DrawWireCube(center, size);
            }
        }
    }
}
