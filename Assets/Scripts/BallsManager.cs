using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Сейчас мячи просто передаются в инспекторе, в будущем менеджер должен создвать из сам

public class BallsManager : MonoBehaviour
{
    public static BallsManager Instance;

    public GameObject ballPrefab;

    [Header("Balls_Settings")]
    public Transform ballsSpawnPosition;
    public int countBalls;
    
    [Header("Corridors_Settings")]
    public float speedDownCorridor;
    public float speedUpCorridor;

    [Header("Throwing_Settings")] 
    public float flyForce_MIN;
    public float flyForce_MAX;
     

    private Ball _ballToThrow;
     
    private CorridorsConductor _corridorsConductor;

    private float _curImpulseForse;
    private Quaternion _curDirection;
    private int _countBallsToThrow;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        InstantiateBalls(); 
    }

    private void InstantiateBalls()
    {
        float offsetX = 0.25f;
        float tempOffsetX = offsetX;
        float offsetY = 0.25f;
        float tempOffsetY = 0;

        for (int i = 0; i < countBalls; i++)
        {
            Vector3 ballPos = new Vector3(ballsSpawnPosition.position.x + tempOffsetX, ballsSpawnPosition.position.y + tempOffsetY);
            tempOffsetX += offsetX;

            Instantiate(ballPrefab, ballPos, Quaternion.identity);

            if (i / 5 == 0)
            {
                tempOffsetX = offsetX;
                tempOffsetY += offsetY;
            }
        }
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
                yield return new WaitForSeconds(0.3f); 
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
                ball.transform.position = Vector3.MoveTowards(ball.transform.position, targetPoint, speedDownCorridor * Time.smoothDeltaTime);
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
                localSpeed += 0.05f;//up acceleration
                yield return new WaitForEndOfFrame();
            }

            indexOfPoint++;
        }

        ThrowBallToPile(ball);
    }

    private void ThrowBallToPile(Ball ball)
    {
        Quaternion direction = Quaternion.Euler(0, 0, 20); 
        float impulseForce = 12f; 
         
        ball.Fly(direction, impulseForce, BallMaterial.Standard, BallLayer.Solid); 
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
            if (true) //check on balls more than 0 in game now
            {
                while (_ballToThrow == null)
                {
                    yield return new WaitForSeconds(0.5f);
                }
            }
            int lastPointDownCorridor = _corridorsConductor.GetPathLength(CorridorType.Down);
            _corridorsConductor.SetOccupiedPointInPath(lastPointDownCorridor - 1, false, CorridorType.Down);

            _ballToThrow.Fly(_curDirection, _curImpulseForse, BallMaterial.Bouncing, BallLayer.Flying);
            _ballToThrow = null;

            _countBallsToThrow--;

            yield return new WaitForSeconds(0.2f); //make more clear
        }
    }
     
    private Quaternion GetDirection()
    {
        float angleZ = Random.Range(-20, -35);
        Quaternion newRotation = Quaternion.Euler(0, 0, -50); // return angeZ
        return newRotation;
    }

    private float GetImpulseForce()
    {
        return Random.Range(flyForce_MIN, flyForce_MAX);
    } 
}
