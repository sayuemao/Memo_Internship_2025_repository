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
    public LayerMask whatIsEnemy;

    public float startFlickerTime;  // ��ʼ��˸��ʱ��㣬��ʣ���ʱ��
    public float flickerInterval = 0.5f; // ��ʼ��˸���ʱ��
    private float currentFlickerTime;

    private float nextFlickerTime; // �´���˸��ʱ���


    private bool isadd;
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
        invincibleCounter = Mathf.Max(-100f, invincibleCounter -= Time.deltaTime);
        if (invincibleCounter > 0)//�޵���
        {
            capsuleCollider2d.enabled = false;
            if (!isadd)
            {
                boxCollider2d.excludeLayers += whatIsEnemy;
                isadd = true;
            }
            //��˸Ч��
            currentFlickerTime += Time.deltaTime;
            float remainingFlickerTime = startFlickerTime - currentFlickerTime;
            flickerInterval = 0.05f + (remainingFlickerTime / startFlickerTime) * 0.4f;
            if (Time.time >= nextFlickerTime)
            {
                SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, (SR.color.a == 1f) ? 0.75f : 1f); // �л�͸����ʵ����˸
                nextFlickerTime = Time.time + flickerInterval;
            }
        }
        else
        {
            capsuleCollider2d.enabled = true;
            if (isadd)
            {             
                boxCollider2d.excludeLayers -= whatIsEnemy;
                isadd = false;
            }
            SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, 1f);
            currentFlickerTime = 0f;
        }
    }

    public void TakeDamage(int damage)
    {
        if (invincibleCounter <= 0)
        {
            invincibleCounter = invincibleLength;

            playerCurrentHealth -= damage;

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

    }
}
