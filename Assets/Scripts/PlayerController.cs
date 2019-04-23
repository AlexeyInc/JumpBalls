using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    CollisionEffect _collisionEffect;

    private void Start()
    {
        _collisionEffect = GetComponentInChildren<CollisionEffect>();
    }

    void FixedUpdate()
    {
#if UNITY_EDITOR
        float moveHorizontal = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.right * moveHorizontal * speed * Time.deltaTime);

#endif

#if UNITY_ANDROID

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.x, 10));

                transform.position = Vector3.Lerp(transform.position, new Vector3(touchedPos.x, transform.position.y, 0), Time.deltaTime * speed);
            }
        }
#endif
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ball")
        { 
            Ball ballScript = BallsManager.Instance[other.gameObject];
            Color color = ballScript.Color;

            GameManager.Instance.UpdateScore(ballScript.Points);

            _collisionEffect.transform.position = other.transform.position; 
            _collisionEffect.ActiveParticle(color);
        }
    }
}
