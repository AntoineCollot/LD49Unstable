using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    [Header("Time")]
    public float maxTime;
    public float difficulty01 { get => Mathf.Clamp01(Time.time / maxTime); }

    [Header("Potion")]
    public float minInstabilityProgressTime = 5;
    public float maxInstabilityProgressTime = 2;
    public float InstabilityProgressTime { get => Mathf.Lerp(minInstabilityProgressTime, maxInstabilityProgressTime, difficulty01); }

    public float minRequestTotalTime = 7;
    public float maxRequestTotalTime = 3;
    public float RequestTotalTime { get => Mathf.Lerp(minRequestTotalTime, maxRequestTotalTime, difficulty01); }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        print(difficulty01);
    }
}
