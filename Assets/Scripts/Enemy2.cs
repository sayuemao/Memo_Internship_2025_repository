using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public float moveSpeed;
    public bool isMovingRight ;

    //private GameObject leftGroundCheckPoint;
    //private GameObject rightGroundCheckPoint;

    private GameObject leftWallCheckPoint;
    private GameObject rightWallCheckPoint;

    private Rigidbody2D RB;
    private SpriteRenderer SR;

    public LayerMask groundLayer;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();

        //leftGroundCheckPoint = transform.GetChild(0).gameObject;
        //rightGroundCheckPoint = transform.GetChild(1).gameObject;

        leftWallCheckPoint = transform.GetChild(2).gameObject;
        rightWallCheckPoint = transform.GetChild(3).gameObject;
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
        //Collider2D leftGroundCollider = Physics2D.OverlapCircle(leftGroundCheckPoint.transform.position, 0.2f, groundLayer);
        //Collider2D rightGroundCollider = Physics2D.OverlapCircle(rightGroundCheckPoint.transform.position, 0.2f, groundLayer);
        Collider2D leftWallCollider = Physics2D.OverlapCircle(leftWallCheckPoint.transform.position, 0.2f, groundLayer);
        Collider2D rightWallCollider = Physics2D.OverlapCircle(rightWallCheckPoint.transform.position, 0.2f, groundLayer);

        // 两侧都无地（腾空/掉落中）时不翻转，避免抖动
        //if (!leftGroundCollider && !rightGroundCollider) return;

        if (isMovingRight)
        {
            // 右侧遇墙或前方悬空 -> 向左
            if (rightWallCollider /*|| !rightGroundCollider*/)
            {
                isMovingRight = false;
            }
        }
        else
        {
            // 左侧遇墙或前方悬空 -> 向右
            if (leftWallCollider /*|| !leftGroundCollider*/)
            {
                isMovingRight = true;
            }
        }

    }
}
