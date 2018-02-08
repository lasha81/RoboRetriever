using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotsAcademy : Academy
{
    [HideInInspector]
    public bool GetToBall, RetrieveBall, AvoidObstacles;

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
