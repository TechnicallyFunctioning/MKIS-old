using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Input Variables
    PlayerInput input;
    Vector2 pitchYaw;
    bool pitchYawPressed;
    bool rollLeftPressed;
    bool rollRightPressed;
    bool boostPressed;
    bool firePressed;
    bool brakePressed;

    //Audio Variables
    public AudioSource audioSource;
    public AudioClip fireSound;

    //Movement Variables
    public float speedBrake = 5f;
    public float speedDefault = 8f;
    public float speedBoost = 15f;
    public Vector3 yawSpeed = new Vector3(0, 80, 0);
    public Vector3 rollSpeed = new Vector3(0, 0, 80);
    public Vector3 pitchSpeed = new Vector3(80, 0, 0);
    private float speed;
    private Rigidbody rb;

    //Laser Variables
    public GameObject bullet;
    public Transform barrel;
    public float nextShot = 0.25f;
    public float fireDelay = 0.5f;
    public float bulletSpeed = 40;



    private void Awake()
    {
        // Get Player Input
        input = new PlayerInput();

        // Pitch / Roll / Left Stick
        input.ShipControls.PitchYaw.performed += ctx =>
        {
            pitchYaw = ctx.ReadValue<Vector2>();
            pitchYawPressed = pitchYaw.x != 0 || pitchYaw.y != 0;
        };

        // Roll / Left Shoulder / Right Shoulder
        input.ShipControls.RollLeft.performed += ctx => rollLeftPressed = ctx.ReadValueAsButton();
        input.ShipControls.RollRight.performed += ctx => rollRightPressed = ctx.ReadValueAsButton();

        // Boost / A Button
        input.ShipControls.Boost.performed += ctx => boostPressed = ctx.ReadValueAsButton();

        // Fire / Right Trigger
        input.ShipControls.Fire.performed += ctx => firePressed = ctx.ReadValueAsButton();

        //Brake / B Button
        input.ShipControls.Brake.performed += ctx => brakePressed = ctx.ReadValueAsButton();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //Boost / Brake
        if (boostPressed)
        {
            speed = speedBoost;
        }

        else
        {
            speed = speedDefault;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movement();

        if (firePressed && Time.time > nextShot)
        {
            Fire();
        }
    }

    void movement()
    {
        // Move Forward / Brake
        if (brakePressed)
        {
            rb.velocity = new Vector3(0, 0, 0);
        }
        else
        {
            rb.MovePosition(rb.position + transform.forward * speed * Time.deltaTime);
        }

        //Yaw
        Quaternion yaw = Quaternion.Euler(pitchYaw.x * yawSpeed * Time.deltaTime);
            rb.MoveRotation(rb.rotation * yaw);

        //Pitch
        Quaternion pitch = Quaternion.Euler(pitchYaw.y * pitchSpeed * Time.deltaTime);
            rb.MoveRotation(rb.rotation * pitch);

        //Roll
        Quaternion rollRight = Quaternion.Euler(-rollSpeed * Time.deltaTime);
        Quaternion rollLeft = Quaternion.Euler(rollSpeed * Time.deltaTime);
        if (rollLeftPressed && !rollRightPressed)
        {
            rb.MoveRotation(rb.rotation * rollLeft);
        }
        if (rollRightPressed && !rollLeftPressed)
        {
            rb.MoveRotation(rb.rotation * rollRight);
        }
    }

    public void Fire()
    {
        // Spawn Bullet, Give Velocity, Play Sound, Destroy Bullet, Cooldown
        GameObject spawnedBullet = Instantiate(bullet, barrel.position, barrel.rotation);
        spawnedBullet.GetComponent<Rigidbody>().velocity = bulletSpeed * barrel.forward;
        audioSource.PlayOneShot(fireSound);
        Destroy(spawnedBullet, 2);
        nextShot = Time.time + fireDelay;
    }


    //Enable / Disable Ship Controls
    private void OnEnable()
    {
        input.ShipControls.Enable();
    }

    private void OnDisable()
    {
        input.ShipControls.Disable();
    }
}
