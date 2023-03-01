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
        
    }

    public void SaveData(GameData data)
    {
        
    }
}
