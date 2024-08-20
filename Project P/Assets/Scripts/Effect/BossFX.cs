using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFX : MonsterFX
{
    private SpriteRenderer sr;

    [Header("FlashFX")]
    [SerializeField] private Material hitMat;
    private Material originalMat;

    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        yield return new WaitForSeconds(.2f);
        sr.material = originalMat;
    }
}
