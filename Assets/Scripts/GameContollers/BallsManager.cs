using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
 
public class BallsManager : MonoBehaviour
{ 
    public Transform ballsContainer;
    public GameObject ballPrefab;

    [Header("Balls_Settings")]
    public Transform ballsSpawnPosition;
    public int countBalls;
    public Color[] ballsColors;
    public Vector3[] ballScaleSizes;
    public float[] tailWidth; 

    
    [Header("Corridors_Settings")]
    public float speedDownCorridor;
    public float speedUpCorridor;
    public float upAcceleration;

    [Header("Throwing_Settings")] 
    public float flyForce_MIN;
    public float flyForce_MAX;

    public float flyDirection_from;
    public float flyDirection_to;

    public event Action<int> BallCountChanged;

    private Dictionary<GameObject, Ball> _balls;
    private Ball _ballToThrow; 
     
    private CorridorsConductor _corridorsConductor;

    private float _curImpulseForse;
    private Quaternion _curDirection;
    private int _countBallsToThrow; 

    private int _curCountBallsInGame;
    private bool gameStart = false;
    private float _speedDownOffset = 2.2f;

    private void Start()
    {  
        SetupBalls();
    } 

    private void SetupBalls()
    {
        _balls = new Dictionary<GameObject, Ball>();

        float offsetX = 0.25f;
        float tempOffsetX = 0;
        float offsetY = 0.25f;
        float tempOffsetY = 0;

        for (int i = 0; i < countBalls; i++)
        {
            Vector3 ballPos = new Vector3(ballsSpawnPosition.position.x + tempOffsetX, ballsSpawnPosition.position.y + tempOffsetY);
            tempOffsetX += offsetX;

            GameObject newBall = Instantiate(ballPrefab, ballPos, Quaternion.identity);
            newBall.transform.parent = ballsContainer;
            Ball ballScript = newBall.GetComponent<Ball>();
            _balls.Add(newBall, ballScript);

            if (i % 10 == 0)
            {
                tempOffsetX = 0;
                tempOffsetY += offsetY;
            }
        } 
    } 

    public void Restart()
    {
        foreach (var ball in _balls)
        {
            Destroy(ball.Key);
        }

        StopAllCoroutines();

        _corridorsConductor.ClearAllOccupidPoints();
        gameStart = false; 

        SetupBalls(); 
    }

    public void Initialize(CorridorsConductor corridorsConductor)
    {
        _corridorsConductor = corridorsConductor;
    }
      
    public void GoCorridorDown(Ball ball)
    {
        StartCoroutine(GoDownCoroutine(ball));
    }
     
    IEnumerator GoDownCoroutine(Ball ball)
    {
        CorridorType corridorType = CorridorType.Down; 
        Vector3 lastPointInPath = _corridorsConductor.GetLastPointInPath(corridorType);

        int indexOfPoint = 0;
        while (lastPointInPath != ball.transform.position)
        { 
            while (_corridorsConductor.IsPointOccupied(indexOfPoint, corridorType))
            { 
                yield return new WaitForSeconds(0.2f);
            }
            if (indexOfPoint > 0)  
            {
                int prevPoint = indexOfPoint - 1;
                _corridorsConductor.SetOccupiedPointInPath(prevPoint, false, corridorType);
            }
            _corridorsConductor.SetOccupiedPointInPath(indexOfPoint, true, corridorType);
             
            Vector3 targetPoint = _corridorsConductor.GetPointPosition(indexOfPoint, corridorType);  
            while (ball.transform.position != targetPoint)
            { 
                ball.transform.position = gameStart ? Vector3.MoveTowards(ball.transform.position, targetPoint, (speedDownCorridor - _speedDownOffset) * Time.smoothDeltaTime) : 
                                                        Vector3.MoveTowards(ball.transform.position, targetPoint, speedDownCorridor * Time.smoothDeltaTime);
                yield return new WaitForEndOfFrame(); 
            }

            indexOfPoint++;
        }

        _ballToThrow = ball;
    }
     
    public void GoCorridorUp(Ball ball)
    {
        StartCoroutine(GoUpCoroutine(ball)); 
    }

