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
    public float[] tailWidth;

    public bool InGame { get; set; } = true;
    public int Points = 1;

    private Collider2D _collider2D;
    private Rigidbody2D _rb2D;
    private SpriteRenderer _sprite;
    private TrailRenderer _trail;
     
    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _trail = GetComponent<TrailRenderer>();

        SetColliderMaterial(BallMaterial.Standard);
        SetRigidbodyType(RigidbodyType2D.Dynamic);

        SetupRandomScale();
        SetupRandomColor();
        ActiveTrail(false);
    } 

    private void SetupRandomColor()
    {
        int index = Random.Range(0, startColors.Length); 
        _sprite.color = startColors[index]; 
    }

    private void SetupRandomScale()
    {
        int index = Random.Range(0, scaleSizes.Length);
        transform.localScale = scaleSizes[index];

        _trail.startWidth = tailWidth[index];
        _trail.endWidth = 0; 
    }

    private void ActiveTrail(bool value)
    {
        _trail.enabled = value;
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
       
    public void Fly(Quaternion rotation, float impulseForce,
                    BallMaterial ballMaterial, BallLayer ballLayer, bool trail = true)  
    {
        if (_rb2D.bodyType == RigidbodyType2D.Static)
        { 
            SetRigidbodyType(RigidbodyType2D.Dynamic); 
        }
        SetColliderMaterial(ballMaterial);
        SetLayer(ballLayer);
        ActiveTrail(trail);

        this.gameObject.transform.rotation = rotation; 

        _rb2D.AddForce(this.transform.up * impulseForce, ForceMode2D.Impulse);
    } 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Corridor_DOWN")
        { 
            SetRigidbodyType(RigidbodyType2D.Static);
            ActiveTrail(false);

            BallsManager.Instance.GoCorridorDown(this);
        }
        else if (other.tag == "Corridor_UP")
        {
            SetRigidbodyType(RigidbodyType2D.Static);
            ActiveTrail(false); 

            BallsManager.Instance.GoCorridorUp(this);
        }
        else if (other.tag == "BallCatcher")
        {
            SetColliderMaterial(BallMaterial.Standard); 
        }
        else if (other.tag == "Ground")
        {
            SetColliderMaterial(BallMaterial.Standard);
            SetLayer(BallLayer.Solid);

            InGame = false;
        }
    }

    public Color Color
    {
        get
        {
            return _sprite.color;
        }
    } 
}
