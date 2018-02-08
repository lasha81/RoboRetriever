using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorController : MonoBehaviour {

    public LayerMask lm;
    public GameObject StartPoint;
    public GameObject EndPoint;
    public GeneralSensorController generalSensorController;
    public GameObject obstacle;

    public float GetDistance(float MaxDistance)
    {
        if (generalSensorController.isInsideObstacle)
        {
            obstacle = generalSensorController.obstacle;
            return 0;
        }

        RaycastHit hit;
        if (Physics.Raycast(StartPoint.transform.position, EndPoint.transform.position - StartPoint.transform.position, out hit, MaxDistance, lm))
        {
            obstacle = hit.collider.gameObject;
            return Vector3.Distance(hit.point, StartPoint.transform.position);
        }
        else return MaxDistance;
    }
}
