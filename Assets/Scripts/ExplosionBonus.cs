using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBonus : MonoBehaviour
{
    public GameObject pocket; 
    public float explosionPower;
    public float timeFly;

    private Collider2D _playerCollider;
    private Collider2D[] _pocketColliders; 

    private List<Collider2D> _goThoughColliders;
    private List<Rigidbody2D> _balls;

    private void Start()
    { 
        _balls = new List<Rigidbody2D>();
        _pocketColliders = pocket.GetComponents<Collider2D>();
         
        _goThoughColliders = new List<Collider2D>();
        _goThoughColliders.AddRange(_pocketColliders);
    }
     
    public void MakeExplosion()
    {
        StartCoroutine(ExplosionProcess()); 
    } 

    public IEnumerator ExplosionProcess()
    { 
        GameManager.Instance.ActiveGameLoop(false);

        GameManager.Instance.PlayerController.IsGameActive(true); //Allow player move
        //-------------------------------
        DiactiveCollidersForBallsFlight();
         
        //-------------------------------
        float massInc = 0.5f;
        foreach (Rigidbody2D rb in _balls)
        {
            if (rb != null)
            {
                rb.mass -= massInc;
            }
        }

        float powerRange = 1.5f;
        foreach (Rigidbody2D rb in _balls)
        {
            float force = Random.Range(explosionPower - powerRange, explosionPower + powerRange);
            if (rb != null)
                rb.AddForce(Vector2.up * force, ForceMode2D.Impulse); 
        }
        yield return new WaitForSeconds(timeFly);

        foreach (Rigidbody2D rb in _balls)
        {
            if (rb != null)
            { 
                rb.mass += massInc;
            }
        }
        _balls.Clear();
        _balls = new List<Rigidbody2D>();
        //------------------------------- 
        ActiveCollidersAfterBallsFlight(); 
        //--------------------------------
        yield return new WaitForSeconds(4f);
         
        GameManager.Instance.ActiveGameLoop(true); 

    }

    private void DiactiveCollidersForBallsFlight()
    {
        if (_playerCollider == null)
        {
            _playerCollider = GameManager.Instance.PlayerController.gameObject.GetComponent<Collider2D>();
            _goThoughColliders.Add(_playerCollider);
        }

        foreach (Collider2D col in _goThoughColliders)
        {
            //col.isTrigger = true;
            col.enabled = false;
        }
    }

    private void ActiveCollidersAfterBallsFlight()
    {
        if (_goThoughColliders != null)
        { 
            foreach (Collider2D col in _goThoughColliders)
            {
                //col.isTrigger = true;
                col.enabled = true;
            }
        }
    } 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ball")
        {
            Rigidbody2D ballRb2D = other.GetComponent<Rigidbody2D>();
            _balls.Add(ballRb2D);
             
            //Debug.Log("Count ground balls: " + _balls.Count);
            //Debug.Log("Count balls in game: " + GameManager.Instance.BallsManager.CoutBalls);
        }
    }
}
