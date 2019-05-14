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
     
    public Vector3 platformStartPos;
    public float offsetScaleVal; 
    public int[] numBallsToDecrease; 

    private CollisionEffect _collisionEffect;

    private int _indxToDecr;

    private Boundary _startBoundary;
    private Vector3 _touchStartPos;
    private Vector2 _touchOffset;
    private Vector3 _platformStartScale;

    private void Start()
    { 
        _collisionEffect = GetComponentInChildren<CollisionEffect>();
        _platformStartScale = transform.localScale;
        _startBoundary = new Boundary() { xMin = boundary.xMin, xMax = boundary.xMax };

        GameManager.Instance.BallsManager.BallCountChanged += DecreaseScaleX;
    }

    private void DecreaseScaleX(int ballCount)
    {
        if (_indxToDecr <= numBallsToDecrease.Length - 1)
        { 
            if (ballCount <= numBallsToDecrease[_indxToDecr])
            { 
                transform.localScale -= new Vector3(offsetScaleVal, 0, 0);
                _indxToDecr++;

                RefreshBoundary(true);
            }
        }
    } 

    private void RefreshBoundary(bool increase = false)
    {
        if (increase)
        { 
            boundary.xMax += 0.02f;
            boundary.xMin -= 0.02f;
        }
        else
        { 
            boundary.xMax -= 0.02f;
            boundary.xMin += 0.02f;
        }
    }

    void FixedUpdate()
    {
#if UNITY_EDITOR
        float moveHorizontal = Input.GetAxis("Horizontal");

        transform.position = new Vector3(transform.position.x + moveHorizontal * speedPC * Time.deltaTime, transform.position.y);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, boundary.xMin, boundary.xMax),
                                                 transform.position.y, 0);

#endif

#if UNITY_ANDROID

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            //if (touch.phase == TouchPhase.Began)
            //{
            //    _touchStartPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));

            //    offsetX = transform.position - _touchStartPos;
            //} 

            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 touchingPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                 
                Vector3 moveTo = Vector3.Lerp(transform.position, new Vector3(touchingPos.x, transform.position.y, 0), Time.deltaTime * speedPhone);
                transform.position = new Vector3(Mathf.Clamp(moveTo.x, boundary.xMin, boundary.xMax),
                                                 transform.position.y, 0);
            }

            //if (touch.phase == TouchPhase.Ended)
            //{ 
            //    offsetX = Vector3.zero;
            //}
        }
#endif 
    }

    public void Restart()
    {
        transform.position = platformStartPos;
        transform.localScale = _platformStartScale;
        boundary = new Boundary() { xMin = _startBoundary.xMin, xMax = _startBoundary.xMax };

        _indxToDecr = 0;
    }

    public void IncreasePlatformWidth()
    { 
        transform.localScale += new Vector3(offsetScaleVal, 0, 0);
        RefreshBoundary();
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
