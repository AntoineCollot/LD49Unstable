using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Transform flyingMonsterPrefab = null;
    public ParticleSystem monsterSpawnParticles = null;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnFlyingMonsters());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnFlyingMonsters()
    {
        while(!GameManager.gameIsOver)
        {
            yield return new WaitForSeconds(DifficultyManager.Instance.MonsterSpawnTime);

            monsterSpawnParticles.Play();

            yield return new WaitForSeconds(2);

            Instantiate(flyingMonsterPrefab, transform.position, Quaternion.identity, transform);

        }
    }
}
