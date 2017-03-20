using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float lookSensitivity = 3f;

    [SerializeField]
    private float thrusterforce = 1000;


    [Header("Spring settings:")]
    [SerializeField]
    private JointDriveMode jointMode = JointDriveMode.Position;
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;


    private PlayerMotor motor;
    private ConfigurableJoint joint;

	void Start()
	{
        joint = GetComponent<ConfigurableJoint>();
		motor = GetComponent<PlayerMotor>();

        SetJointSettings(jointSpring);
    }

	void Update()
	{
		//calculate movement velocity as a 3d vector
		float _xMov  = Input.GetAxisRaw("Horizontal");
		float _zMov = Input.GetAxisRaw("Vertical");

		Vector3 _movHorizontal = transform.right * _xMov;
		Vector3 _movVertical = transform.forward * _zMov;

		//final movement vector
		Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;

		//apply movement
		motor.Move(_velocity);

		//calculate rotation as a 3d vector: turning
		float _yRot = Input.GetAxisRaw("Mouse X");

		Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

		//Apply rotation
		motor.Rotate(_rotation);

		//calculate camera rotation as a 3d vector: up and down
		float _xRot = Input.GetAxisRaw("Mouse Y");

		float _cameraRotationX = _xRot * lookSensitivity;

		//Apply rotation
		motor.RotateCamera(_cameraRotationX);

        //apply thrusterforce
        Vector3 _thrusterforce = Vector3.zero;
        if (Input.GetButton("Jump"))
        {
            _thrusterforce = Vector3.up * thrusterforce;
            SetJointSettings(0f);
        }
        else
        {
            SetJointSettings(jointSpring);
        }
        motor.Applythruster(_thrusterforce);

	}


    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive
        {
            mode = jointMode,
            positionSpring = jointSpring,
            maximumForce = jointMaxForce
        };
    }

}
