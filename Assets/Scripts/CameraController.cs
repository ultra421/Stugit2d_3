using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float smoothing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 newCameraPos = new Vector3(playerPos.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position,newCameraPos,smoothing * Time.deltaTime);
    }
}
