using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public Color lineColor;
    
    public List<Transform> waypoints = new List<Transform>();

    void OnDrawGizmosSelected()
    {
        Gizmos.color = lineColor;

        Transform[] pathTransforms = GetComponentsInChildren<Transform>();
        waypoints = new List<Transform>();

        for(int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != transform)
            {
                waypoints.Add(pathTransforms[i]);
            }
        }

        for(int i = 0; i < waypoints.Count; i++)
        {
            Vector3 currentWaypoint = waypoints[i].position;
            Vector3 previousWaypoint = Vector3.zero;

            if (i>0)
            {
                previousWaypoint = waypoints[i-1].position;
            }
            
            else if(i == 0 && waypoints.Count > 1)
            {
                previousWaypoint = waypoints[waypoints.Count-1].position;
            }

            Gizmos.DrawLine(previousWaypoint, currentWaypoint);
            Gizmos.DrawWireSphere(currentWaypoint, 0.3f);
        }
    }
}
