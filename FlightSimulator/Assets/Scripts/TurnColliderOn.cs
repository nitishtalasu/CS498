using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnColliderOn : MonoBehaviour
{
    public float AfterSeconds;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("TurnOn", AfterSeconds);     
    }

    void TurnOn()
    {
        // Debug.Log("Collider enabled");
        transform.GetComponent<BoxCollider>().enabled = true;
    }
}
