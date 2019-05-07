using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBonus : MonoBehaviour
{
    public GameObject pocket;
    public float explosionPower;
    public float timeFly = 2f;

    private Collider2D _playerCollider;
    private Collider2D[] _pocketColliders;

    private List<Collider2D> _goThoughColliders;

    private void Start()
    {
        _pocketColliders = pocket.GetComponents<Collider2D>();
    }

    //make setup method

    public void MakeExplosion(List<Rigidbody2D> balls)
    {
        StartCoroutine(ExplosionProcess(balls));
    }

    public IEnumerator ExplosionProcess(List<Rigidbody2D> balls)
    {
        //-------------------------------
        DiactiveColliders();

        //-------------------------------
        float massOffset = 0.5f;
        foreach (Rigidbody2D rb in balls)
        {
            rb.mass -= massOffset;
        }

        float powerOffset = 1.5f;
        foreach (Rigidbody2D rb in balls)
        {
            float force = Random.Range(explosionPower - powerOffset, explosionPower + powerOffset);
            if (rb != null)
                rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(timeFly);

        foreach (Rigidbody2D rb in balls)
        {
            rb.mass += massOffset;
        }
        balls.Clear();
        //-------------------------------

        ActiveColliders();
    }

    private void DiactiveColliders()
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

    private void ActiveColliders()
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
}
