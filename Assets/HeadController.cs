
using UnityEngine;
using System.Collections;
using Rewired;
using Valve;


public class HeadController : MonoBehaviour
{

    public int playerId = 0; // The Rewired player id of this character

    public float moveSpeed = 3.0f;

    private Player player; // The Rewired Player
    private GameObject cc;

    private Vector3 moveVector;
    private float thrustInput;
    private Quaternion rotVector;
    private float rotThrust;

    private Vector3 rotInput;
    private bool fire;
    private float maxRotSpeed;
    private float maxSpeed;
    private float mass;
    private float thrustModifier;
    private float thrust;

    private Rigidbody ship;


    void Awake()
    {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);
        cc = gameObject;
        rotThrust = .05f;
        thrustModifier = 16; 
        Debug.Log("Testing");
        rotVector = new Quaternion(0, 0, 0, .1f);

    }

    private void Start()
    {
        ship = GetComponent<Rigidbody>();
        Valve.VR.OpenVR.Compositor.SetTrackingSpace(Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated);
        Valve.VR.OpenVR.System.ResetSeatedZeroPose();

    }

    void Update()
    {
        GetInput();
        ProcessInput();
    }

    private void GetInput()
    {
        // Get the input from the Rewired Player. All controllers that the Player owns will contribute, so it doesn't matter
        // whether the input is coming from a joystick, the keyboard, mouse, or a custom controller.

        float roll = deadZoneInput(player.GetAxis("roll"),rotThrust,.05f);
        float pitch = deadZoneInput(player.GetAxis("pitch"),rotThrust,.05f);
        float yaw = deadZoneInput(player.GetAxis("yaw"), rotThrust, .05f);

        Quaternion rotChange = Quaternion.Euler(pitch, yaw, roll);

        rotVector = rotVector * rotChange;


        thrustInput = player.GetAxis("thrust")-.5f;
        if(thrustInput > 0)
        {
            Debug.Log(thrustInput);
        }
        thrust = thrustInput * thrustModifier;

    }

    private float deadZoneInput(float axis,float multiplier,float minInput)
    {
        if(Mathf.Abs(axis) > minInput)
        {
            return axis * multiplier;
        }
        else
        {
            return 0f; 
        }
    }

    private void ProcessInput()
    {
        cc.transform.Rotate(rotVector.eulerAngles);
        ship.AddForce(transform.forward * thrust);

   
    }
}