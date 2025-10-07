using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour, IDropItem, IEnemyController
{
    public float jumpForce = 3f;
    public float jumpInterval = 7f;
    private float jumpCooldown = 0f;
    private float jumpTimer = 0f;
    private bool isGrounded = false;

    public bool isJumping = false;

    public float moveSpeed;
    public bool isMovingRight = false;

    private GameObject leftGroundCheckPoint;
    private GameObject rightGroundCheckPoint;

    private GameObject leftWallCheckPoint;
    private GameObject rightWallCheckPoint;

    public GameObject groundCheckPoint;

    private Rigidbody2D RB;
    private SpriteRenderer SR;
    private Animator anim;
    public LayerMask groundLayer;

    public float turnCooldown = 1f;        // 换向冷却，避免抖动
    private float turnCooldownTimer = 0f;

    public GameObject[] dropItemOnDestroy;

    public int ScoreValue { get { return scoreValue; } set { scoreValue = value; } }
    public int scoreValue;
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        leftGroundCheckPoint = transform.GetChild(0).gameObject;
        rightGroundCheckPoint = transform.GetChild(1).gameObject;

        leftWallCheckPoint = transform.GetChild(2).gameObject;
        rightWallCheckPoint = transform.GetChild(3).gameObject;

        jumpCooldown = jumpInterval;
    }


    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.transform.position, 0.2f, groundLayer);
        if (isGrounded && !isJumping)
        {
            if (turnCooldownTimer > 0f)
            {
                turnCooldownTimer -= Time.deltaTime;
            }
            isJumping = false;
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

            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0f)
            {
                RB.velocity = new Vector2(RB.velocity.x, jumpForce);
                isJumping = true;
                jumpInterval = Random.Range(0f, jumpCooldown);
                jumpTimer = jumpInterval;
            }
        }
        else
        {
            if(Mathf.Abs(RB.velocity.y) < 0.1f)
            {
                isJumping = false;
            }
            //if (RB.velocity.x>0)
            //{
            //    SR.flipX = true;
            //}
            //else
            //{
            //    SR.flipX = false;
            //}
        }

        anim.SetBool("isJumping", isJumping);

    }

    private void CheckEdge()
    {
        if(turnCooldownTimer > 0f) return;

        Collider2D leftGroundCollider = Physics2D.OverlapCircle(leftGroundCheckPoint.transform.position, 0.05f, groundLayer);
        Collider2D rightGroundCollider = Physics2D.OverlapCircle(rightGroundCheckPoint.transform.position, 0.05f, groundLayer);
        Collider2D leftWallCollider = Physics2D.OverlapCircle(leftWallCheckPoint.transform.position, 0.05f, groundLayer);
        Collider2D rightWallCollider = Physics2D.OverlapCircle(rightWallCheckPoint.transform.position, 0.05f, groundLayer);

        // 两侧都无地（腾空/掉落中）时不翻转，避免抖动
        if (!leftGroundCollider && !rightGroundCollider) return;
        //if(leftWallCheckPoint && rightWallCheckPoint) return;

        if (isMovingRight)
        {
            // 右侧遇墙或前方悬空 -> 向左
            if (rightWallCollider || !rightGroundCollider)
            {
                isMovingRight = false;
                turnCooldownTimer = turnCooldown;
            }
        }
        else
        {
            // 左侧遇墙或前方悬空 -> 向右
            if (leftWallCollider || !leftGroundCollider)
            {
                isMovingRight = true;
                turnCooldownTimer = turnCooldown;
            }
        }

    }

    public void DropItem(Transform dropPosition)//权重掉落
    {
        if (dropItemOnDestroy == null || dropItemOnDestroy.Length == 0) return;

        int n = dropItemOnDestroy.Length;

        int totalWeight = 0;
        for (int i = 0; i < n; i++)
        {
            totalWeight += (n - i);
        }

        int roll = Random.Range(0, totalWeight); // 上限不含
        int cumulative = 0;
        int chosenIndex = 0;

        for (int i = 0; i < n; i++)
        {
            cumulative += (n - i);
            if (roll <= cumulative)
            {
                chosenIndex = i;
                break;
            }
        }

        var prefab = dropItemOnDestroy[chosenIndex];
        if (prefab != null)
        {
            Instantiate(prefab, dropPosition.position, dropPosition.rotation);
        }
    }
}
