using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {

    public int coinCount;
    public Dictionary<string, bool> coinsCollected;

    //Initial Values
    public GameData()
    {
        this.coinCount = 0;
        coinsCollected= new Dictionary<string, bool>();
    }
}
