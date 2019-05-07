using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckExplosion : MonoBehaviour
{
    public float radius = 5.0F;
    public float power = 10.0F;

    public Collider2D[] colliders;

    void Start()
    {
        Invoke("Drop", 2f);
    }

    //то есть пока что так. Выключить коллайдеры на player и pocket, затем дроп и 
    //через секунду включить коллайдеры
    //сдлеать выброс с разной силой и направлением в игре

    public void Drop()
    {
        Vector3 explosionPos = transform.position;
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (rb != null)
                rb.AddForce(Vector2.up * power, ForceMode2D.Impulse);

            Debug.Log("Hit");
        }
    }
}
