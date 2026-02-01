using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


public class UnityController : MonoBehaviour
{
    public GridController gridController;
    public GameObject unitPref;
    public int numUnitsToSpawn;
    public float moveSpeed;

    public List<GameObject> unitsInGame = new List<GameObject>();
    private float spawnClearanceRadius = 2.5f;

    // Update is called once per frame
    void Update()
    {
        //Spawn units: 1 on keyboard
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnUnits();
        }
        //Destroy units: 2 on keyboard
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DestroyUnits();
        }
    }

    private void FixedUpdate()
    {
        if (gridController.curFlowField == null)
        {
            return;
        }
        foreach (GameObject unit in unitsInGame)
        {
            Cell cellBelow = gridController.curFlowField.GetCellFromWorldPos(unit.transform.position);
            Vector3 moveDirection = new Vector3(cellBelow.bestGridDirection.directionVector.x, 0, cellBelow.bestGridDirection.directionVector.y);
            Rigidbody unitRB = unit.GetComponent<Rigidbody>();
            unitRB.linearVelocity = moveDirection * moveSpeed;
        }
    }

    private void SpawnUnits()
    {
        Vector2Int gridSize = gridController.gridSize;
        float cellRadius = gridController.cellRadius;
        Vector2 maxSpawnPos = new Vector2(gridSize.x * cellRadius * 2 + cellRadius, gridSize.y * cellRadius * 2 + cellRadius);
        //Can't spawn on impassable cells or other units.
        int collisionMask = LayerMask.GetMask("Impassable", "Units", "SpawnBlocker");
        Vector3 newPos;

        for (int i = 0; i < numUnitsToSpawn; i++)
        {
            //Fill the spawn list
            GameObject newUnit = Instantiate(unitPref);
            newUnit.transform.parent = transform;
            unitsInGame.Add(newUnit);

            //Prevent looping forever
            int attempts = 0;
            int maxAttempts = 100;

            do
            {
                newPos = new Vector3(Random.Range(0, maxSpawnPos.x), 0, Random.Range(0, maxSpawnPos.y));
                //Debug.Log("maxSpawnPos.y: " + maxSpawnPos.y + " newPos: " + newPos);
                attempts++;
            }
            //Doesn't check fast enough because of the physics steps. Fine for slow spawning.
            while(Physics.OverlapSphere(newPos, spawnClearanceRadius, collisionMask).Length > 0 && attempts < maxAttempts);
            
            newUnit.transform.position = newPos;
        }
        //Debug.Log("Units ingame: " + unitsInGame.Count);
    }

    private void DestroyUnits()
    {
        foreach (GameObject unit in unitsInGame)
        {
            Destroy(unit);
        }
        unitsInGame.Clear();
    }
}
