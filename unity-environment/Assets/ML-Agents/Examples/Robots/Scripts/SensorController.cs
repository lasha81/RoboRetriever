using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorController : MonoBehaviour {

    public LayerMask lm;
    public GameObject StartingPoint;

    public float GetDistance(float MaxDistance)
    {
        RaycastHit hit;
        if (Physics.Raycast(StartingPoint.transform.position, transform.forward, out hit, MaxDistance, lm))
        {
            return Vector3.Distance(hit.point, StartingPoint.transform.position);
        }
        else return MaxDistance;
    }
}
