using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSensorController : MonoBehaviour {

    public bool isInsideObstacle;
    public GameObject obstacles;
    public GameObject obstacle;

    // Update is called once per frame
    void FixedUpdate () {
        isInsideObstacle = false;
        obstacle = null;
        foreach (Collider collider in obstacles.GetComponentsInChildren<Collider>()) {
            //Debug.Log(collider.gameObject.name);
            if (collider.bounds.Contains(transform.position))
            {
                isInsideObstacle = true;
                obstacle = collider.gameObject;
            }
        }
        
    }
}

