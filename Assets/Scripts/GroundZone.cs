using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ball")
        {
            GameManager.Instance.BallsManager[other.gameObject].SetOutOFGameState(); 
            GameManager.Instance.BallsManager.BallOutOfGame();
        }

        //Debug.Log("All: " + GameManager.Instance.BallsManager.CoutBalls);

        int ingame = GameManager.Instance.BallsManager.CountBallsInGame;
        //Debug.Log("On ground: " + (GameManager.Instance.BallsManager.CoutBalls - ingame).ToString());
        //Debug.Log("In game: " + ingame);

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ball")
        {
            GameManager.Instance.BallsManager[other.gameObject].SetInGameState();
        }
    }
}
