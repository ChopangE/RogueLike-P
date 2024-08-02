using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBar : MonoBehaviour
{
    public void SetVolume_(float volume) {
        SoundManager.Instance.SetVolume(volume);
    }


}
