using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public Rigidbody2D rb;

    public bool isGrounded;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    private Animator anim;
    private SpriteRenderer spriteRenderer;
    public CapsuleCollider2D capsuleCollider2D;
    public CapsuleCollider2D capsuleCollider2DTrigger;
    public BoxCollider2D boxCollider2D;

    public float moveSpeed = 7.5f;
    private float currentSpeedDes;
    private float currentSpeed;
    public float groundAcceleration = 35f;
    public float airAcceleration = 20f;
    public float jumpingForce = 10f;

    public float groundStopFriction = 15f;  // 速度很小且无输入时快速归零的力度
    public float minFrictionSpeed = 0.1f;   // 触发摩擦的最小速度绝对值

    public float knockBackForceHorizontal,knockBackForceVertical, knockBackLength;
    private float knockBackCounter;


    public bool stopInput;

    public bool isAttack;
    public GameObject shotArrowPosition;
    public GameObject arrowPrefab;
    public float arrowSpeed;

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
        //capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, 0.2f, whatIsGround);
        if (isGrounded)
        {
            knockBackCounter = 0;
            if (!capsuleCollider2D.enabled /*|| !boxCollider2D.enabled*/)
            {
                capsuleCollider2D.enabled = true;
                //boxCollider2D.excludeLayers &= ~ PlayerHealthController.Instance.groundlayer.value;
            }
        }
        if (/*!PauseMenu.Instance.isPaused &&*/ !stopInput)
        {
            HandleMove();

            if (knockBackCounter <= 0)
            {
                HandleJump();
                HandleAttack();
            }
            else
            {
                knockBackCounter -= Time.deltaTime;
            }
        }

        AnimUpdate();

        //capsuleCollider2D.size = capsuleCollider2DTrigger.size;
        //capsuleCollider2D.offset = capsuleCollider2DTrigger.offset;

    }
    private void HandleMove()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        currentSpeedDes = moveSpeed * horizontalInput;
        currentSpeed = rb.velocity.x;
        bool hasInput = Mathf.Abs(horizontalInput) > 0.01f;
        float accel;
        if (isGrounded)
        {
            accel = groundAcceleration;
        }
        else
        {
            accel = airAcceleration;
        }
        if (hasInput && Mathf.Sign(currentSpeedDes) != Mathf.Sign(currentSpeed))
        {
            accel *= 1.2f;
        }
        float newSpeed = Mathf.MoveTowards(currentSpeed, currentSpeedDes, accel * Time.deltaTime);
        if (isGrounded && !hasInput && Mathf.Abs(newSpeed) < minFrictionSpeed)
        {
            newSpeed = Mathf.MoveTowards(newSpeed, 0f, groundStopFriction * Time.deltaTime);
        }
        rb.velocity = new Vector2(newSpeed, rb.velocity.y);

        if (knockBackCounter <= 0)
        {
            if (rb.velocity.x < 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (rb.velocity.x > 0)
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingForce);
            //AudioManager.Instance.PlaySFX(10);                
        }

    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttack)
        {
            isAttack = true;
            anim.SetTrigger("Attack");
        }
    }

    public void ShootArrow()
    {
        Vector3 arrowPos = shotArrowPosition.transform.position;

        GameObject arrow = Instantiate(arrowPrefab, arrowPos, Quaternion.identity);

        if (spriteRenderer.flipX)
        {
            arrow.transform.localScale = new Vector3(-1f, 1f, 1f);
            arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(arrowSpeed, 0f);
        }
        else
        {
            arrow.transform.localScale = new Vector3(1f, 1f, 1f);
            arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(-arrowSpeed, 0f);
        }
        //AudioManager.Instance.PlaySFX(9);
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
            rb.velocity = new Vector2(knockBackForceHorizontal, knockBackForceVertical);
        }
        else
        {
            rb.velocity = new Vector2(-knockBackForceHorizontal, knockBackForceVertical);
        }      

        anim.SetTrigger("Hurt");
    }


}
