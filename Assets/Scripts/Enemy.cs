using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour,IDropItem,IEnemyController
{
    public float moveSpeed;
    public bool isMovingRight = false;

    private GameObject leftGroundCheckPoint;
    private GameObject rightGroundCheckPoint;

    private GameObject leftWallCheckPoint;
    private GameObject rightWallCheckPoint;

    private Rigidbody2D RB;
    private SpriteRenderer SR;

    public LayerMask groundLayer;

    public GameObject[] dropItemOnDestroy;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();

        leftGroundCheckPoint = transform.GetChild(0).gameObject;
        rightGroundCheckPoint = transform.GetChild(1).gameObject;

        leftWallCheckPoint = transform.GetChild(2).gameObject;
        rightWallCheckPoint = transform.GetChild(3).gameObject;
    }

    
    public virtual void Update()
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
        Collider2D leftGroundCollider = Physics2D.OverlapCircle(leftGroundCheckPoint.transform.position, 0.2f, groundLayer);
        Collider2D rightGroundCollider = Physics2D.OverlapCircle(rightGroundCheckPoint.transform.position, 0.2f, groundLayer);
        Collider2D leftWallCollider = Physics2D.OverlapCircle(leftWallCheckPoint.transform.position, 0.2f, groundLayer);
        Collider2D rightWallCollider = Physics2D.OverlapCircle(rightWallCheckPoint.transform.position, 0.2f, groundLayer);

        // ���඼�޵أ��ڿ�/�����У�ʱ����ת�����ⶶ��
        if (!leftGroundCollider && !rightGroundCollider) return;

        if (isMovingRight)
        {
            // �Ҳ���ǽ��ǰ������ -> ����
            if (rightWallCollider || !rightGroundCollider)
            {
                isMovingRight = false;
            }
        }
        else
        {
            // �����ǽ��ǰ������ -> ����
            if (leftWallCollider || !leftGroundCollider)
            {
                isMovingRight = true;
            }
        }

    }

   
    public void DropItem(Transform dropPosition)//Ȩ�ص���
    {
        if (dropItemOnDestroy == null || dropItemOnDestroy.Length == 0) return;

        int n = dropItemOnDestroy.Length;

        int totalWeight = 0;
        for (int i = 0; i < n; i++)
        {
            totalWeight += (n - i);
        }

        int roll = Random.Range(0, totalWeight); // ���޲���
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
