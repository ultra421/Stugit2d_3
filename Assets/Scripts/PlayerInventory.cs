using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, IDataPersistence
{

    public SerializableDictionary<string,int> inventory;
    // Start is called before the first frame update
    void Start()
    {
        if (inventory == null) { inventory = new SerializableDictionary<string,int>(); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collided = collision.gameObject;
        if (collided.CompareTag("Collectible") && (collided.GetComponent<CoinScript>() != null)) //Coin case
        {
            //If coin is not collected
            if (!collided.GetComponent<CoinScript>().isCollected) { CollectCoin(collided); }
        }
    }

    private void CollectCoin(GameObject coin)
    {
        CoinScript coinScript = coin.GetComponent<CoinScript>();
        //In case inventory coin is not yet initialized
        if (!inventory.ContainsKey("coin")) { inventory["coin"] = 0; }
        inventory["coin"]++;
        coinScript.CoinCollected(); //Collect the coin
        Debug.Log("Collected a coin! current = " + GetCollectedAmount("coin"));
    }

    public int GetCollectedAmount(string collectibleName)
    {
       if (inventory.ContainsKey("coin"))
        {
            return inventory["coin"];
        } else
        {
            return 0;
        }
    }
    public void LoadData(GameData data)
    {
        Debug.Log("found " + data.coinCount + " coins");
        if (inventory == null) { inventory = new SerializableDictionary<string, int>(); }
        inventory["coin"] = data.coinCount;
        UpdateGui();
    }

    public void SaveData(GameData data)
    {
        data.coinCount = GetCollectedAmount("coin");
    }

    public void UpdateGui()
    {
        TextMeshProUGUI text = GameObject.Find("CoinCount").GetComponent<TextMeshProUGUI>();
        text.text = "Coins = " + inventory["coin"];
    }
}
