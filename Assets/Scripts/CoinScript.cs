using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinScript : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string id;
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
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

    public void LoadData(GameData data)
    {
        //Stores the result of the map into isCollectd
        data.coinsCollected.TryGetValue(id, out isCollected);
        Debug.Log("Is this coin collected ? " + isCollected + id);
        if (isCollected)
        {
            CoinCollected();
        }
    }

    public void SaveData(GameData data)
    {
        if (data.coinsCollected.ContainsKey(id))
        {
            data.coinsCollected.Remove(id);
        }
        Debug.Log("Saving coin " + id + " collected = " + isCollected);
        data.coinsCollected.Add(id, isCollected);
        Debug.Log("Saved this = " + data.coinsCollected[id]);
    }
}
