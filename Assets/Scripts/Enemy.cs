using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    private bool isMovingRight = true;

    private GameObject leftPoint;
    private GameObject rightPoint;

    private Rigidbody2D RB;
    private SpriteRenderer SR;

    public LayerMask groundLayer;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();

        leftPoint = transform.GetChild(0).gameObject;
        rightPoint = transform.GetChild(1).gameObject;
    }

    
    void Update()
    {
        CheckEdge();
        RB.velocity = new Vector2(isMovingRight ? moveSpeed : -moveSpeed, RB.velocity.y);
        if (isMovingRight)
        {
            SR.flipX = true;
        }
        else
        {
            SR.flipX = false;
        }
       
    }

    private void CheckEdge()
    {
        if(Physics2D.OverlapCircle(leftPoint.transform.position, 0.2f, groundLayer) == null)
        {
            isMovingRight = true;
        }
        else if(Physics2D.OverlapCircle(rightPoint.transform.position, 0.2f, groundLayer) == null)
        {
            isMovingRight = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == groundLayer)
        {
            isMovingRight = !isMovingRight;
        }
    }

}
