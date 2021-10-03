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
    public float rotationSpeed = 1;

    [Header("Grounded")]
    public Transform groundTestRef = null;
    public LayerMask groundLayer = 0;
    public bool IsGrounded { get; private set; }
    public float groundTestLength = 0.6f;
    public float heightDelta = 0.05f;
   //RaycastHit hitGround;
    float lastJumpTime= -10;

    [Header("Gravity")]
    public float downwardAccelerationBonus = 1;

    public static MovementController Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (GameManager.gameIsOver)
            inputs = Vector2.zero;
        else
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

        GroundTest();

        if (rigidbody.velocity.y < 0 && !IsGrounded)
        {
            rigidbody.AddForce(Vector3.down * downwardAccelerationBonus, ForceMode.Acceleration);
        }
    }

    void GroundTest()
    {
        RaycastHit hit;
        if (Physics.OverlapSphere(groundTestRef.position, 0.03f, groundLayer).Length > 0)
        {
            Vector3 pos = transform.position;
            pos.y = groundTestRef.position.y + heightDelta - groundTestRef.localPosition.y;
            transform.position = pos;
        }
        else
        {

            IsGrounded = Physics.Raycast(groundTestRef.position, Vector3.down, out hit, groundTestLength, groundLayer);
            if (IsGrounded && Time.time > lastJumpTime + 2)
            {
                Vector3 pos = groundTestRef.position;
                if (pos.y < hit.point.y + heightDelta)
                {
                    pos.y = hit.point.y + heightDelta - groundTestRef.localPosition.y;
                    transform.position = pos;
                }
            }
        }
    }

    void UpdateFacingDirection()
    {
        Vector3 velocity = rigidbody.velocity;
        velocity.y = 0;
        if (velocity.sqrMagnitude > 0.05f)
        {
            Vector3 lookRotation = velocity;
            lookRotation.y = 0;
            Quaternion targetRot = Quaternion.LookRotation(lookRotation);
            rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }

    public void Jump(float height)
    {
        Vector3 velocity = rigidbody.velocity;
        velocity.y = 0;
        velocity *= 0.5f;
        rigidbody.velocity = velocity;
        rigidbody.AddForce(Vector3.up * height, ForceMode.VelocityChange);
        lastJumpTime = Time.time;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(groundTestRef.position + Vector3.left * 0.02f, Vector3.down * groundTestLength);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(groundTestRef.position+Vector3.left*0.01f, Vector3.down * heightDelta);
    }
}
