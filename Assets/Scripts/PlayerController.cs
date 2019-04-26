 using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax;
}

public class PlayerController : MonoBehaviour
{
    public Boundary boundary;
    public float speed;

    private CollisionEffect _collisionEffect;

    public float decreaseScaleVal; 
    public int[] numBallsToDecrease;
    int _indxToDecr;

    private void Start()
    {
        _collisionEffect = GetComponentInChildren<CollisionEffect>();

        BallsManager.Instance.BallCountChanged += DecreaseScaleX;
    }

    private void DecreaseScaleX(int ballCount)
    {
        if (_indxToDecr <= numBallsToDecrease.Length - 1)
        { 
            if (ballCount <= numBallsToDecrease[_indxToDecr])
            {
                transform.localScale = new Vector2(transform.localScale.x - decreaseScaleVal, transform.localScale.y);
                _indxToDecr++;
            }
        }
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

                Vector3 moveTo = Vector3.Lerp(transform.position, new Vector3(touchedPos.x, transform.position.y, 0), Time.deltaTime * speed);
                transform.position = new Vector3(Mathf.Clamp(moveTo.x, boundary.xMin, boundary.xMax), 
                                                 transform.position.y, 0);
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
             
            _collisionEffect.transform.position = other.transform.position; 
            _collisionEffect.ActiveParticle(color);
        }
    }
}
