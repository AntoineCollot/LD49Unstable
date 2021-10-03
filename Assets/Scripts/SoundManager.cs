using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource = null;
    public AudioSource sfxSource = null;

    //0 PIckUp
    //1 Toad
    //2 Mushrooms
    //3 Roots
    //4 Water
    //5 Crystal
    //6 Monster death
    //7 Monster Apparition
    //8 New Request
    //9 correct pick up
    //10 RootsBuildUp
    //11 RootsExplosion
    //12 CharacterDeath
    //13 Crystal BuildUp
    //14 Crystal attack
    public AudioClip[] clips;
    [Range(0, 1)] public float[] volumes;

    public static SoundManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleMusicMute()
    {
        if (musicSource.isPlaying)
            musicSource.Pause();
        else
            musicSource.Play();
    }

    public static void PlaySound(int id)
    {
        if (Instance == null)
            return;
        Instance.sfxSource.PlayOneShot(Instance.clips[id], Instance.volumes[id]);
    }
}
