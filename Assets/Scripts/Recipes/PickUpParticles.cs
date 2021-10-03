using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpParticles : MonoBehaviour
{
    public float animTime = 1;
    public float bonusHeight = 1.5f;
    public Transform target;
    ParticleSystem particles;
    ParticleSystem.MainModule particlesMain;

    public Color correctColor;
    public Color inccorectColor;

    public static PickUpParticles Instance;

    void Awake()
    {
        Instance = this;
        particles = GetComponent<ParticleSystem>();
        particlesMain = particles.main;
    }

    public void PickUpAnimation(Vector3 origin)
    {
        StopAllCoroutines();
        StartCoroutine(PickUpC(origin));
    }

    public void SetCorrect(bool value)
    {
        if (value)
            particlesMain.startColor = correctColor;
        else
            particlesMain.startColor = inccorectColor;
    }

    IEnumerator PickUpC(Vector3 origin)
    {
        particles.Play();
        float t = 0;
        while(t<1)
        {
            t += Time.deltaTime / animTime;

            Vector3 pos = Curves.QuadEaseIn(origin, target.position, Mathf.Clamp01(t));
            pos.y = Mathf.Lerp(pos.y, pos.y + bonusHeight, Mathf.PingPong(t * 2, 1));
            transform.position = pos;

            yield return null;
        }
        particles.Stop();
    }
}
