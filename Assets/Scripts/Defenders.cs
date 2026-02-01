using UnityEngine;

public class Defenders : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float range = 5f;
    private int collisionMask;
    private float timeCounter;
    [SerializeField] float fireRate = 2f;
    [SerializeField] LayerMask enemyLayerMask;
    //[SerializeField] Collider[] hits = new Collider[16];

    UnityController unityController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //collisionMask = LayerMask.GetMask("Unit");
        unityController = GameObject.Find("UnityController").GetComponent<UnityController>();
    }

    // Update is called once per frame
    void Update()
    {
        //FindTarget();
        ShootTarget();
    }

    //Search for a target Unit
    private void FindTarget()
    {
        Collider[] hits = new Collider[16];

        int count = Physics.OverlapSphereNonAlloc(transform.position, range, hits, enemyLayerMask);

        Transform closest;
        float minDistSq = range * range;

        for (int i = 0; i < count; i++)
        {
            float distSq = (hits[i].transform.position - transform.position).sqrMagnitude;
            if (distSq < minDistSq)
            {
                minDistSq = distSq;
                closest = hits[i].transform;
                target = closest;
                Debug.Log("Found target");
            }
        }

    }

    //Shoot at target until it doesn't exist. Then find new target
    private void ShootTarget()
    {
        if(target == null)
        {
            Debug.Log("Target: " + target);
            FindTarget();
        }
        else
        {
            Debug.Log("Target locked to: " + target);

            timeCounter += Time.deltaTime;
            if (timeCounter > fireRate)
            {
                timeCounter = 0;
                unityController.unitsInGame.Remove(target.gameObject);
                Destroy(target.gameObject);
                Debug.Log("Shot");
            }
        }
    }    
}
