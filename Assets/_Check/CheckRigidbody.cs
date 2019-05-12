using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRigidbody : MonoBehaviour
{
    Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void ChangeMassRigidbody()
    {
        if (_rigidbody == null)
        {
            Debug.Log("null rb");
        }
        _rigidbody.mass = 2;
    }
}
