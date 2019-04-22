using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallMaterial
{
    Standard, Bouncing
}

public enum BallLayer
{
    Solid, Flying
}

public class Ball : MonoBehaviour
{
    public PhysicsMaterial2D bouncingMat;
    public PhysicsMaterial2D solidMat;

    public Color[] startColors;
    public Vector3[] scaleSizes;

    bool _inGame = true;
    bool _inCorridor = false;
     
    Collider2D _collider2D;
    Rigidbody2D _rb2D;
    SpriteRenderer _sprite;
     
    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        _sprite = GetComponentInChildren<SpriteRenderer>();

        SetColliderMaterial(BallMaterial.Standard);
        SetRigidbodyType(RigidbodyType2D.Dynamic);

        //SetupRandomScale();
        SetupRandomColor();
    }
     
    private void OutOfGame()
    {
        _inGame = false;
    }

    private void SetupRandomScale()
    {
        int index = Random.Range(0, startColors.Length - 1);
        _sprite.color = startColors[index];
    }

    private void SetupRandomColor()
    {
        int index = Random.Range(0, scaleSizes.Length - 1);
        transform.localScale = scaleSizes[index];
    }

    private void SetColliderMaterial(BallMaterial ballMaterial)
    {
        switch (ballMaterial)
        {
            case BallMaterial.Standard:
                _collider2D.sharedMaterial = solidMat;
                break;
            case BallMaterial.Bouncing:
                _collider2D.sharedMaterial = bouncingMat;
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
        if (_rb2D.bodyType == RigidbodyType2D.Static)
        {
            SetRigidbodyType(RigidbodyType2D.Dynamic);
        }
        SetColliderMaterial(ballMaterial);
        SetLayer(ballLayer);

        this.gameObject.transform.rotation = rotation; 

        _rb2D.AddForce(this.transform.up * impulseForce, ForceMode2D.Impulse);

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
        else if (other.tag == "BallCatcher")
        {
            SetColliderMaterial(BallMaterial.Standard);
        }
        else if (other.tag == "Ground")
        {
            SetColliderMaterial(BallMaterial.Standard);

            OutOfGame();
        }
    }
}
