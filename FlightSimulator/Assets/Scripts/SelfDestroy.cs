using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float DestructionTime;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("SelfDestroyObject", DestructionTime);
    }

    void SelfDestroyObject()
    {
        Destroy(gameObject);
    }
}
