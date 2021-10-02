using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInstabilityBar : MonoBehaviour
{
    public Image progressImage;

    // Update is called once per frame
    void LateUpdate()
    {
        progressImage.fillAmount = PotionState.Instance.instability01;
    }
}
