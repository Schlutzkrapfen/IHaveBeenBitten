using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRange;
    [Range(0f, 360f)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public List<Transform> visibleTargets = new List<Transform>();
    public List<Transform> targetsInRange = new List<Transform>();

    private void Start()
    {
        StartCoroutine("FindTargetsWithDelay", .2f);
    }
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);

            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        //set layer to default thenn later reset the layer so it doesent colide with itself
        int objectLayer = gameObject.layer;

        gameObject.layer = 0;

        visibleTargets.Clear();
        targetsInRange.Clear();
        //get all transforms tag as target in view range
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRange, targetMask);
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;

            Vector2 currentTarget = new Vector2(target.position.x,target.position.z);
            Vector2 currentPos = new Vector2(transform.position.x,transform.position.z);


            Vector2 dirToTraget = (currentTarget - currentPos).normalized;

            //check if it is in view angle

            if (Vector2.Angle(transform.forward, dirToTraget) < viewAngle * 0.5f)
            {
                // check if there is an obstacle in between 

                float distToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTraget, distToTarget, obstacleMask))
                {
                    //we see the target
                    visibleTargets.Add(target);
                }
            }

            targetsInRange.Add(targetsInViewRadius[i].transform);
        }

        gameObject.layer = objectLayer;
    }
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
