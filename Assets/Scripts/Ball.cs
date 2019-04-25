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
    [SerializeField] private PhysicsMaterial2D bouncingMat;
    [SerializeField] private PhysicsMaterial2D solidMat;

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
        ActiveTrail(false); 
    } 

    public void UpdateColor(Color color)
    {
        Color = color;

        Points = IsBig ? Points + 2 : Points + 1;
    }

    public void UpdateScaleSize(Vector3 newScale, float tailWidth)
    {
        if (IsBig)
        {
            return;
        }
        ScaleSize = newScale;
        TailWidth = tailWidth;

        IsBig = true;

        Points = Points + 2;
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
            ActiveTrail(false);

            InGame = false;
        }
    }

    public Color Color
    {
        get { return _sprite.color; }
        set
        {
            _sprite.color = value;
            _trail.startColor = value;
        }
    } 

    public Vector3 ScaleSize
    {
        get { return transform.localScale; }
        set
        {
            transform.localScale = value;
        }
    }
    public float TailWidth
    {
        get { return _trail.startWidth; }
        set
        {
            _trail.startWidth = value;
            _trail.endWidth = 0;
        }
    }
     
    public bool InGame { get; set; } = true;
    public int Points { get; set; } = 1;
    public bool IsBig { get; set; } = false; 
}
