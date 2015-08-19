using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

    public float Speed = 10f;
    [Range(0, 1)]
    public float AirSpeedMultiplier = 0.3f;

    public float MaxJumpCharge = 10f;
    public float JumpChargePerSecond = 5f;
    public float JumpTapCutoff = 4f;
    public float JumpTapForce = 7f;
    private float currentJumpCharge = 0f;

    Rigidbody rb;

    public LayerMask Ground;
    public Camera cam;

    bool grounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (cam == null) cam = Camera.main;
    }

    public void Update()
    {
        if (Input.GetButton("Jump"))
        {
            currentJumpCharge += currentJumpCharge <= MaxJumpCharge ? JumpChargePerSecond * Time.deltaTime : 0;
            Debug.LogFormat("Charging {0}", currentJumpCharge);
        }

        if (Input.GetButtonUp("Jump"))
        {
            if (grounded)
            {
                if (currentJumpCharge > JumpTapCutoff)
                {
                    rb.AddForce(Vector3.up * currentJumpCharge, ForceMode.Impulse);
                }
                else
                {
                    rb.AddForce(Vector3.up * JumpTapForce, ForceMode.Impulse);
                }
            }
            currentJumpCharge = 0;
        }
    }

    void FixedUpdate()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, 1, Ground);

        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");

        Vector3 movement;//= new Vector3(HorizontalInput, 0, VerticalInput);
        movement = cam.transform.forward * VerticalInput;
        movement += cam.transform.right * HorizontalInput;

        movement *= Speed;

        if (!grounded)
        {
            movement *= AirSpeedMultiplier;
        }

        rb.AddForce(movement);
    }
}
