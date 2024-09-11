using System.Collections;
using System.Collections.Generic;
using Game.DataBase;
using Unity.VisualScripting;
using UnityEngine;

public class ContainerTest : MonoBehaviour
{
    public Container SpreadsheetContainer;

    void Start()
    {
        var list = SpreadsheetContainer.Content.Units;
        foreach (var unit in list)
        {
            Debug.Log(unit.Health);
        }
    }
}
