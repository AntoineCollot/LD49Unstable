using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    new Rigidbody rigidbody;
    Animator anim;
    Vector3 refVelocity;
    public float movementSmooth = 0.1f;

    public float moveSpeed = 3;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetVelocity = rigidbody.velocity;

        //Read the movement vector
        Vector2 inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        //Apply it to the correct axis
        targetVelocity.x = moveSpeed * inputVector.x;
        targetVelocity.z = moveSpeed * inputVector.y;

        rigidbody.velocity = Vector3.SmoothDamp(rigidbody.velocity, targetVelocity, ref refVelocity, movementSmooth);
        UpdateFacingDirection();

        anim.SetFloat("MovementInput", Mathf.Abs(inputVector.magnitude));
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
}
