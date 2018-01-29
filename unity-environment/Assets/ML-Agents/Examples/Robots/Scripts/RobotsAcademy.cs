using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotsAcademy : Academy
{
    public float currentStage;


    public override void AcademyReset()
    {
        currentStage = (float)resetParameters["current_stage"];
    }

    public override void AcademyStep()
    {

    }
}
