using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NorskaLib.Spreadsheets;



namespace Game.DataBase {

    [System.Serializable]
    public class UnitData {
        public string Id;
        
        public int Health;
        public int Damage;
    }

    [System.Serializable]
    public class SpreadsheetContent {
        [SpreadsheetPage("Units")]
        public List<UnitData> Units;
    }

    [CreateAssetMenu(fileName = "SpreadsheetContainer", menuName = "spreadSheet")]
    public class Container : SpreadsheetsContainerBase
    {
        [SpreadsheetContent]
        [SerializeField] SpreadsheetContent content;
        public SpreadsheetContent Content => content;
    }
}

