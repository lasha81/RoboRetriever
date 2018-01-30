using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotsAgent : Agent
{

    [Header("Specific to Robots")]
    public GameObject Ball;
    public GameObject Goal;
    public GameObject Floor;
    public GameObject Academy;
    public Material RedMaterial;
    public Material GreenMaterial;
    public Material YellowMaterial;


    public int CurrentStage;

    public bool Collided;

    public GameObject[] Obstacles;
    public SensorController[] Sensors;
    public Rigidbody RobotBodyRB;
    public Rigidbody BallRB;
    public GameObject Body;
    public GameObject NoseTargetPoint;
    public float PrevNoseBallDist = -1;
    public float PrevRobotBallAngle = -1;
    public float PrevRobotGoalAngle = -1;
    public float PrevBallGoalDist = -1;

    public float[] SensorValues = new float[6];
    public float[] PrevSensorValues =  new float[6];

    public RobotController robotController;

    private float minimalDistance = 7;

    private List<Vector3> ObjectPositions = new List<Vector3>();
    private Vector3 NewPosition;
    private float RandomRange = 20f;
    private float SensorMaxDistance = 4f;

    public override List<float> CollectState()
    {
        List<float> state = new List<float>();


        /*
        float dist;
        for (int i=0; i< Sensors.Length; i++)
        {
            dist = Sensors[i].GetDistance(SensorMaxDistance);
            //state.Add((dist - SensorMaxDistance / 2) / (SensorMaxDistance / 2 ));
            state.Add(1-(dist / SensorMaxDistance)); // 0 .. 1
            //Debug.Log(1 - (dist / SensorMaxDistance));
        }
        */



        Vector3 BallDir = Ball.transform.position - transform.position;
        BallDir.y = 0;
        Vector3 GoalDir = Goal.transform.position - transform.position;
        GoalDir.y = 0;

        //state.Add(transform.position.x / 25f);// normalized to -1 .. 1
        //state.Add(transform.position.z / 25f);// normalized to -1 .. 1
        //Debug.Log(state.Add(transform.rotation.eulerAngles.y));

        float BallAngle = Vector3.SignedAngle(BallDir, transform.forward, Vector3.up); // -180 .. 180
        state.Add(BallAngle/180f); // -1 .. 1
        //state.Add((BallAngle+180) / 360f); // 0 .. 1


        float GoalAngle = Vector3.SignedAngle(GoalDir, transform.forward, Vector3.up);
        state.Add(GoalAngle / 180f);

        float BallDistance = Vector3.Distance(transform.position, Ball.transform.position);
        state.Add(BallDistance / 35f);

        float GoalDistance = Vector3.Distance(transform.position, Goal.transform.position);
        state.Add(GoalDistance / 35f);

        
        
        state.Add(RobotBodyRB.velocity.x / 2f); // -1 .. 1
        state.Add(RobotBodyRB.velocity.z / 2f); // -1 .. 1
        state.Add(RobotBodyRB.angularVelocity.y / 2.5f); // -1 .. 1

        //state.Add((RobotBodyRB.velocity.x + 2) / 4f);  // 0 .. 1
        //state.Add((RobotBodyRB.velocity.z + 2) / 4f);  // 0 .. 1
        //state.Add((RobotBodyRB.angularVelocity.y + 2.5f) / 5f); // 0 .. 1




        state.Add(BallRB.velocity.x / 2f);
        state.Add(BallRB.velocity.z / 2f);        


        //Debug.Log("dist x = " + (Bullets.transform.position.x - gameObject.transform.position.x) + ", dist z = " + (Bullets.transform.position.z - gameObject.transform.position.z));
        return state;
    }
    
    public override void AgentStep(float[] act)
    {
        int action = (int)act[0];
        robotController.doAction(action);
        Transform bodyTransform = RobotBodyRB.transform;

        Vector3 BallDirection = Ball.transform.position - transform.position;
        BallDirection.y = 0;
        float RobotBallAngle = Vector3.SignedAngle(BallDirection, transform.forward, Vector3.up);


        Vector3 GoalDirection = Goal.transform.position - transform.position;
        GoalDirection.y = 0;
        float RobotGoalAngle = Vector3.SignedAngle(GoalDirection, transform.forward, Vector3.up);


        //Debug.Log(angle/180f);

        float NoseBallDist = Vector3.Distance(NoseTargetPoint.transform.position, Ball.transform.position);
        float BallGoalDist = Vector3.Distance(Goal.transform.position, Ball.transform.position);
        float RobotGoalDist = Vector3.Distance(transform.position, Goal.transform.position);

        /*
        for (int i = 0; i < Sensors.Length; i++)
        {
            SensorValues[i] = 1 - (Sensors[i].GetDistance(SensorMaxDistance) / SensorMaxDistance);
        }*/

        //if (SensorValues[0]!=0) Debug.Log(SensorValues[0]);

        // time punishment
        reward -= 0.01f;

        // ball between reward
        //if (BallGoalDist < RobotGoalDist) reward += 0.01f; else reward -= 0.01f;

        
        // nose getting closer to ball reward
        if (PrevNoseBallDist != -1)  
        {
            reward += (PrevNoseBallDist - NoseBallDist);
        }

        // look at ball reward
        if (PrevRobotBallAngle != -1) {
            reward += (PrevRobotBallAngle - Mathf.Abs(RobotBallAngle)) / 180f;
        }

        


        if (CurrentStage > 1) { 
            // not looking at same direction punishment
            //reward -= Mathf.Abs((RobotBallAngle - RobotGoalAngle)) / 5000f;
            
            // ball getting closer to goal reward
            if (BallGoalDist != -1)
            {
                reward += (PrevBallGoalDist - BallGoalDist) * 2f;
            }/**/

            // look at goal reward
            if (PrevRobotGoalAngle != -1)
            {
                reward += (PrevRobotGoalAngle - Mathf.Abs(RobotGoalAngle)) / 180f;
            }/**/
        }

        /*
        // obstacle proximity increase punishment
        if (CurrentStage > 2)
        {
            if (PrevNoseBallDist != -1) { 
                for (int i = 0; i < Sensors.Length; i++) {
                    reward -= 4*SensorValues[i]*(SensorValues[i] - PrevSensorValues[i]);
                    //if (i == 0 && SensoeValues[i] != PrevSensorValues[i]) Debug.Log(SensoeValues[i] - PrevSensorValues[i]);
                }
            }

        }
        */
        ///// endings


        // if robot or ball fell
        if (bodyTransform.position.y < -2f || Ball.transform.position.y < -2f)
        {
            //Debug.Log("Fell");
            done = true;
            reward = -1f;
        }

        /*
        if (CurrentStage == 1)
        {
            // robot reached ball
            if (NoseBallDist < 1.5f && NoseBallDist != -1)
            {
                //Debug.Log("Reached ball");
                done = true;
                reward = 1f;
            }
        }
       /**/


        // ball reached goal
        if (BallGoalDist < (1.5f + 0.25f) && BallGoalDist != -1)
            {
                //Debug.Log("Reached goal");
                done = true;
                reward = 1f;
            }/**/

        /*
        if (CurrentStage > 2)
        {
            // collided with obstacle
            if (Collided)
            {
                //Debug.Log("Collided");
                done = true;
                reward = -1f;
            }
        }/**/


        PrevNoseBallDist = NoseBallDist;
        PrevRobotBallAngle = Mathf.Abs(RobotBallAngle);
        PrevRobotGoalAngle = Mathf.Abs(RobotGoalAngle);
        PrevBallGoalDist = BallGoalDist;

        /*
        for (int i = 0; i < Sensors.Length; i++)
        {
            PrevSensorValues[i] = SensorValues[i];
        }
        */


    }


    public override void AgentReset()
    {
        //Debug.Log("reset");
        CurrentStage = Academy.GetComponent<RobotsAcademy>().currentStage;

        //if (CurrentStage == 1) Floor.GetComponentInChildren<MeshRenderer>().material = YellowMaterial;
        //if (CurrentStage == 2) Floor.GetComponentInChildren<MeshRenderer>().material = RedMaterial;




        PrevNoseBallDist = -1;
        PrevRobotBallAngle = -1;
        PrevRobotGoalAngle = -1;
        PrevBallGoalDist = -1;

        ObjectPositions.Clear();
        Collided = false;
        robotController.doAction(-1);//reset

        transform.position = GetNewPosition(0); 
        Ball.transform.position = GetNewPosition(0.25f);
        Goal.transform.position = GetNewPosition(0.05f);

        /*
        foreach (GameObject Obstacle in Obstacles)
        {
            Obstacle.transform.position = GetNewPosition(0.05f);
        }*/




        Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;


    }

    private Vector3 GetNewPosition(float height)
    {
        do NewPosition = new Vector3(Floor.transform.position.x +  Random.Range(-RandomRange, RandomRange), 0, Floor.transform.position.z + Random.Range(-RandomRange, RandomRange));
        while (!IsValidNewPosition());
        ObjectPositions.Add(NewPosition);
        NewPosition.y = height;
        return NewPosition;
    }

    private bool IsValidNewPosition()
    {

        foreach (Vector3 p in ObjectPositions)
        {
            if (Vector3.Distance(p, NewPosition) < minimalDistance) return false;
        }

        return true;
    }

}
