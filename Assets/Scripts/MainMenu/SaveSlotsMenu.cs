using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotsMenu : MonoBehaviour
{
    private SaveSlot[] saveSlots;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    public void LoadSave(int slot)
    {
        SetSlotListActive(false);
        DataPersistanceManager.instance.SetProfileId(slot);
        SetSlotListActive(true);
    }

    private void SetSlotListActive(bool input)
    {
        foreach (SaveSlot saveSlot in saveSlots)
        {
            saveSlot.gameObject.SetActive(input);
        }
    }


}