    IEnumerator GoUpCoroutine(Ball ball)
    {
        CorridorType corridorType = CorridorType.Up;
        Vector3 lastPointInPath = _corridorsConductor.GetLastPointInPath(corridorType);
        float localSpeed = speedUpCorridor;

        int indexOfPoint = 0;
        while (lastPointInPath != ball.transform.position)
        { 
            Vector3 targetPoint = _corridorsConductor.GetPointPosition(indexOfPoint, corridorType);
            while (ball.transform.position != targetPoint)
            {
                ball.transform.position = Vector3.MoveTowards(ball.transform.position, targetPoint, localSpeed * Time.smoothDeltaTime);
                localSpeed += upAcceleration;
                yield return new WaitForEndOfFrame();
            }

            indexOfPoint++;
        }

        ThrowBall(ball, Quaternion.Euler(0, 0, 60), impulseForce: 5f);
    }

    private void ThrowBall(Ball ball, Quaternion direction, float impulseForce)
    {
        ball.Fly(direction, impulseForce, BallMaterial.Standard, BallLayer.Solid, false);  
    }

    public void ThrowBalls(int count)
    {  
        _countBallsToThrow = count;

        _curDirection = GetDirection();
        _curImpulseForse = GetImpulseForce();

        StartCoroutine(ThrowCourutine());
    }

    IEnumerator ThrowCourutine()
    {
        while (_countBallsToThrow > 0)
        {
            while (_ballToThrow == null)
            {
                yield return new WaitForSeconds(0.1f);
            }
            if (!gameStart)
            {
                gameStart = true;
            }

            int lastPointDownCorridor = _corridorsConductor.GetPathLength(CorridorType.Down);
            _corridorsConductor.SetOccupiedPointInPath(lastPointDownCorridor - 1, false, CorridorType.Down);

            _ballToThrow.Fly(_curDirection, _curImpulseForse, BallMaterial.Bouncing, BallLayer.Flying);
            _ballToThrow = null;

            _countBallsToThrow--;

            yield return new WaitForSeconds(0.125f);
        }
    }

    public int CountBallsInGame()
    {
        int counter = 0;
        foreach (var ball in _balls)
        {
            if (ball.Value.InGame)
            {
                counter++;
            }
        } 

        return counter; 
    } 

    public bool IsEnaughBallsOnGround()
    {
        return (countBalls - CountBallsInGame()) > (countBalls / 2);
    }
     
    private Quaternion GetDirection()
    {
        float angleZ = Random.Range(flyDirection_from, flyDirection_to);
        Quaternion newRotation = Quaternion.Euler(0, 0, angleZ); // -50
        return newRotation;
    }

    private float GetImpulseForce()
    {
        return Random.Range(flyForce_MIN, flyForce_MAX); ;//
    }  

    public void BallOutOfGame(bool OutOfGameZone = false)
    {
        if (OutOfGameZone)
        {
            countBalls--;
        }

        int ballsLeft = CountBallsInGame();

        if (ballsLeft == 0)
        {
            GameManager.Instance.GameOver();
        }
        else
        {
            BallCountChanged(ballsLeft);
        }
    }
     
    public void UpgrateBallLevel(GameObject ballObj)
    {
        int nextColorIndx = Array.IndexOf(ballsColors, _balls[ballObj].Color) + 1;
        if (ballsColors.Length > nextColorIndx)
        {
            Color newColor = ballsColors[nextColorIndx];
            _balls[ballObj].UpdateColor(newColor);
        }
        else
            Debug.Log("index out of range (ball have mac level)");
    }

    public void UpgrateBallSize(GameObject ballObj)
    {
        _balls[ballObj].UpdateScaleSize(ballScaleSizes[1], tailWidth[1]); 
    }

    public void CreateExtraBall(GameObject ballObj)
    {
        if (ballObj.layer == LayerMask.NameToLayer("FlyingBall"))
        {
            Vector3 ballPosition = ballObj.transform.position;
            ballObj.layer = LayerMask.NameToLayer("SolidBall"); //for prevent massive balls instantiation

            InstantiateExtraBall(ballPosition);
        }
    }

    private void InstantiateExtraBall(Vector3 pos)
    {
        GameObject newBall = Instantiate(ballPrefab, pos, Quaternion.identity);
        newBall.transform.parent = ballsContainer; 
        Ball ballScript = newBall.GetComponent<Ball>(); 
        _balls.Add(newBall, ballScript); 

        ThrowBall(ballScript, Quaternion.Euler(0, 0, -40), impulseForce: 4.5f);
    } 
     
    public Ball this[GameObject ballObj]
    {
        get
        {
            if (_balls.ContainsKey(ballObj))
            {
                return _balls[ballObj];
            }
            else
            {
                Debug.LogError("Key doesn't exist!");
                return null;
            }
        }
    }
}
