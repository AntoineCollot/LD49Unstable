using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBoneHeight : MonoBehaviour
{
    public Transform bone = null;
    float delta;

    private void Start()
    {
        delta = transform.position.y - bone.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = bone.position.y +delta;
        transform.position = pos;
    }
}
