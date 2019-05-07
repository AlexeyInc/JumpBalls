using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRigidbody : MonoBehaviour
{
    Rigidbody rigidbody;
    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void ChangeMassRigidbody()
    {
        if (rigidbody == null)
        {
            Debug.Log("null rb");
        }
        rigidbody.mass = 2;
    }
}
