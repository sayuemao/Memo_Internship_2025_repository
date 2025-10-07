using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public int enemyMaxHealth = 1;
    public int enemyCurrentHealth;

    public int enemyDamage = 1;

    private Animator anim;
    private CapsuleCollider2D capsuleCollider;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public float knockBackForceHorizontal, knockBackForceVertical;
    private float dir = -1f;
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        enemyCurrentHealth = enemyMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerHealthController.Instance.TakeDamage(enemyDamage);

        }
        else if(other.transform.parent!=null && other.transform.parent.gameObject.GetComponent<Arrow>()!=null && !other.transform.parent.GetChild(0).gameObject.activeSelf)
        {
            enemyCurrentHealth -= other.transform.parent.gameObject.GetComponent<Arrow>().arrowDamage;
            dir = Mathf.Sign(other.transform.parent.GetComponent<Rigidbody2D>().velocity.x);
            Destroy(other.transform.parent.gameObject);
            if(enemyCurrentHealth<=0)
            {
                this.GetComponent<IDropItem>()?.DropItem(transform);
                GameManager.Instance.AddScore(this.GetComponent<IDropItem>().ScoreValue);
                DieStep();
            }
            AudioManager.Instance.PlaySFX(5);
        }
    }

    private void DieStep()
    {
        if (!spriteRenderer.flipX)
        {
            rb.velocity = new Vector2(knockBackForceHorizontal*dir, knockBackForceVertical);
        }
        else
        {
            rb.velocity = new Vector2(-knockBackForceHorizontal*dir, knockBackForceVertical);
        }

        anim.SetBool("isDead", true);
        capsuleCollider.enabled = false;
        //rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 2.5f;
        this.GetComponent<WrapObject>().enabled = false;

        //if(this.GetComponent<Enemy>()) this.GetComponent<Enemy>().enabled = false;
        //else if(this.GetComponent<Enemy2>()) this.GetComponent<Enemy2>().enabled = false;
        //else if(this.GetComponent<Enemy3>()) this.GetComponent<Enemy3>().enabled = false;

        if(this.GetComponent<IEnemyController>() is Behaviour b) b.enabled = false;
    }

    public void DieFinal()
    {
        GameManager.Instance.enemycount--;
        Destroy(gameObject);
    }
}
