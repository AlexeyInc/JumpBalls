using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    public static Catapult Instance;

    public GameObject ball;

    public float impulseForce_MIN;
    public float impulseForce_MAX;

    private GameObject _nextBallToThrow;
    
    private float _numBallsToThrow;
    private bool _throwingState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void LaunchNextBallsCatapulting(int countBalls)
    {
        _numBallsToThrow = countBalls;

        

        CatapultBall(_nextBallToThrow);

        _throwingState = true;
    }

    

    private void CatapultBall(GameObject ballObj)
    {
        //Ball curball = BallsManager.Instance.GetBallScript(ballObj);

        //curball.ThrowBall(_curDirection, _curThrustForse);

        if (_numBallsToThrow > 0)
        {
            _numBallsToThrow--;
        }
        else
        {
            _throwingState = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ball")
        {
            Debug.Log("OnTriggerEnter2D " + other.name);

            if (_throwingState == false)
            {
                _nextBallToThrow = other.gameObject;

                if (_nextBallToThrow.layer != LayerMask.NameToLayer("flyingBall"))
                {
                    //Ball ball = BallsManager.Instance.GetBallScript(_nextBallToThrow);
                    //ball.SetRigidbodyType(RigidbodyType2D.Static);
                }
            }
            else
            {
                CatapultBall(other.gameObject);
            }
        }
    }
}
