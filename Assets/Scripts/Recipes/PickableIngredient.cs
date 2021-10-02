using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableIngredient : MonoBehaviour
{
    Material instancedMat;
    float outlineWidth;
    public float highlightOutlineWidth = 0.001f;
    public float outlineAnimSmooth = 0.05f;
    float refOutlineWidth = 0;

    bool isHighlighted = false;

    public Ingredient ingredient;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        instancedMat = GetComponentInParent<Renderer>().material;
        anim = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isHighlighted && outlineWidth < highlightOutlineWidth)
        {
            outlineWidth = Mathf.SmoothDamp(outlineWidth, highlightOutlineWidth, ref refOutlineWidth, outlineAnimSmooth);
            instancedMat.SetFloat("_Outline", outlineWidth);
        }
        else if (!isHighlighted && outlineWidth > 0)
        {
            outlineWidth = Mathf.SmoothDamp(outlineWidth, 0, ref refOutlineWidth, outlineAnimSmooth);
            instancedMat.SetFloat("_Outline", outlineWidth);
        }
    }

    public void Highlight(bool value)
    {
            isHighlighted = value;
    }

    public Ingredient PlayerPickUp()
    {
        anim?.SetTrigger("PickUp");
        return ingredient;
    }
}
