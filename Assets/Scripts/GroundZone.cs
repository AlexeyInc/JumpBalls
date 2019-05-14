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
    }
     
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ball")
        {
            GameManager.Instance.BallsManager[other.gameObject].SetInGameState();
        }
    }
}
