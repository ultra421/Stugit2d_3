using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuCoinScript : MonoBehaviour, IDataPersistence
{
    public void LoadData(GameData data)
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = "COINS COLLECTED = " + data.coinCount;
    }

    public void SaveData(GameData data)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

}
