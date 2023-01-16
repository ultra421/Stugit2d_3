using System.Collections;
using System.Collections.Generic;
using 
using UnityEngine;

public class ConfigReader : MonoBehaviour
{
    [SerializeField] TextAsset file;

    private class TestClass
    {
        public int FirstField { get; set; }
        public string SecondField { get; set;}
    }
    private void Start()
    {

    }

}
