using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMoving : MonoBehaviour
{
    public GameObject targetPoint;
    public float speed;

    void Start()
    {
        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 20); ;

        GetComponent<Rigidbody2D>().AddForce(this.transform.up * 5f, ForceMode2D.Impulse);

        Debug.Log("push");
        //StartCoroutine(StartMove());
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
