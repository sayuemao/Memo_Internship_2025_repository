using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController Instance { get; private set; }

    public int playerMaxHealth = 3;
    public int playerCurrentHealth;

    public float invincibleLength;
    private float invincibleCounter;
    
    private BoxCollider2D boxCollider2d;
    private CapsuleCollider2D capsuleCollider2d;
    private SpriteRenderer SR;
    

    public float startFlickerTime;  // 开始闪烁的时间点，是剩余的时间
    public float flickerInterval = 0.5f; // 初始闪烁间隔时间
    private float currentFlickerTime;

    private float nextFlickerTime; // 下次闪烁的时间点

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        boxCollider2d = GetComponent<BoxCollider2D>();
        capsuleCollider2d = GetComponent<CapsuleCollider2D>();
        SR = GetComponent<SpriteRenderer>();
        playerCurrentHealth = playerMaxHealth;
        invincibleCounter = -1f;
        startFlickerTime = invincibleLength;
        currentFlickerTime = 0;
        nextFlickerTime = flickerInterval + Time.time;
    }

    
    void Update()
    {
        if (playerCurrentHealth > 0)
        {
            invincibleCounter = Mathf.Max(-100f, invincibleCounter -= Time.deltaTime);
            if (invincibleCounter > 0)//无敌中
            {
                capsuleCollider2d.enabled = false;
                //闪烁效果
                currentFlickerTime += Time.deltaTime;
                float remainingFlickerTime = startFlickerTime - currentFlickerTime;
                flickerInterval = 0.05f + (remainingFlickerTime / startFlickerTime) * 0.4f;
                if (Time.time >= nextFlickerTime)
                {
                    SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, (SR.color.a == 0.9f) ? 0.75f : 0.9f); // 切换透明度实现闪烁
                    nextFlickerTime = Time.time + flickerInterval;
                }
            }
            else
            {
                capsuleCollider2d.enabled = true;
                SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, 1f);
                currentFlickerTime = 0f;
            }
        }
        else
        {
            SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, 0.75f);
        }
    }

    public void TakeDamage(int damage)
    {
        if (invincibleCounter <= 0)
        {
            invincibleCounter = invincibleLength;

            playerCurrentHealth -= damage;

            boxCollider2d.enabled = false;

            GameManager.Instance.UpdateHealthDisplay(playerCurrentHealth);

            PlayerController.Instance.KnockBack();

            if (playerCurrentHealth <= 0)
            {
                PlayerDie();
            }
        }
    }

    public void PlayerDie()
    {
        PlayerController.Instance.stopInput = true;
        boxCollider2d.enabled = false;
        capsuleCollider2d.enabled = false;
        this.GetComponent<WrapObject>().enabled = false;
        Destroy(this.gameObject, 5f);

        GameManager.Instance.PlayerDie();
    }
}
