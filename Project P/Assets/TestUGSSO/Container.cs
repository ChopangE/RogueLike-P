using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NorskaLib.Spreadsheets;
namespace Game.DataBase {

    [System.Serializable]
    public class UnitData {
        string Id;
        int Health;
        int Damage;
    }

    [System.Serializable]
    public class SpreadsheetContent {
        [SpreadsheetPage("Units")]
        public List<UnitData> units;
    }

    [CreateAssetMenu(fileName = "SpreadsheetContainer", menuName = "spreadSheet")]
    public class Container : SpreadsheetsContainerBase
    {
        [SpreadsheetContent]
        [SerializeField] SpreadsheetContent content;
        public SpreadsheetContent Content => content;
    }
}

