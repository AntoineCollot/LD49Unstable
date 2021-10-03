using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BendGrass : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector2 v = new Vector2(transform.position.x, transform.position.z);
        Shader.SetGlobalVector("_BenderPosition", v);
    }
}
