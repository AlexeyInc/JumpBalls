using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    { 
        //StartCoroutine(GameLoop());
    } 

    IEnumerator GameLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            BallsManager.Instance.ThrowBalls(2);

            yield return new WaitForSeconds(3f);
        }

    }
}
