using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarMovement : MonoBehaviour
{
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;

    public Transform path;
    public float maxSteerAngle = 45f;

    private List<Transform> waypoints;
    private int currentWaypoint = 0;

    void Start()
    {
        Transform[] pathTransforms = GetComponentsInChildren<Transform>();
        waypoints = new List<Transform>();

        for(int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                waypoints.Add(pathTransforms[i]);
            }
        }
    }

    private void FixedUpdate()
    {
        ApplySteer();
    }

    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(waypoints[currentWaypoint].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;

        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
    }
}
