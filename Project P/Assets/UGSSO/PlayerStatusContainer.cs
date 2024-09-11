using System.Collections;
using System.Collections.Generic;
using NorskaLib.Spreadsheets;
using UnityEngine;

namespace Game.DataBase
{
    
    [System.Serializable]
    public class PlayerStatus
    {
        public int Health;
        public int Attack;
        public float Speed;
        public float Jump;
    }

    [System.Serializable]
    public class PlayerUpgrade
    {
        public int UpgradeHealth;
        public int UpgradeAttack;
        public float UpgradeSpeed;
        public float UpgradeJump;
    }
    [System.Serializable]
    public class Contents
    {
        [SpreadsheetPage("PlayerStat")] 
        public List<PlayerStatus> Stats;
        [SpreadsheetPage("PlayerUpgrade")]
        public List<PlayerUpgrade> Upgrades;
    }
    
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "PlayerStats")]
    public class PlayerStatusContainer : SpreadsheetsContainerBase
    {
        [SpreadsheetContent]
        [SerializeField] Contents content;
        public Contents Content => content;
        
    }
    
    
}