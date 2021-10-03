using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootExplosion : MonoBehaviour
{
    public float explosionDelay = 2;
    public float explosionColliderRadius = 1;
    public float explosionAnimTime = 0.2f;
    public LayerMask monsterLayers = 0;
    public ParticleSystem smokeParticles = null;

    Material instancedSpriteMaterial;
    //public Renderer filling;

    // Start is called before the first frame update
    void Start()
    {
        instancedSpriteMaterial = GetComponent<SpriteRenderer>().material;
    }

    public void Explode()
    {
        StartCoroutine(ExplodeC());
        smokeParticles.Play();
    }

    IEnumerator ExplodeC()
    {
        float t = 0;
        SoundManager.PlaySound(10);
        while (t < 1)
        {
            t += Time.deltaTime / explosionDelay;

            yield return null;
        }
        float circleRadius = 0;
        float circleWidth = 1;
        float explosionRadius;
        t = 0;
        SoundManager.PlaySound(11);
        while(t<1)
        {
            t += Time.deltaTime / explosionAnimTime;

            circleRadius = Curves.QuadEaseOut(0, 1, Mathf.Clamp01(t));
            circleWidth = Curves.QuadEaseOut(.5f, 0, Mathf.Clamp01(t));
            instancedSpriteMaterial.SetFloat("_CircleRadius", circleRadius);
            instancedSpriteMaterial.SetFloat("_CircleWidth", circleWidth);
            explosionRadius = Curves.QuadEaseOut(0, explosionColliderRadius, Mathf.Clamp01(t));

            Collider[] monsters = Physics.OverlapSphere(transform.position, explosionRadius, monsterLayers);
            foreach(Collider col in monsters)
            {
                col.GetComponent<FlyingMonsterMove>().Kill((col.transform.position - transform.position).normalized);
            }

            yield return null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, explosionColliderRadius);
    }
}
