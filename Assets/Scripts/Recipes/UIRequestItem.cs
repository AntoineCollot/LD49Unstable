using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRequestItem : MonoBehaviour
{
    public Image icon = null;
    public Image progress = null;
    public int id = 0;
    Animator anim;
    int urgencyHash;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        urgencyHash = Animator.StringToHash("Urgency");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        PotionState.IngredientRequest request = PotionState.Instance.requests[id];
        icon.sprite = UIRequestList.Instance.GetSpriteOfIngredient(request.ingredient);
        progress.fillAmount = 1 - request.remainingTime01;

        anim.SetFloat(urgencyHash, 1 - request.remainingTime01);
    }
}
