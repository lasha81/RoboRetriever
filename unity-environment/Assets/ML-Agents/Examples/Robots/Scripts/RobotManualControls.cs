using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManualControls : MonoBehaviour {

    
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

    void Start()
    {


        FrontRightHingeJoint = FrontRightWheel.GetComponent<HingeJoint>();
        FrontLeftHingeJoint = FrontLeftWheel.GetComponent<HingeJoint>();
        RearRightHingeJoint = RearRightWheel.GetComponent<HingeJoint>();
        RearLeftHingeJoint = RearLeftWheel.GetComponent<HingeJoint>();

        FrontRightJointMotor = FrontRightHingeJoint.motor;
        FrontLeftJointMotor = FrontLeftHingeJoint.motor;
        RearRightJointMotor = RearRightHingeJoint.motor;
        RearLeftJointMotor = RearLeftHingeJoint.motor;

    }

    void FixedUpdate()
    {

        FrontRightHingeJoint.useMotor = false;
        FrontLeftHingeJoint.useMotor = false;
        RearRightHingeJoint.useMotor = false;
        RearLeftHingeJoint.useMotor = false;



        if (Input.GetKey("w"))
        {
            FrontRightJointMotor.targetVelocity = ForwardMotorPower;
            FrontLeftJointMotor.targetVelocity = ForwardMotorPower;
            RearRightJointMotor.targetVelocity = ForwardMotorPower;
            RearLeftJointMotor.targetVelocity = ForwardMotorPower;


            FrontRightHingeJoint.useMotor = true;
            FrontLeftHingeJoint.useMotor = true;
            RearRightHingeJoint.useMotor = true;
            RearLeftHingeJoint.useMotor = true;
        }

        if (Input.GetKey("s")) {
            FrontRightJointMotor.targetVelocity = -RearMotorPower;
            FrontLeftJointMotor.targetVelocity = -RearMotorPower;
            RearRightJointMotor.targetVelocity = -RearMotorPower;
            RearLeftJointMotor.targetVelocity = -RearMotorPower;


            FrontRightHingeJoint.useMotor = true;
            FrontLeftHingeJoint.useMotor = true;
            RearRightHingeJoint.useMotor = true;
            RearLeftHingeJoint.useMotor = true;
        }

        if (Input.GetKey("d")) {
            FrontRightJointMotor.targetVelocity = -ForwardMotorPower;
            FrontLeftJointMotor.targetVelocity = ForwardMotorPower;
            RearRightJointMotor.targetVelocity = -ForwardMotorPower;
            RearLeftJointMotor.targetVelocity = ForwardMotorPower;


            FrontRightHingeJoint.useMotor = true;
            FrontLeftHingeJoint.useMotor = true;
            RearRightHingeJoint.useMotor = true;
            RearLeftHingeJoint.useMotor = true;
        }

        if (Input.GetKey("a"))
        {
            FrontRightJointMotor.targetVelocity = ForwardMotorPower;
            FrontLeftJointMotor.targetVelocity = -ForwardMotorPower;
            RearRightJointMotor.targetVelocity = ForwardMotorPower;
            RearLeftJointMotor.targetVelocity = -ForwardMotorPower;


            FrontRightHingeJoint.useMotor = true;
            FrontLeftHingeJoint.useMotor = true;
            RearRightHingeJoint.useMotor = true;
            RearLeftHingeJoint.useMotor = true;
        }

        FrontRightHingeJoint.motor= FrontRightJointMotor;
        FrontLeftHingeJoint.motor= FrontLeftJointMotor;
        RearRightHingeJoint.motor= RearRightJointMotor;
        RearLeftHingeJoint.motor= RearLeftJointMotor;

    }
}
