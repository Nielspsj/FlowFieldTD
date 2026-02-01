using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAll : MonoBehaviour
{
    //Test1: Mouse input in an if-statement.
    //Test2: Create a Physics2D.Raycast that sends a ray.
    //Test3: Check if we hit anything.
    //Test4: Create a variable to store the thing we hit and want to drag. Store the hit info.
    //Test5: Empty the dragging variable when we release the mouse button.
    //Test6: Offset the position of the object being dragged?? (we need the object's original z-position).
    
    private Transform draggingTF = null;
    private Vector3 offset;
    [SerializeField] private LayerMask defenderMask;
  
    // Update is called once per frame
    void Update()
    {
        SendRay();
    }
    private void SendRay()
    {
        if (Input.GetMouseButtonDown(2))
        {
            //Store hit information of the object we hit. It must have a collider.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, defenderMask))
            {
                draggingTF = hit.transform;
                Debug.Log("hit: " + hit.collider.gameObject.name);

                offset = draggingTF.position - Camera.main.ScreenToWorldPoint(Input.mousePosition); ;
            }
        }
        //Empty the storage of the dragging variable when we release the mouse.
        else if (Input.GetMouseButtonUp(2))
        {
            draggingTF = null;
        }
        //If we have an object stored in the dragging variable: Change the position.
        if (draggingTF != null)
        {
            draggingTF.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        }
    }
}
