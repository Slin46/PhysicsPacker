using UnityEngine;

public class Balance : MonoBehaviour
{
    public bool invert;
    //dragging the arms up and down
    public float dragSensitivity = 2f;
    public float minAngle = -30f;
    public float maxAngle = 140f;

    private float currentAngle;
    //assign it in inspector in case i want to change
    public float torqueForce;
    public float angularDamping;
    public float maxForce;
    public float springForce;
    public float springDamping;

    public Vector3 targetVel;

    public Transform target;
    private JointDrive drive;
    private SoftJointLimitSpring spring;
    private ConfigurableJoint joint;
    private Quaternion startingRotation;

    //camera and body and arm target
    public Transform cam;
    public Transform com;
    public bool TargetMode;
    public bool canUse;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        invert = false;

        //increasing force so it can grab and hold the objects
        torqueForce = 500f;
        angularDamping = 0.0f;
        maxForce = 500f;

        springForce = 0f;
        springDamping = 0f;

        targetVel = new Vector3(0f, 0f, 0f);

        drive.positionSpring = torqueForce;
        drive.positionDamper = angularDamping;
        drive.maximumForce = maxForce;

        spring.spring = springForce;
        spring.damper = springDamping;

        //grab joint so that when it grabs the object the object act as a joint so it can be held
        joint = gameObject.GetComponent<ConfigurableJoint>();

        joint.slerpDrive = drive;

        joint.linearLimitSpring = spring;
        joint.rotationDriveMode = RotationDriveMode.Slerp;
        joint.projectionMode = JointProjectionMode.None;
        joint.targetAngularVelocity = targetVel;
        joint.configuredInWorldSpace = false;
        joint.swapBodies = true;

        startingRotation = Quaternion.Inverse(target.localRotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && canUse)
        {
            float mouseY = Input.GetAxis("Mouse Y");
            //inverse direction so that when u drag down arms go down and vice versa
            float direction = invert ? 1f : -1f;
            currentAngle += mouseY * dragSensitivity * direction;

            currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);

            target.localRotation = Quaternion.Euler(currentAngle, 0, 0);
        }
    }
    void LateUpdate()
    {
        if (invert)
            joint.targetRotation = Quaternion.Inverse(target.localRotation * startingRotation);
        else
            joint.targetRotation = target.localRotation * startingRotation;
    }
}
