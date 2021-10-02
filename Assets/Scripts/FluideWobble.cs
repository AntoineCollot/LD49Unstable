using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluideWobble : MonoBehaviour
{
    Vector3 lastPos;
    Vector3 lastRot;
    Vector2 wobbleAmount;
    public float maxWobble;
    public float wobbleSpeed;
    public float calmTime;

    // Update is called once per frame
    void Update()
    {
        //Decrease
        wobbleAmount.x = Mathf.Lerp(wobbleAmount.x, 0, Time.deltaTime * (calmTime));
        wobbleAmount.y = Mathf.Lerp(wobbleAmount.y, 0, Time.deltaTime * (calmTime));

        // make a sine wave of the decreasing wobble
        float wobbleX = wobbleAmount.x * Mathf.Sin(wobbleSpeed * Time.time);
        float wobbleZ = wobbleAmount.y * Mathf.Sin(wobbleSpeed * Time.time);

        transform.rotation = Quaternion.Euler(wobbleZ, 0, wobbleX);

        // velocity
        Vector3 velocity = (lastPos - transform.position) / Time.deltaTime;
        Vector3 angularVelocity = transform.parent.rotation.eulerAngles - lastRot;

        //Update amound based on velocity
        wobbleAmount.x += Mathf.Clamp((velocity.x + (angularVelocity.z * 0.2f)) * maxWobble, -maxWobble, maxWobble);
        wobbleAmount.y += Mathf.Clamp((velocity.z + (angularVelocity.x * 0.2f)) * maxWobble, -maxWobble, maxWobble);

        lastPos = transform.position;
        lastRot = transform.parent.eulerAngles;
    }
}
