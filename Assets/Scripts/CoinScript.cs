using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public bool isCollected;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CoinCollected()
    {
        this.isCollected = true;
        //Update the UI
        TextMeshProUGUI text = GameObject.Find("CoinCount").GetComponent<TextMeshProUGUI>();
        text.text = "Coins = " + GameObject.Find("Player").GetComponent<PlayerInventory>().GetCollectedAmount("coin");

        this.transform.gameObject.SetActive(false);
    }
}
