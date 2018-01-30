using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotsAcademy : Academy
{
    public int currentStage;


    public override void AcademyReset()
    {
        currentStage = (int)resetParameters["current_stage"];
    }

    public override void AcademyStep()
    {

    }
}
