using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBound : MonoBehaviour
{
    [SerializeField] 
    private BoxCollider2D boundsCollider;

    public Rect WorldRect
    {
        get
        {
            if (boundsCollider == null) return new Rect();
            Bounds b = boundsCollider.bounds;
            return new Rect((Vector2)b.min, (Vector2)b.size);
        }
    }

    private void Reset()
    {
        boundsCollider = GetComponent<BoxCollider2D>();
        if (boundsCollider != null) boundsCollider.isTrigger = true; // 仅用于定义区域
    }
}
