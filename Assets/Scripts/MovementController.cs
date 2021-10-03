using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    new Rigidbody rigidbody;
    Animator anim;
    Vector2 inputs;
    Vector3 refVelocity;
    public float movementSmooth = 0.1f;
    public float movementSmoothAirborn = 1f;
    public float moveSpeed = 3;
    public float moveSpeedAirborn = 1.5f;

    [Header("Grounded")]
    public LayerMask groundLayer = 0;
    public bool IsGrounded { get; private set};

    [Header("Gravity")]
    public float downwardAccelerationBonus = 1;

    public static MovementController Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        inputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        GroundTest();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetVelocity = rigidbody.velocity;

        //Read the movement vector
        Vector2 inputVector = inputs;

        //Apply it to the correct axis
        if (IsGrounded)
        {
            targetVelocity.x = moveSpeed * inputVector.x;
            targetVelocity.z = moveSpeed * inputVector.y;

            rigidbody.velocity = Vector3.SmoothDamp(rigidbody.velocity, targetVelocity, ref refVelocity, movementSmooth);
        }
        else
        {
            targetVelocity.x = moveSpeedAirborn * inputVector.x;
            targetVelocity.z = moveSpeedAirborn * inputVector.y;

            rigidbody.velocity = Vector3.SmoothDamp(rigidbody.velocity, targetVelocity, ref refVelocity, movementSmoothAirborn);
        }
        UpdateFacingDirection();

        anim.SetFloat("MovementInput", Mathf.Abs(inputVector.magnitude));

        if (rigidbody.velocity.y < 0)
        {
            rigidbody.AddForce(Vector3.down * downwardAccelerationBonus, ForceMode.Acceleration);
        }
    }

    void GroundTest()
    {
        RaycastHit hit;
        IsGrounded = Physics.Raycast(transform.position + Vector3.up * 0.025f, Vector3.down, out hit, 0.05f, groundLayer);
    }

    void UpdateFacingDirection()
    {
        if (rigidbody.velocity.sqrMagnitude > 0.05f)
        {
            Vector3 lookRotation = rigidbody.velocity;
            lookRotation.y = 0;
            rigidbody.rotation = Quaternion.LookRotation(lookRotation);
        }
    }

    public void Jump(float height)
    {
        rigidbody.velocity = rigidbody.velocity * 0.5f;
        rigidbody.AddForce(Vector3.up * height, ForceMode.VelocityChange);
    }
}
