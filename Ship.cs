using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Ship : MonoBehaviour
{
    Rigidbody rb;

    public ParticleSystem hitIndicator;
    public AudioSource audioSource;
    public AudioClip fireSound;
    public GameObject bullet;
    public Transform barrel;

    public float health = 5.0f;
    public float bulletSpeed = 40.0f;
    public float thrust = 15.0f;
    public Vector3 yawSpeed = new Vector3(0, 80, 0);
    public Vector3 pitchSpeed = new Vector3(80, 0, 0);
    public Vector3 rollSpeed = new Vector3(0, 0, 80);
    public float fireDelay = 0.5f;

    private float healthInitial;
    private Vector3 startPos;
    private Vector2 pitchYaw;
    private bool rollLeft;
    private bool rollRight;
    private bool firePressed;
    private bool boostPressed;
    private float nextShot = 0.25f;

//GetInput
    public void OnPitchYaw(InputAction.CallbackContext context)
    {
        pitchYaw = context.ReadValue<Vector2>();
    }
    public void OnRollLeft(InputAction.CallbackContext context)
    {
        rollLeft = context.ReadValueAsButton();
    }
    public void OnRollRight(InputAction.CallbackContext context)
    {
        rollRight = context.ReadValueAsButton();
    }
    public void OnBoost(InputAction.CallbackContext context)
    {
        boostPressed = context.ReadValueAsButton();
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        firePressed = context.ReadValueAsButton();
    }

    private void Start()
    {
        healthInitial = health;
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        hitIndicator = transform.Find("HitIndicator").GetComponent<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        PitchYaw(pitchYaw);
        RollLeft();
        RollRight();
        Throttle();
        if(firePressed && Time.time > nextShot)
        {
            Fire();
        }
        if(health < 1)
        {
            Respawn();
        }
    }

//Handle Movement
    private void PitchYaw(Vector2 direction)
    {
        Quaternion yaw = Quaternion.Euler(direction.x * yawSpeed * Time.deltaTime);
        rb.MoveRotation(rb.rotation * yaw);
        Quaternion pitch = Quaternion.Euler(direction.y * pitchSpeed * Time.deltaTime);
        rb.MoveRotation(rb.rotation * pitch);
    }
    private void RollLeft()
    {
        if (rollLeft)
        {
            Quaternion rollLeft = Quaternion.Euler(rollSpeed * Time.deltaTime);
            rb.MoveRotation(rb.rotation * rollLeft);
        }
    }
    private void RollRight()
    {
        if (rollRight)
        {
            Quaternion rollRight = Quaternion.Euler(-rollSpeed * Time.deltaTime);
            rb.MoveRotation(rb.rotation * rollRight);
        }
    }
    private void Throttle()
    {
        if (boostPressed)
        {
            rb.AddForce(transform.forward * thrust);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealthPowerUp"))
        {
            health = healthInitial;
        }
    }

    //Attack
    private void Fire()
    {
        GameObject spawnedBullet = Instantiate(bullet, barrel.position, barrel.rotation);
        spawnedBullet.GetComponent<Rigidbody>().velocity = bulletSpeed * barrel.forward;
        audioSource.PlayOneShot(fireSound);
        Destroy(spawnedBullet, 2);
        nextShot = Time.time + fireDelay;
    }

    public void TakeDamage()
    {
        health -= 1;
        hitIndicator.Play();
    }

    private void Respawn()
    {
        transform.position = startPos;
        health = healthInitial;
    }
}
