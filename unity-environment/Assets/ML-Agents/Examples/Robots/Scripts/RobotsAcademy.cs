using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotsAcademy : Academy
{

    public bool GetToBall;
    public bool RetrieveBall;
    public bool AvoidObstacles;


    public override void AcademyReset()
    {
        if ((int)resetParameters["get_to_ball"] == 0) GetToBall = false; else GetToBall = true;
        if ((int)resetParameters["retrieve_ball"] == 0) RetrieveBall = false; else RetrieveBall = true;
        if ((int)resetParameters["avoid_obstacles"] == 0) AvoidObstacles = false; else AvoidObstacles = true;
    }

    public override void AcademyStep()
    {

    }
}
