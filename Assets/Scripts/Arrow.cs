using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private GameObject platform;

    public GameObject detectPoint;
    public LayerMask detectLayer;

    public float stayTime = 7.0f;
    private float staycounter ;

    public float deadTime = 15.0f;
    private float deadcounter ;

    private bool shouldBePlatform;

    private Rigidbody2D rb;
    private SpriteRenderer SR;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        platform = transform.GetChild(0).gameObject;
        platform.SetActive(false);
        staycounter = -1f;
        deadcounter = deadTime;
    }

    
    void Update()
    {
        Vector2 destination;
        if (transform.localScale.x > 0)//Ïò×ó
        {
            destination = Vector2.left;
        }
        else
        {
            destination = Vector2.right;
        }

        RaycastHit2D hit = Physics2D.Raycast(detectPoint.transform.position, destination, 0.1f, detectLayer);

        if (hit.collider != null)
        {
            shouldBePlatform = true;
        }

        //shouldBePlatform = Physics2D.OverlapCircle(detectPoint.transform.position, 0.2f, detectLayer);  

        if(shouldBePlatform)
        {
            if (!platform.activeSelf)
            {
                platform.SetActive(true);
                staycounter = stayTime;
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;

                SR.sortingLayerName = "Map";
                SR.sortingOrder = -1;

                gameObject.layer = LayerMask.NameToLayer("ArrowPlatform");
            }                    
        }


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

    }

    private void DestroyArrow()
    {
        Destroy(gameObject);
    }
}
