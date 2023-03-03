using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JumpAmountPrefs : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;

    public void OnSubmitPrefs()
    {
        int jumps = int.Parse(input.text);
        PlayerPrefs.SetInt("jumpAmount", jumps);
        Debug.Log("Set jumps to " + jumps);
    }
}
