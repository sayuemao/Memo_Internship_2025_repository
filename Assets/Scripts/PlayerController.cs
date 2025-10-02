using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance { get; private set; }


    public Rigidbody2D rb;

    private bool isGrounded;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;


    private Animator anim;
    private SpriteRenderer spriteRenderer;

    public float moveSpeed = 7.5f;
    public float jumpingForce = 10f;

    public float knockBackForce, knockBackLength;
    private float knockBackCounter;

    public float bounceForce;

    public bool stopInput;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, 0.2f, whatIsGround);
        if (/*!PauseMenu.Instance.isPaused &&*/ !stopInput)
        {
            if (knockBackCounter <= 0)
            {
                rb.velocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), rb.velocity.y);

                if (Input.GetButtonDown("Jump")&& isGrounded)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpingForce);
                    //AudioManager.Instance.PlaySFX(10);                
                }

                if (rb.velocity.x < 0)
                {
                    spriteRenderer.flipX = true;
                }
                else if (rb.velocity.x > 0)
                {
                    spriteRenderer.flipX = false;
                }
            }
            else
            {
                knockBackCounter -= Time.deltaTime;
                /*if (!spriteRenderer.flipX)
                {
                    rb.velocity = new Vector2(-knockBackForce, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(knockBackForce, rb.velocity.y);
                }个人感觉在KnockBack函数中修改更直观。*/
            }
        }
        AnimUpdate();
    }

    private void AnimUpdate()
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetFloat("velocityXabs", Mathf.Abs(rb.velocity.x));
    }

    public void KnockBack()
    {
        knockBackCounter = knockBackLength;
        //rb.velocity = new Vector2(0f,knockBackForce);
        if (!spriteRenderer.flipX)
        {
            rb.velocity = new Vector2(-knockBackForce, knockBackForce);
        }
        else
        {
            rb.velocity = new Vector2(knockBackForce, knockBackForce);
        }

        anim.SetTrigger("Hurt");
    }

    public void Bounce()
    {
        rb.velocity = new Vector2(rb.velocity.x, bounceForce);
        //AudioManager.Instance.PlaySFX(10);
    }
}
