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

    public UnityEngine.UI.Text TextReward;

    public bool GetToBall;
    public bool RetrieveBall;
    public bool AvoidObstacles;

    public bool Collided;

    const int SENSOR_COUNT = 2;

    public GameObject[] Obstacles;
    public SensorController[] Sensors;
    public Rigidbody RobotBodyRB;
    public Rigidbody BallRB;
    public GameObject Body;
    public GameObject NoseTargetPoint;
    public float RobotBallAngle;
    public float[] RobotObstacleAngles  = new float[SENSOR_COUNT];
    int MovingTowardsBallSign;
    float RobotMovementBallAngle;
    /*public float PrevNoseBallDist = -1;
    public float PrevRobotBallAngle = -1;
    public float PrevRobotGoalAngle = -1;
    public float PrevBallGoalDist = -1;*/

    public float[] SensorValues = new float[SENSOR_COUNT];
    private float[] SensorDistances = new float[SENSOR_COUNT];
    private Vector3[] ObstacleDirections = new Vector3[SENSOR_COUNT];
    private Vector3[] RobotMovementProjectionToObstacles = new Vector3[SENSOR_COUNT];
    //public float[] PrevSensorValues =  new float[OBSTACLE_COUNT];
    int[] MovingTowardsObstacleSigns = new int[SENSOR_COUNT];
    float[] RobotMovementObstacleAngles = new float[SENSOR_COUNT];

    public RobotController robotController;

    private float minimalDistance = 5;

    private List<Vector3> ObjectPositions = new List<Vector3>();
    private Vector3 NewPosition;
    private float RandomRange = 20f;
    private float SensorMaxDistance = 4f;
    private bool IsFirstStep;
    private float ObstacleReward;



    public override List<float> CollectState()
    {
        List<float> state = new List<float>();

        
        if (AvoidObstacles) { 
            for (int i=0; i< Sensors.Length; i++)
            {
                SensorDistances[i] = Sensors[i].GetDistance(SensorMaxDistance);
                SensorValues[i] = 1 - (SensorDistances[i] / SensorMaxDistance);
                state.Add(SensorValues[i]); // 0 .. 1
                /*if (SensorValues[i]>0) { 
                    state.Add((Sensors[i].GetComponent<SensorController>().obstacle.transform.position.x- NoseTargetPoint.transform.position.x)/35); 
                    state.Add((Sensors[i].GetComponent<SensorController>().obstacle.transform.position.y - NoseTargetPoint.transform.position.y)/35); 
                } else
                {
                    state.Add(1);
                    state.Add(1);
                }*/
            }
        }


        if (GetToBall)
        {
            Vector3 BallDir = Ball.transform.position - NoseTargetPoint.transform.position;
            BallDir.y = 0;

            float BallAngle = Vector3.SignedAngle(BallDir, transform.forward, Vector3.up); // -180 .. 180
            state.Add(BallAngle / 180f); // -1 .. 1

            float BallDistance = Vector3.Distance(NoseTargetPoint.transform.position, Ball.transform.position);
            state.Add(BallDistance / 35f);

        }

        if (RetrieveBall)
        {
            Vector3 GoalDir = Goal.transform.position - NoseTargetPoint.transform.position;
            GoalDir.y = 0;

            float GoalAngle = Vector3.SignedAngle(GoalDir, transform.forward, Vector3.up);
            state.Add(GoalAngle / 180f);

            float GoalDistance = Vector3.Distance(NoseTargetPoint.transform.position, Goal.transform.position);
            state.Add(GoalDistance / 35f);

            state.Add(BallRB.velocity.x / 2f);
            state.Add(BallRB.velocity.z / 2f);

        }


        //state.Add(transform.position.x / 25f);// normalized to -1 .. 1
        //state.Add(transform.position.z / 25f);// normalized to -1 .. 1
        //Debug.Log(state.Add(transform.rotation.eulerAngles.y));

        //state.Add((BallAngle+180) / 360f); // 0 .. 1

        state.Add(RobotBodyRB.velocity.x / 2f); // -1 .. 1
        state.Add(RobotBodyRB.velocity.z / 2f); // -1 .. 1
        state.Add(RobotBodyRB.angularVelocity.y / 2.5f); // -1 .. 1

        //state.Add((RobotBodyRB.velocity.x + 2) / 4f);  // 0 .. 1
        //state.Add((RobotBodyRB.velocity.z + 2) / 4f);  // 0 .. 1
        //state.Add((RobotBodyRB.angularVelocity.y + 2.5f) / 5f); // 0 .. 1


        
        //Debug.Log("dist x = " + (Bullets.transform.position.x - gameObject.transform.position.x) + ", dist z = " + (Bullets.transform.position.z - gameObject.transform.position.z));
        return state;
    }
    
    public override void AgentStep(float[] act)
    {
        int action = (int)act[0];
        robotController.doAction(action);
        ObstacleReward = 0;

        Vector3 BallDirection = Ball.transform.position - transform.position;
        BallDirection.y = 0;
        RobotBallAngle = Vector3.SignedAngle(BallDirection, transform.forward, Vector3.up);

        RobotMovementBallAngle = Vector3.Angle(BallDirection, RobotBodyRB.velocity);
        

        if (RobotMovementBallAngle < 90) MovingTowardsBallSign = 1; else MovingTowardsBallSign = -1;
        Vector3 RobotMovementProjectionToBall = Vector3.Project(RobotBodyRB.velocity, BallDirection);
        if (RobotMovementProjectionToBall.magnitude < 0.2f) MovingTowardsBallSign = 0;


        Vector3 GoalDirection = Goal.transform.position - transform.position;
        GoalDirection.y = 0;
        float RobotGoalAngle = Vector3.SignedAngle(GoalDirection, transform.forward, Vector3.up);


        //Debug.Log(angle/180f);

        float NoseBallDist = Vector3.Distance(NoseTargetPoint.transform.position, Ball.transform.position);
        float BallGoalDist = Vector3.Distance(Goal.transform.position, Ball.transform.position);
        float RobotGoalDist = Vector3.Distance(transform.position, Goal.transform.position);

        if (AvoidObstacles) { 
            for (int i = 0; i < Sensors.Length; i++)
            {
                SensorValues[i] = 1 - (Sensors[i].GetDistance(SensorMaxDistance) / SensorMaxDistance);
            }
        }

        //if (SensorValues[0]!=0) Debug.Log(SensorValues[0]);

        // time punishment
        reward = -0.01f;

        //if (!IsFirstStep)
        //{

        if (GetToBall)
        {
            reward += MovingTowardsBallSign * RobotMovementProjectionToBall.magnitude / 5f;

            /*// nose getting closer to ball reward
            reward += (PrevNoseBallDist - NoseBallDist);
            // look at ball reward
            reward += (PrevRobotBallAngle - Mathf.Abs(RobotBallAngle)) / 180f;*/
            //reward = RobotMovementBallAngle / 500f;
            //- Mathf.Abs(RobotBallAngle) / 500f 
            //- 0.005f * NoseBallDist;

            //reward = PrevNoseBallDist - NoseBallDist;
            //reward += (Mathf.Abs(PrevRobotBallAngle) - Mathf.Abs(RobotBallAngle))/100f;
            //Debug.Log(RobotMovementBallAngle);
        }

        if (RetrieveBall)
        {

            // ball getting closer to goal reward
            //reward += (PrevBallGoalDist - BallGoalDist) * 2f;

            // look at goal reward
            //reward += (PrevRobotGoalAngle - Mathf.Abs(RobotGoalAngle)) / 180f;
        }

        // obstacle proximity increase punishment
        if (AvoidObstacles)
        {


            //reward += MovingTowardsBallSign * RobotMovemntProjectionToBall.magnitude / 5f;
            for (int i = 0; i < Sensors.Length; i++)
            {
                if (SensorValues[i]>0) { 
                    ObstacleDirections[i] = Sensors[i].GetComponent<SensorController>().obstacle.transform.position - transform.position;
                    ObstacleDirections[i].y = 0;
                    //RobotObstacleAngles[i] = Vector3.SignedAngle(ObstacleDirections[i], transform.forward, Vector3.up);

                    RobotMovementObstacleAngles[i] = Vector3.Angle(ObstacleDirections[i], RobotBodyRB.velocity);

                    if (RobotMovementObstacleAngles[i] < 90) MovingTowardsObstacleSigns[i] = 1; else MovingTowardsObstacleSigns[i] = -1;
                    RobotMovementProjectionToObstacles[i] = Vector3.Project(RobotBodyRB.velocity, ObstacleDirections[i]);
                    if (RobotMovementProjectionToObstacles[i].magnitude < 0.2f) MovingTowardsObstacleSigns[i] = 0;

                    float RobotMovementObstacleAngle = Vector3.Angle(ObstacleDirections[i], RobotBodyRB.velocity);
                    ObstacleReward -= 0.5f * (MovingTowardsObstacleSigns[i] * RobotMovementProjectionToObstacles[i].magnitude / SENSOR_COUNT);
                }

            }

            if (ObstacleReward < -0.1f) reward = ObstacleReward;
        }

            


            //if (ObstacleReward!=0) reward = ObstacleReward;

            //reward = reward / 10f;
            //}

            //if (Mathf.Abs(reward) > 1) reward = reward / Mathf.Abs(reward);
        if (reward  > 0.8f) reward = 0.8f;
        if (reward < -0.8f) reward = -0.8f;
        //}

        if (transform.parent.gameObject.name=="Game")
        {
            //Debug.Log(RobotMovemntProjectionToBall);
            //Debug.DrawLine(transform.position, transform.position+RobotMovemntProjectionToBall, Color.yellow,0,false);
            TextReward.text = ObstacleReward.ToString("F5");
            //Debug.Log(ObstacleReward.ToString("F4"));
            //if (RobotBodyRB.velocity.magnitude > 0.3) TextReward.text = reward.ToString("F4"); else TextReward.text = "S";
            //TextReward.text = (RobotBodyRB.velocity.magnitude.ToString("F10"));
        }/**/

       


        ///// endings


        // if robot or ball fell
        if (RobotBodyRB.transform.position.y < -2f || Ball.transform.position.y < -2f)
        {
            //Debug.Log("Fell");
            done = true;
            reward = -1f;
        }

        if (GetToBall && !RetrieveBall)
        {
            // robot reached ball
            if (NoseBallDist < 1.5f)
            {
                //Debug.Log("Reached ball");
                done = true;
                reward = 1f;
            }/**/
        }


        if (RetrieveBall) {
            // ball reached goal
            /*
            if (BallGoalDist < (1.5f + 0.25f))
            {
                //Debug.Log("Reached goal");
                done = true;
                reward = 1f;
            }/**/
        }

        
        if (AvoidObstacles)
        {
            // collided with obstacle
            
            if (Collided)
            {
                //Debug.Log("Collided");
                done = true;
                reward = -1f;
            }/**/
        }

        /*
        PrevNoseBallDist = NoseBallDist;
        PrevRobotBallAngle = RobotBallAngle;
        PrevRobotGoalAngle = Mathf.Abs(RobotGoalAngle);
        PrevBallGoalDist = BallGoalDist;
        */
        /*if (AvoidObstacles)
        {
            for (int i = 0; i < Sensors.Length; i++)
            {
                PrevSensorValues[i] = SensorValues[i];
            }
        }*/

        IsFirstStep = false;
    }


    public override void AgentReset()
    {
        //Debug.Log("reset");
   
        GetToBall = Academy.GetComponent<RobotsAcademy>().GetToBall;
        RetrieveBall = Academy.GetComponent<RobotsAcademy>().RetrieveBall;
        AvoidObstacles = Academy.GetComponent<RobotsAcademy>().AvoidObstacles;




        IsFirstStep = true;

        ObjectPositions.Clear();
        Collided = false;
        robotController.doAction(-1);//reset

        transform.position = GetNewPosition(new Vector3(-18, 0, -18), new Vector2(2, 2));
        transform.eulerAngles = new Vector3(10, Random.Range(-180, 180), 0);
        Ball.transform.position = GetNewPosition(new Vector3(15,0.25f,15), new Vector2(3,3));
        

        foreach (GameObject Obstacle in Obstacles)
        {
            Obstacle.transform.position = GetNewPosition(new Vector3(0, 0.05f, 0), new Vector2(20, 20), true);
        }

        Goal.transform.position = GetNewPosition(new Vector3(-18, 0.25f, 0), new Vector2(1, 18),true);


        Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;


    }

    private Vector3 GetNewPosition(Vector3 Offset, Vector2 RandomRange, bool AvoidCollision = false)
    {
        int i = 0;
        do
        {
            NewPosition = new Vector3(Floor.transform.position.x + Offset.x + Random.Range(-RandomRange.x, RandomRange.x), 0, Floor.transform.position.z + Offset.z + Random.Range(-RandomRange.y, RandomRange.y));

        } while (!IsValidNewPosition() && i++<10 && AvoidCollision);

        ObjectPositions.Add(NewPosition);
        NewPosition.y = Offset.y;
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
