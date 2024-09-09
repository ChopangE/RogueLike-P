using System.Collections;
using System.Collections.Generic;
using UGS;
using UnityEngine;

public class PlayerDataUGS {
    public List<GameBalance.Player> list;
    PlayerDataUGS() {
        UnityGoogleSheet.Load<GameBalance.Player>();
        list = GameBalance.Player.PlayerList;
    }

}
