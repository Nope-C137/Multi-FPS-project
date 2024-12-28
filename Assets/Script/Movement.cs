using System.Collections;
using System.Collections.Generic;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Movement : MonoBehaviour
{
    public float wakkSpeed = 4f;
    public float sprintSpeed = 14f;
    public float maxVelocityChange = 10f;
    [Space]
    public float airControl = 0.5f;

    [Space]
    public float jumpHeight = 5f;

    private Vector2 input;
    private Rigidbody rb;

    private bool sprinting;
    private bool jumping;

    private bool grounded;

    [Header("Animations")]
    public Animation handAnimation;
    public AnimationClip handWalkAnim;
    public AnimationClip idleAnim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        input = new Vector2(x: Input.GetAxisRaw("Horizontal"), y: Input.GetAxisRaw("Vertical"));
        input.Normalize();

        sprinting = Input.GetButton("Sprint");
        jumping = Input.GetButton("Jump");
    }

    private void OnTriggerStay(Collider other)
    {
        grounded = true;
    }

    private void FixedUpdate()
    {
        if (grounded)
        {
            if (jumping)
            {
                rb.velocity = new Vector3(rb.velocity.x, y: jumpHeight, rb.velocity.z);
                handAnimation.clip = idleAnim;
                handAnimation.Play();
            }
            else if (input.magnitude > 0.5f)
            {
                handAnimation.clip = handWalkAnim;
                handAnimation.Play();
                rb.AddForce(CalculateMovement(sprinting ? sprintSpeed : wakkSpeed), ForceMode.VelocityChange);

            }
            else
            {
                handAnimation.clip = idleAnim;
                handAnimation.Play();

                var velocity1 = rb.velocity;
                velocity1 = new Vector3(x: velocity1.x * 0.2f * Time.fixedDeltaTime, velocity1.y, z: velocity1.z * 0.2f * Time.deltaTime);
                rb.velocity = velocity1;
            }
        }
        else 
        {
            if (input.magnitude > 0.5f)
            {
                rb.AddForce(CalculateMovement(sprinting ? sprintSpeed * airControl : wakkSpeed * airControl), ForceMode.VelocityChange);

            }
            else
            {
                var velocity1 = rb.velocity;
                velocity1 = new Vector3(x: velocity1.x * 0.2f * Time.fixedDeltaTime, velocity1.y, z: velocity1.z * 0.2f * Time.deltaTime);
                rb.velocity = velocity1;
            }
        }

        grounded = false;
        
    }
    

    Vector3 CalculateMovement(float _speed)
    {
        Vector3 targetVelocity = new Vector3(input.x, y: 0, z: input.y);
        targetVelocity = transform.TransformDirection(targetVelocity);

        targetVelocity *= _speed;

        Vector3 velocity = rb.velocity;

        if (input.magnitude > 0.5f)
        {
            Vector3 velocityChange = targetVelocity - velocity;

            velocityChange.x = Mathf.Clamp(value: velocityChange.x, min: -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(value: velocityChange.z, min: -maxVelocityChange, maxVelocityChange);

            velocityChange.y = 0;

            return (velocityChange);

        }
        else
        {
            return new Vector3();
        }

        
    }
}
