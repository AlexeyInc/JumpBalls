using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Сейчас мячи просто передаются в инспекторе, в будущем менеджер должен создвать из сам

public class BallsManager : MonoBehaviour
{
    public static BallsManager Instance;

    public float speedInCorridor;

    public float flyForce_MIN;
    public float flyForce_MAX;

    private Queue<Ball> _ballsToThrow;
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
        _ballsToThrow = new Queue<Ball>();
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
                ball.transform.position = Vector3.MoveTowards(ball.transform.position, targetPoint, speedInCorridor * Time.smoothDeltaTime);
                yield return new WaitForEndOfFrame(); 
            }

            indexOfPoint++;
        }
         
        _ballsToThrow.Enqueue(ball);
    }
     
    public void GoCorridorUp(Ball ball)
    {
        StartCoroutine(GoUpCoroutine(ball)); 
    }

    IEnumerator GoUpCoroutine(Ball ball)
    {
        CorridorType corridorType = CorridorType.Up;
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
                ball.transform.position = Vector3.MoveTowards(ball.transform.position, targetPoint, speedInCorridor * Time.smoothDeltaTime);
                yield return new WaitForEndOfFrame();
            }

            indexOfPoint++;
        }

        ThrowBallInPile(ball);
    }

    private void ThrowBallInPile(Ball ball)
    {
        Quaternion direction = Quaternion.Euler(0, 0, 20);//make more clear
        float impulseForce = 5f;//make more clear

        ball.Fly(direction, impulseForce, BallMaterial.None, BallLayer.Solid);//убедиться, что последние 2 параметра необходимы
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
            _ballsToThrow.Dequeue().Fly(_curDirection, _curImpulseForse, BallMaterial.Bouncing, BallLayer.Flying);
            yield return new WaitForSeconds(0.5f);
        }
    }


    private Quaternion GetDirection()
    {
        float angleZ = Random.Range(-20, -55);
        Quaternion newRotation = Quaternion.Euler(0, 0, -50); // return angeZ
        return newRotation;
    }

    private float GetImpulseForce()
    {
        return Random.Range(flyForce_MIN, flyForce_MAX);
    }


}
