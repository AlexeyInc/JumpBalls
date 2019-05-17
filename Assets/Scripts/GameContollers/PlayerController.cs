using System.Collections;
using UnityEngine;
 
public class Boundary
{
    public float xMin, xMax;

    public Boundary(float xMin, float xMax)
    {
        this.xMin = xMin;
        this.xMax = xMax;
    }
}

public class PlayerController : MonoBehaviour
{ 
    public float speedPC;
    public float speedPhone;
     
    public Vector3 platformStartPos;
    public float offsetScaleVal; 
    public int[] numBallsToDecrease; 

    private CollisionEffect _collisionEffect;

    private int _indxToDecr;

    private Boundary _startBoundary;
    private Boundary _curBoundary;
    private Vector3 _touchStartPos;
    private Vector2 _touchOffset;
    private Vector3 _platformStartScale;

    private bool isGameActive;

    private void Start()
    { 
        _collisionEffect = GetComponentInChildren<CollisionEffect>();
        _platformStartScale = transform.localScale;

        GameManager.Instance.BallsManager.BallCountChanged += DecreaseScaleX;

        StartCoroutine(SetGameBoundary());
        //StartCoroutine(SetStartScale());
    }

    public IEnumerator SetGameBoundary()
    {
        yield return new WaitForSeconds(0.5f);

        Boundary boundary = ScreenScaler.Instance.Boundary;
        _startBoundary = new Boundary(boundary.xMin, boundary.xMax);
        _curBoundary = new Boundary(boundary.xMin, boundary.xMax);
    }

    //public IEnumerator SetStartScale()
    //{
    //    yield return new WaitForSeconds(0.5f);

    //    transform.localScale = new Vector3(ScreenScaler.Instance.ScaleX, 1f);

    //    _platformStartScale = transform.localScale;
    //}

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
            _curBoundary.xMax += 0.02f;
            _curBoundary.xMin -= 0.02f;
        }
        else
        {
            _curBoundary.xMax -= 0.02f;
            _curBoundary.xMin += 0.02f;
        }
    }

    void FixedUpdate()
    {
        
        if (isGameActive)
        {
#if UNITY_EDITOR || UNITY_WEBGL
            float moveHorizontal = Input.GetAxis("Horizontal");

            transform.position = new Vector3(transform.position.x + moveHorizontal * speedPC * Time.deltaTime, transform.position.y);

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, _curBoundary.xMin, _curBoundary.xMax),
                                                     transform.position.y, 0);

#endif

#if UNITY_ANDROID || UNITY_IOS

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                float xPos = Input.GetTouch(0).deltaPosition.x * speedPhone * Time.smoothDeltaTime;

                this.transform.Translate(new Vector2(xPos, 0));

                transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, _curBoundary.xMin, _curBoundary.xMax),
                                                             transform.position.y, 0); 
            }
#endif 
        } 
    }

    public void Restart()
    {
        transform.position = platformStartPos;
        transform.localScale = _platformStartScale;
        _curBoundary = new Boundary(_startBoundary.xMin, _startBoundary.xMax);

        _indxToDecr = 0;
    }

    public void IncreasePlatformWidth()
    { 
        transform.localScale += new Vector3(offsetScaleVal, 0, 0);
        RefreshBoundary();
    } 

    public void IsGameActive(bool value)
    {
        isGameActive = value;
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
