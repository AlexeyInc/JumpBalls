using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInit : MonoBehaviour
{
    public GameObject cube_;
     
    public void OnClick()
    {
        GameObject cube = Instantiate(cube_, Vector3.zero, Quaternion.identity);
        CheckRigidbody script = cube.GetComponent<CheckRigidbody>();

        script.ChangeMassRigidbody();

    }
}
