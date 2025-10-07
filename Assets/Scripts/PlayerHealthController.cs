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
    
    public CapsuleCollider2D  capsuleCollider2d;
    public CapsuleCollider2D capsuleCollider2dTrigger;
    public BoxCollider2D BoxCollider2D;
    private SpriteRenderer SR;

    public LayerMask groundlayer;

    public float startFlickerTime;  // ��ʼ��˸��ʱ��㣬��ʣ���ʱ��
    public float flickerInterval = 0.5f; // ��ʼ��˸���ʱ��
    private float currentFlickerTime;

    private float nextFlickerTime; // �´���˸��ʱ���

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
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
            if (invincibleCounter > 0)//�޵���
            {
                capsuleCollider2dTrigger.enabled = false;
                //��˸Ч��
                currentFlickerTime += Time.deltaTime;
                float remainingFlickerTime = startFlickerTime - currentFlickerTime;
                flickerInterval = 0.05f + (remainingFlickerTime / startFlickerTime) * 0.4f;
                if (Time.time >= nextFlickerTime)
                {
                    SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, (SR.color.a == 0.9f) ? 0.75f : 0.9f); // �л�͸����ʵ����˸
                    nextFlickerTime = Time.time + flickerInterval;
                }
            }
            else
            {
                capsuleCollider2dTrigger.enabled = true;
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

            capsuleCollider2d.enabled = false;
            //BoxCollider2D.excludeLayers |= groundlayer.value; // ʹ��ҵ�BoxCollider2D���Ե����

            GameManager.Instance.UpdateHealthDisplay(playerCurrentHealth);

            PlayerController.Instance.KnockBack();

            AudioManager.Instance.PlaySFX(8);

            if (playerCurrentHealth <= 0)
            {
                PlayerDie();
            }
        }
    }

    public void PlayerDie()
    {
        PlayerController.Instance.stopInput = true;
        capsuleCollider2d.enabled = false;
        //BoxCollider2D.excludeLayers |= groundlayer.value; // ʹ��ҵ�BoxCollider2D���Ե����
        capsuleCollider2dTrigger.enabled = false;
        this.GetComponent<WrapObject>().enabled = false;
        Destroy(this.gameObject, 5f);

        AudioManager.Instance.PlaySFX(7);

        GameManager.Instance.PlayerDie();
    }
}
