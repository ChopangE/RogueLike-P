using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBar : MonoBehaviour
{
    AudioSource audioSource;
    void Start() {
        audioSource = FindAnyObjectByType<AudioSource>();
    }
    public void SetVolume_(float volume) {
        audioSource.volume = volume;
    }


}
