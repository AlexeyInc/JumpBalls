using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMoving : MonoBehaviour
{
    public GameObject targetPoint;
    public float speed;

    void Start()
    {
        StartCoroutine(StartMove());
    }

    IEnumerator StartMove()
    {
        while (transform.position != targetPoint.transform.position)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, targetPoint.transform.position, Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }
    }
}
