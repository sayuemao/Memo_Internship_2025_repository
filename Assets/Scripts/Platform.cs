using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Platform : MonoBehaviour
{
    private GameObject left, right;
    public LayerMask otherPlatform;
    public float detectDistance = 0.2f;


    void Start()
    {
        left = transform.GetChild(0).gameObject;
        right = transform.GetChild(1).gameObject;
    }

    
    void Update()
    {
        DetectDirection(Vector2.left, left);
        DetectDirection(Vector2.right, right);
    }

    void DetectDirection(Vector2 destination,GameObject originObject)
    {
        RaycastHit2D hit = Physics2D.Raycast(originObject.transform.position, destination, detectDistance, otherPlatform);
        
        if (hit.collider != null)
        {
            originObject.SetActive(false);
        }
    }
}
