using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushrooms : MonoBehaviour
{
    Animator anim;

    public Mushrooms otherMush = null;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        GetComponentInChildren<PickableIngredient>().onIngredientPickedUp.AddListener(OnIngredientPickedUp);
    }

    private void OnEnable()
    {
        anim.SetBool("Hide", false);
    }

    void OnIngredientPickedUp()
    {
        StartCoroutine(Hide());
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("Hide", true);
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
        otherMush.Show();
    }
}
