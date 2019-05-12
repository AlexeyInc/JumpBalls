using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBonus : MonoBehaviour
{
    public GameObject pocket;
    public Collider2D _returnInGameCollider;
    public Collider2D _groundCollider;
    public float explosionPower;
    public float timeFly = 2f;

    private Collider2D _playerCollider;
    private Collider2D[] _pocketColliders;
    private Collider2D _col;

    private List<Collider2D> _goThoughColliders;
    private List<Rigidbody2D> _balls;

    private void Start()
    {
        _col = GetComponent<Collider2D>();
        _balls = new List<Rigidbody2D>();
        _pocketColliders = pocket.GetComponents<Collider2D>();
    }

    //make setup method

    public void MakeExplosion()
    {
        StartCoroutine(ExplosionProcess());
    }
    //check

    public IEnumerator ExplosionProcess()
    {
        GameManager.Instance.ActiveGameLoop(false);
        SetupCollidersForBallsExplosion(true);
        //-------------------------------
        DiactiveCollidersForBallsFlight();

        yield return new WaitForSeconds(0.25f);
        //-------------------------------
        float massInc = 0.5f;
        foreach (Rigidbody2D rb in _balls)
        {
            rb.mass -= massInc;
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
            rb.mass += massInc;
        }
        _balls.Clear();
        //------------------------------- 
        ActiveCollidersForBallsFlight();
        //-------------------------------
        SetupCollidersForBallsExplosion(false);
        //--------------------------------
        yield return new WaitForSeconds(5f);
        Debug.Log("yield return new WaitForSeconds(5f);");
        GameManager.Instance.ActiveGameLoop(true);
    }

    private void DiactiveCollidersForBallsFlight()
    {
        if (_playerCollider == null)
        {
            _playerCollider = GameManager.Instance.PlayerController.gameObject.GetComponent<Collider2D>();
        }

        _goThoughColliders = new List<Collider2D>();
        _goThoughColliders.Add(_playerCollider);
        _goThoughColliders.AddRange(_pocketColliders);

        foreach (Collider2D col in _goThoughColliders)
        {
            //col.isTrigger = true;
            col.enabled = false;
        }
    }

    private void ActiveCollidersForBallsFlight()
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

    private void SetupCollidersForBallsExplosion(bool value)
    {
        _col.enabled = value;
        _returnInGameCollider.enabled = value;
        _groundCollider.enabled = !value;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ball")
        {
            Rigidbody2D ballRb2D = other.GetComponent<Rigidbody2D>();
            _balls.Add(ballRb2D);

            Debug.Log("Count trig balls: " + _balls.Count);
        }
    } 
}
