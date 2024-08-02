using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public AudioClip mainClip;
    public AudioClip playClip;
    private static SoundManager instance;
    AudioHighPassFilter bgmEffect;
    AudioSource audioSource;
    public static SoundManager Instance {
        get {
            if (instance == null) {
                var obj = FindObjectOfType<SoundManager>();
                if (obj != null) {
                    instance = obj;
                }
            }
            else {
                var newObj = new GameObject().AddComponent<SoundManager>();
                instance = newObj;
            }
            return instance;
        }
    }
    void Awake() {
        var objs = FindObjectsOfType<SoundManager>();
        if (objs.Length != 1) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
    
    void Init() {
        audioSource = GetComponent<AudioSource>();
        bgmEffect = GetComponent<AudioHighPassFilter>();
        EffectBgm(false);
        PlayingMain();
    }
    public void EffectBgm(bool isPlay) {
        bgmEffect.enabled= isPlay;
    }
    void PlayingMain() {
        audioSource.clip = mainClip;
        audioSource.Play();
    }
    public void SetVolume(float volume) {
        audioSource.volume = volume;
    }
}
