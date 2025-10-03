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
    
    private bool shouldBePlatform;

    void Start()
    {
        platform = transform.GetChild(0).gameObject;
        platform.SetActive(false);
        staycounter = -1f;
    }

    
    void Update()
    {
        shouldBePlatform = Physics2D.OverlapCircle(detectPoint.transform.position, 0.2f, detectLayer);  
        if(shouldBePlatform)
        {
            if(!platform.activeSelf) platform.SetActive(true);
            staycounter = stayTime;
        }
        if (staycounter > 0)
        {
            staycounter -= Time.deltaTime;
            if (staycounter <= 0)
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
