using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private GameObject platform;

    public GameObject detectPoint;
    public LayerMask detectLayer;

    public float stayTime = 7.0f;
    private float staycounter ;

    public float startFlickerTime = 3.5f;  // 开始闪烁的时间点，是剩余的时间
    public float flickerInterval = 0.5f; // 初始闪烁间隔时间
    private float currentFlickerTime;

    private float nextFlickerTime; // 下次闪烁的时间点

    public float deadTime = 15.0f;
    private float deadcounter ;

    private bool shouldBePlatform;

    private Rigidbody2D rb;
    private SpriteRenderer SR;
    private BoxCollider2D boxCollider2d;
 

    private float rayExtra = 0.05f;

    public int arrowDamage = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        SR.enabled = true;
        boxCollider2d = GetComponent<BoxCollider2D>();
        platform = transform.GetChild(0).gameObject;
        platform.SetActive(false);

        staycounter = -1f;
        deadcounter = deadTime;
        currentFlickerTime = 0;
        nextFlickerTime = flickerInterval+Time.time;
    }

    private void FixedUpdate()
    {
        Vector2 destination;
        if (transform.localScale.x > 0)//向左
        {
            destination = Vector2.left;
        }
        else
        {
            destination = Vector2.right;
        }

        float castDist = rb.velocity.magnitude * Time.fixedDeltaTime + rayExtra;

        // 如果速度很小也给一点最小检测距离
        if (castDist < rayExtra) castDist = rayExtra;

        RaycastHit2D hit = Physics2D.Raycast(detectPoint.transform.position, destination, castDist, detectLayer);

        if (hit.collider != null)
        {
            shouldBePlatform = true;
        }

        if (shouldBePlatform)
        {
            if (!platform.activeSelf)
            {
                platform.SetActive(true);
                staycounter = stayTime;
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;

                SR.sortingLayerName = "Attack";
                SR.sortingOrder = 1;
                
                boxCollider2d.enabled = false;

                platform.layer = LayerMask.NameToLayer("ArrowPlatform");

                AudioManager.Instance.PlaySFX(9);
            }
        }
    }
    void Update()
    {
        if (staycounter > 0)
        {
            staycounter -= Time.deltaTime;
            if (staycounter <= 0)
            {
                DestroyArrow();
            }
        }
        
        if(deadcounter>0)
        {
            deadcounter -= Time.deltaTime;
            if (deadcounter <= 0)
            {
                DestroyArrow();
            }
        }

        /*当剩余时间小于等于设定的开始闪烁时间时，开始计算闪烁间隔,闪烁间隔会随着剩余时间减少而缩短，公式为0.1f + (remainingTime / startFlickerTime) * 0.4f，确保即使在最后时刻也有基本的闪烁间隔
        */
        if ((shouldBePlatform && staycounter < startFlickerTime)||(deadcounter<startFlickerTime))
        {
            currentFlickerTime += Time.deltaTime;
            float remainingFlickerTime = startFlickerTime - currentFlickerTime;
            flickerInterval = 0.1f + (remainingFlickerTime / startFlickerTime) * 0.4f;
            if (Time.time >= nextFlickerTime)
            {
                SR.color = new Color(SR.color.r,SR.color.g,SR.color.b,(SR.color.a==1f)?0.5f:1f); // 切换透明度实现闪烁
                nextFlickerTime = Time.time + flickerInterval;
            }
        }
        
    }

    private void DestroyArrow()
    {
        Destroy(gameObject);
    }



}
