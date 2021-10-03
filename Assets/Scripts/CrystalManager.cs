using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalManager : MonoBehaviour
{
    public Transform crystalPrefab = null;
    public float crystalInterval = 1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCrystals());
    }

    IEnumerator SpawnCrystals()
    {
        while(!GameManager.gameIsOver)
        {
            yield return new WaitForSeconds(DifficultyManager.Instance.CrystalSeriesInterval);
            yield return StartCoroutine(CrystalSeries());
        }
    }

    IEnumerator CrystalSeries()
    {
        for (int i = 0; i < DifficultyManager.Instance.CrystalCount; i++)
        {
            if (GameManager.gameIsOver)
                yield break;

            if(MovementController.Instance.IsGrounded)
                Instantiate(crystalPrefab, MovementController.Instance.transform.position, Quaternion.Euler(crystalPrefab.eulerAngles.x,Random.Range(0,360), crystalPrefab.eulerAngles.z), transform);
            yield return new WaitForSeconds(crystalInterval);
        }
    }
}
