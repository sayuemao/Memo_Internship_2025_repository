using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRock : MonoBehaviour
{
    public float fallTime = 3f;
    private float fallTimer;

    private SpriteRenderer spriteRenderer;

    public float resetTime = 5f;
    private float resetTimer;

    public bool isFalling;

    public GameObject rock;

    private Animator anim;
    private void Start()
    {
        rock = transform.GetChild(0).gameObject; // ��ȡ��һ����������Ϊ��ʯ
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isFalling)
        {
            // ����ʱ���ﵽʱ�����������ʧ��
            fallTimer -= Time.deltaTime;
            if (fallTimer <= 0f)
            {
                rock.SetActive(false);
                isFalling = false;
                resetTimer = resetTime; // ��ʼ����ָ���ʱ
                anim.SetTrigger("Break");
            }
        }
        // rock ���ڡ�����/ʧ��У���ʼ�ָ���ʱ
        else if (!rock.activeSelf)
        {
            resetTimer -= Time.deltaTime;
            if (resetTimer <= 0f)
            {
                rock.SetActive(true); // �ָ���ʼ״̬�����¿ɲȣ�
                spriteRenderer.enabled = true; // ȷ����ʯ�ɼ�
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ����δ�ڵ���ʱ����ʯ��ǰ�Ǽ��������´���
        if (collision.CompareTag("Player") && !isFalling && rock.activeSelf)
        {
            isFalling = true;
            fallTimer = fallTime;
        }
    }

    public void HideRock()
    {
        spriteRenderer.enabled = false;
    }
}
