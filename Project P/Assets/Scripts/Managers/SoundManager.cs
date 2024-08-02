using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public AudioClip mainClip;
    public AudioClip playClip;
    
    AudioSource audioSource;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayingMain();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayingMain() {
        audioSource.clip = mainClip;
        audioSource.Play();
    }
    public void SetVolume(float volume) {
        audioSource.volume = volume;
    }
}
