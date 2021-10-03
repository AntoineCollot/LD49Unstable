using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public float spawnHeight = -2.2f;
    public float attackTime = 0.2f;
    public ParticleSystem smokeParticles = null;
    public Transform shardsParticlesPrefab = null;
    public int shardParticlesCount;
    public Collider ingredientCollider;

    bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<PickableIngredient>().onIngredientPickedUp.AddListener(OnIngredientPickedUp);
        StartCoroutine(SpawnAttack());
        Destroy(gameObject, DifficultyManager.Instance.CrystalSeriesInterval + 1.5f);
        ingredientCollider.enabled = false;
    }

    void OnIngredientPickedUp()
    {
        Destroy(gameObject, 0.5f);
    }

    private void OnDestroy()
    {
        if(Application.isPlaying)
            Destroy(Instantiate(shardsParticlesPrefab, transform.position, shardsParticlesPrefab.rotation, null).gameObject, 1);
    }

    IEnumerator SpawnAttack()
    {
        Vector3 targetPos = transform.position;
        transform.Translate(Vector3.up * spawnHeight, Space.World);
        Vector3 startPos = transform.position;
        SoundManager.PlaySound(13);

        yield return new WaitForSeconds(DifficultyManager.Instance.CrystalShowningUpTime);
        smokeParticles.Stop();

        Destroy(Instantiate(shardsParticlesPrefab, targetPos, shardsParticlesPrefab.rotation, null).gameObject, 1);

        isAttacking = true;
        float t = 0;
        SoundManager.PlaySound(14);

        while (t<1)
        {
            t += Time.deltaTime / attackTime;

            transform.position = Curves.Berp(startPos, targetPos, Mathf.Clamp01(t));

            yield return null;
        }
        isAttacking = false;
        ingredientCollider.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isAttacking && collision.collider.CompareTag("Player"))
        {
            GameManager.Instance.GameOver();
        }
    }
}
