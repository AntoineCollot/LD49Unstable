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

    [Header("Crystals")]
    public float minCrystalCount = 2;
    public float maxCrystalCount = 6;
    public int CrystalCount { get => Mathf.FloorToInt(Mathf.Lerp(minCrystalCount, maxCrystalCount, difficulty01)); }

    public float minCrystalSeriesInterval = 10;
    public float maxCrystalSeriesInterval = 4;
    public float CrystalSeriesInterval { get => Mathf.Lerp(minCrystalSeriesInterval, maxCrystalSeriesInterval, difficulty01); }

    public float minCrystalShowningUpTime = 3;
    public float maxCrystalShowningUpTime = 1;
    public float CrystalShowningUpTime { get => Mathf.Lerp(minCrystalShowningUpTime, maxCrystalShowningUpTime, difficulty01); }

    [Header("Monster")]
    public float minMonsterSpawnTime = 10;
    public float maxMonsterSpawnTime = 3;
    public float MonsterSpawnTime { get => Mathf.Lerp(minMonsterSpawnTime, maxMonsterSpawnTime, difficulty01); }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }
}
