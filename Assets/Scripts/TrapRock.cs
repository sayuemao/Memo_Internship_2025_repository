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
        rock = transform.GetChild(0).gameObject; // 获取第一个子物体作为岩石
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isFalling)
        {
            // 倒计时，达到时间后让子物体失活
            fallTimer -= Time.deltaTime;
            if (fallTimer <= 0f)
            {
                rock.SetActive(false);
                isFalling = false;
                resetTimer = resetTime; // 开始进入恢复计时
                anim.SetTrigger("Break");
            }
        }
        // rock 处于“碎裂/失活”中，开始恢复计时
        else if (!rock.activeSelf)
        {
            resetTimer -= Time.deltaTime;
            if (resetTimer <= 0f)
            {
                rock.SetActive(true); // 恢复初始状态（重新可踩）
                spriteRenderer.enabled = true; // 确保岩石可见
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 仅当未在倒计时且岩石当前是激活的情况下触发
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
