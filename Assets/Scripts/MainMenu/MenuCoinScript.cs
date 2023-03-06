using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuCoinScript : MonoBehaviour, IDataPersistence
{
    public void LoadData(GameData data)
    {
        TextMeshProUGUI coinText = gameObject.GetComponent<TextMeshProUGUI>();
        if (data != null)
        {
            coinText.text = "COINS COLLECTED = " + data.coinCount;
        } else
        {
            coinText.text = "COINS COLLECTED = 0";
        }
    }

    public void SaveData(GameData data)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

}
