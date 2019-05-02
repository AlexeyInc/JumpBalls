 using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax;
}

public class PlayerController : MonoBehaviour
{
    public Boundary boundary;
    public float speedPC;
    public float speedPhone;

    private CollisionEffect _collisionEffect;

    public float offsetScaleVal; 
    public int[] numBallsToDecrease;

    private int _indxToDecr;
    private Vector3 _platformStartScale;
    private Vector3 _platformStartPos;

    private Vector2 _touchStartPos;
    private Vector2 _touchOffset;

    private void Start()
    {
        _collisionEffect = GetComponentInChildren<CollisionEffect>();
        _platformStartScale = transform.localScale;

        GameManager.Instance.BallsManager.BallCountChanged += DecreaseScaleX;
    }

    private void DecreaseScaleX(int ballCount)
    {
        if (_indxToDecr <= numBallsToDecrease.Length - 1)
        { 
            if (ballCount <= numBallsToDecrease[_indxToDecr])
            {
                transform.localScale = new Vector2(transform.localScale.x - offsetScaleVal, transform.localScale.y);
                _indxToDecr++;
            }
        }
    }

    void FixedUpdate()
    {
#if UNITY_EDITOR
        float moveHorizontal = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.right * moveHorizontal * speedPC * Time.deltaTime);

#endif

#if UNITY_ANDROID

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            //if (touch.phase == TouchPhase.Began)
            //{
            //    _touchStartPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
            //}
            //else if (touch.phase == TouchPhase.Moved)
            //{
            //    _touchOffset = touch.position - _touchStartPos;
            //}
            //else if (touch.phase == TouchPhase.Ended)
            //{
            //    _touchOffset = Vector3.zero;
            //}


            //Debug.Log("X: " + _touchOffset.x);
            //Debug.Log("VAL: " + (_touchOffset.x > 0).ToString());

            //Vector3 target = new Vector3(transform.position.x + _touchOffset.x, _touchOffset.y, 0);
            //Vector3 moveTo = Vector3.Lerp(transform.position, target, Time.deltaTime * speedPhone);

            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));

                Vector3 moveTo = Vector3.Lerp(transform.position, new Vector3(touchedPos.x, transform.position.y, 0), Time.deltaTime * speedPhone);
                transform.position = new Vector3(Mathf.Clamp(moveTo.x, boundary.xMin, boundary.xMax),
                                                 transform.position.y, 0);
            }
        }
#endif
        Debug.Log("pos - " + transform.position);
    }

    public void Restart()
    {
        transform.position = _platformStartPos;
        transform.localScale = _platformStartScale; 
    }

    public void IncreasePlatformWidth()
    {
        transform.localScale = new Vector2(transform.localScale.x + offsetScaleVal, transform.localScale.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ball")
        { 
            Ball ballScript = GameManager.Instance.BallsManager[other.gameObject];
            Color color = ballScript.Color;
             
            _collisionEffect.transform.position = other.transform.position; 
            _collisionEffect.ActiveParticle(color);
        }
    }
}
