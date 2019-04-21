using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallMaterial
{
    None, Bouncing
}

public enum BallLayer
{
    Solid, Flying
}

public class Ball : MonoBehaviour
{
    bool _inGame = true;
    bool _inCorridor = false;
     
    Collider2D _collider2D;
    Rigidbody2D _rb2D;
    PhysicsMaterial2D _bouncingMat;

    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        _bouncingMat = _collider2D.sharedMaterial;

        SetCollidermaterial(BallMaterial.None);
        SetRigidbodyType(RigidbodyType2D.Dynamic);
    }
     
    private void OutOfGame()
    {
        _inGame = false;
    }

    private void SetBallScale()
    {

    }

    private void SetBallColor()
    {

    }

    private void SetCollidermaterial(BallMaterial ballMaterial)
    {
        switch (ballMaterial)
        {
            case BallMaterial.None:
                _collider2D.sharedMaterial = null;
                break;
            case BallMaterial.Bouncing:
                _collider2D.sharedMaterial = _bouncingMat;
                break;
            default:
                break;
        }
    }

    private void SetRigidbodyType(RigidbodyType2D value)
    {
        _rb2D.bodyType = value;
    }

    private void SetLayer(BallLayer ballLayer)
    {
        switch (ballLayer)
        {
            case BallLayer.Solid:
                this.gameObject.layer = LayerMask.NameToLayer("SolidBall");
                break;
            case BallLayer.Flying:
                this.gameObject.layer = LayerMask.NameToLayer("FlyingBall");
                break;
            default:
                break;
        }
    }
      
    public bool InGame
    {
        get { return _inGame; }
    }

    public void Fly(Quaternion rotation, float impulseForce,
                    BallMaterial ballMaterial, BallLayer ballLayer)  
    {
        //if (_rb2D.bodyType == RigidbodyType2D.Static)
        //{ 
        //}

        this.gameObject.transform.rotation = rotation; 

        _rb2D.AddForce(this.transform.up * impulseForce, ForceMode2D.Force);

        SetRigidbodyType(RigidbodyType2D.Dynamic);
        SetCollidermaterial(ballMaterial);
        SetLayer(ballLayer);
    } 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Corridor_DOWN")
        { 
            SetRigidbodyType(RigidbodyType2D.Static);

            BallsManager.Instance.GoCorridorDown(this);
        }
        else if (other.tag == "Corridor_UP")
        {
            SetRigidbodyType(RigidbodyType2D.Static);

            BallsManager.Instance.GoCorridorUp(this);
        }
    }
}
