using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreatureFX : MonoBehaviour
{

    [Header("Pop up Text")]
    [SerializeField] private GameObject popUpTextPrefab; 
    public void CreatePopUpText(string _text, Vector3 pos)
    {
        Vector3 position = pos; 

        GameObject newText = Instantiate(popUpTextPrefab, position, Quaternion.identity);
        newText.GetComponent<TextMeshPro>().text = _text; 
     
    }
}
