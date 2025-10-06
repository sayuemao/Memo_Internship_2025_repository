using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapObject : MonoBehaviour
{
    public WorldBound bounds;         // 拖入上面的边界物体
    public float margin = 0f;           // 可给一点边缘裕量，避免抖动

    private Rigidbody2D rb;
    private bool hasRB;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hasRB = rb != null;
    }

    private void Start()
    {
        bounds = FindAnyObjectByType<WorldBound>();
    }
    private void FixedUpdate()
    {
        if (bounds == null) return;
        Rect rect = bounds.WorldRect;
        if (rect.width <= 0f || rect.height <= 0f) return;

        float minX = rect.xMin - margin;
        float maxX = rect.xMax + margin;
        float minY = rect.yMin - margin;
        float maxY = rect.yMax + margin;

        Vector2 pos = hasRB ? rb.position : (Vector2)transform.position;
        bool wrapped = false;

        float width = rect.width;
        float height = rect.height;

        if (pos.x < minX) { pos.x += width; wrapped = true; }
        else if (pos.x > maxX) { pos.x -= width; wrapped = true; }

        if (pos.y < minY) { pos.y += height; wrapped = true; }
        else if (pos.y > maxY) { pos.y -= height; wrapped = true; }

        if (wrapped)
        {
             transform.position = pos;
        }
    }
}
