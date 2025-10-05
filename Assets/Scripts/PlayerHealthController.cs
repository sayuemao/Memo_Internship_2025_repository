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
    }

    
    void Update()
    {
        invincibleCounter = Mathf.Max(-100f, invincibleCounter -= Time.deltaTime);
        if (invincibleCounter > 0)//ÎÞµÐÖÐ
        {
            capsuleCollider2d.enabled = false;
            if (!isadd)
            {
                boxCollider2d.excludeLayers += whatIsEnemy;
                isadd = true;
            }
            SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, 0.75f);

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
        }
    }

    public void TakeDamage(int damage)
    {
        if (invincibleCounter <= 0)
        {
            invincibleCounter = invincibleLength;

            playerCurrentHealth -= damage;

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
