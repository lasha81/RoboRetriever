using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour {

    public GameObject FrontRightWheel;
    public GameObject FrontLeftWheel;
    public GameObject RearRightWheel;
    public GameObject RearLeftWheel;


    public float ForwardMotorPower;
    public float RearMotorPower;

    private HingeJoint FrontRightHingeJoint;
    private HingeJoint FrontLeftHingeJoint;
    private HingeJoint RearRightHingeJoint;
    private HingeJoint RearLeftHingeJoint;

    private JointMotor FrontRightJointMotor;
    private JointMotor FrontLeftJointMotor;
    private JointMotor RearRightJointMotor;
    private JointMotor RearLeftJointMotor;


    // Use this for initialization
    void Start () {


        FrontRightHingeJoint = FrontRightWheel.GetComponent<HingeJoint>();
        FrontLeftHingeJoint = FrontLeftWheel.GetComponent<HingeJoint>();
        RearRightHingeJoint = RearRightWheel.GetComponent<HingeJoint>();
        RearLeftHingeJoint = RearLeftWheel.GetComponent<HingeJoint>();

        FrontRightJointMotor = FrontRightHingeJoint.motor;
        FrontLeftJointMotor = FrontLeftHingeJoint.motor;
        RearRightJointMotor = RearRightHingeJoint.motor;
        RearLeftJointMotor = RearLeftHingeJoint.motor;
    }

    public void doAction(int Action)
    {
        FrontRightHingeJoint.useMotor = false;
        FrontLeftHingeJoint.useMotor = false;
        RearRightHingeJoint.useMotor = false;
        RearLeftHingeJoint.useMotor = false;


        //if  (Action == 4) return;

        if (Action == 0) //w
        {
            FrontRightJointMotor.targetVelocity = ForwardMotorPower;
            FrontLeftJointMotor.targetVelocity = ForwardMotorPower;
            RearRightJointMotor.targetVelocity = ForwardMotorPower;
            RearLeftJointMotor.targetVelocity = ForwardMotorPower;

            FrontRightJointMotor.force = 200;
            FrontLeftJointMotor.force = 200;
            RearRightJointMotor.force = 200;
            RearLeftJointMotor.force = 200;
            
            FrontRightHingeJoint.useMotor = true;
            FrontLeftHingeJoint.useMotor = true;
            RearRightHingeJoint.useMotor = true;
            RearLeftHingeJoint.useMotor = true;
        }

        if (Action == 1) //s
        {
            FrontRightJointMotor.targetVelocity = -RearMotorPower;
            FrontLeftJointMotor.targetVelocity = -RearMotorPower;
            RearRightJointMotor.targetVelocity = -RearMotorPower;
            RearLeftJointMotor.targetVelocity = -RearMotorPower;

            FrontRightJointMotor.force = 200;
            FrontLeftJointMotor.force = 200;
            RearRightJointMotor.force = 200;
            RearLeftJointMotor.force = 200;
            
            FrontRightHingeJoint.useMotor = true;
            FrontLeftHingeJoint.useMotor = true;
            RearRightHingeJoint.useMotor = true;
            RearLeftHingeJoint.useMotor = true;
        }

        

        if (Action == 2) //a
        {
            FrontRightJointMotor.targetVelocity = ForwardMotorPower;
            FrontLeftJointMotor.targetVelocity = -ForwardMotorPower;
            RearRightJointMotor.targetVelocity = ForwardMotorPower;
            RearLeftJointMotor.targetVelocity = -ForwardMotorPower;

            FrontRightJointMotor.force = 200;
            FrontLeftJointMotor.force = 200;
            RearRightJointMotor.force = 200;
            RearLeftJointMotor.force = 200;

            FrontRightHingeJoint.useMotor = true;
            FrontLeftHingeJoint.useMotor = true;
            RearRightHingeJoint.useMotor = true;
            RearLeftHingeJoint.useMotor = true;
        }

        if (Action == 3) //d
        {


            FrontRightJointMotor.targetVelocity = -ForwardMotorPower;
            FrontLeftJointMotor.targetVelocity = ForwardMotorPower;
            RearRightJointMotor.targetVelocity = -ForwardMotorPower;
            RearLeftJointMotor.targetVelocity = ForwardMotorPower;

            FrontRightJointMotor.force = 200;
            FrontLeftJointMotor.force = 200;
            RearRightJointMotor.force = 200;
            RearLeftJointMotor.force = 200;

            FrontRightHingeJoint.useMotor = true;
            FrontLeftHingeJoint.useMotor = true;
            RearRightHingeJoint.useMotor = true;
            RearLeftHingeJoint.useMotor = true;
        }

        if (Action == -1) // reset
        {
            Rigidbody RobotBodyRB = GetComponent<Rigidbody>();
            
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            transform.position = new Vector3(0, 0, 0);

            RobotBodyRB.angularVelocity = Vector3.zero;
            RobotBodyRB.velocity = Vector3.zero;



            FrontRightJointMotor.targetVelocity = 0;
            FrontLeftJointMotor.targetVelocity = 0;
            RearRightJointMotor.targetVelocity = 0;
            RearLeftJointMotor.targetVelocity = 0;


            FrontRightHingeJoint.useMotor = true;
            FrontLeftHingeJoint.useMotor = true;
            RearRightHingeJoint.useMotor = true;
            RearLeftHingeJoint.useMotor = true;
        }

        FrontRightHingeJoint.motor = FrontRightJointMotor;
        FrontLeftHingeJoint.motor = FrontLeftJointMotor;
        RearRightHingeJoint.motor = RearRightJointMotor;
        RearLeftHingeJoint.motor = RearLeftJointMotor;

    }
}
