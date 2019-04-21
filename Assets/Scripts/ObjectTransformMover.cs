using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    right, left, up, down
}

public class ObjectTransformMover : MonoBehaviour
{
    public Direction direction;

    Vector2 _moveDirection;

    private void Start()
    {
        InitDirection();
    }

    void FixedUpdate()
    {
        
    }
    
    private void InitDirection()
    {
        switch (direction)
        {
            case Direction.right:
                _moveDirection = Vector2.right;
                break;
            case Direction.left:
                _moveDirection = Vector2.left; 
                break;
            case Direction.up:
                _moveDirection = Vector2.up; 
                break;
            case Direction.down:
                _moveDirection = -Vector2.up; 
                break;
            default:
                _moveDirection = Vector2.zero; 
                break;
        }
    }
}
