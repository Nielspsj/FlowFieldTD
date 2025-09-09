using UnityEngine;

public class GridController : MonoBehaviour
{
    //Test1: Bug. Clicked cell is 1 above the one that displays 0.
    public DebugGrid debugGrid;
    public Transform mousePosCube;
    public Vector2Int gridSize;
    public float cellRadius = 0.5f;
    public FlowField curFlowField;

    //flowfield gets created here as a c# object with the values we send it.
    private void InitializeFlowField()
    {
        curFlowField = new FlowField(gridSize, cellRadius);
        curFlowField.CreateGrid();

        debugGrid.SetFlowField(curFlowField);
        //Debug.Log("initialize flowfield");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InitializeFlowField();

            curFlowField.CreateCostField();

            //destinationCell from mouse click:
            /*
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            Cell destinationCell = curFlowField.GetCellFromWorldPos(worldMousePos);
            */

            //destinationCell set from start:
            Vector3 worldDestinationPos = new Vector3(10.5f, 0f, 9.5f);
            //mousePosCube.position = worldMousePos; //For debuging.
            Cell destinationCell = curFlowField.GetCellFromWorldPos(worldDestinationPos);
            curFlowField.CreateIntegrationField(destinationCell);

            curFlowField.CreateFlowField();
            debugGrid.DrawFlowField();
        }
    }
}
