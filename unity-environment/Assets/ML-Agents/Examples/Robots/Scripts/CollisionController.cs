using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour {

    public RobotsAgent robotAgent;

    private void OnTriggerEnter(Collider other)
    {

        
        if (other.gameObject.layer == 11 || other.gameObject.layer == 12) { 
            robotAgent.Collided = true;
        }
    }



}
