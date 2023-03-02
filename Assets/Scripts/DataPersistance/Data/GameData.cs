using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {

    public int coinCount;
    public SerializableDictionary<string, bool> coinsCollected;

    //Initial Values
    public GameData()
    {
        this.coinCount = 0;
        coinsCollected= new SerializableDictionary<string, bool>();
    }
}
