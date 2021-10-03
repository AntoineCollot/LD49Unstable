using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMonsterMove : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;
    new Rigidbody rigidbody;
    Vector3 refVelocity;
    public float movementSmooth = 0.5f;

    float refHeight;
    public float heightSmooth = 0.2f;

    [Header("Death")]
    public float knockbackForce = 10;
    bool isDead;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        SoundManager.PlaySound(7);
    }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        if (GameManager.gameIsOver || isDead)
            return;

        Vector3 target = MovementController.Instance.transform.position - transform.position;
        Vector3 targetFlat = target;
        targetFlat.y = 0;
        //Turn
        Quaternion rotation = Quaternion.LookRotation(targetFlat, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, turnSpeed * Time.deltaTime);

        //Move
        Vector3 targetVelocity = rigidbody.velocity;
        targetVelocity = moveSpeed * transform.forward;

        rigidbody.velocity = Vector3.SmoothDamp(rigidbody.velocity, targetVelocity, ref refVelocity, movementSmooth);

        float height = Mathf.SmoothDamp(transform.position.y, MovementController.Instance.transform.position.y + 1f, ref refHeight, heightSmooth);
        transform.Translate(Vector3.up * (height- transform.position.y), Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead && collision.collider.gameObject.layer != gameObject.layer)
            Destroy(gameObject);
        if (!GameManager.gameIsOver && collision.collider.CompareTag("Player"))
            GameManager.Instance.GameOver();
    }

    public void Kill(Vector3 direction)
    {
        if (isDead)
            return;
        SoundManager.PlaySound(6);
        isDead = true;
        rigidbody.AddForce(direction * knockbackForce, ForceMode.VelocityChange);
        rigidbody.useGravity = true;
    }
}
