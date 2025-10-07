using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieWithTime : MonoBehaviour
{
    public float startFlickerTime = 3.5f;  // ��ʼ��˸��ʱ��㣬��ʣ���ʱ��
    public float flickerInterval = 0.2f; // ��ʼ��˸���ʱ��
    private float currentFlickerTime;

    private float nextFlickerTime; // �´���˸��ʱ���

    public float deadTime = 11.0f;
    private float deadcounter;

    private SpriteRenderer SR;

    // Start is called before the first frame update
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        deadcounter = deadTime;
        currentFlickerTime = 0;
        nextFlickerTime = flickerInterval + Time.time;
    }

    // Update is called once per frame
    void Update()
    {

        if (deadcounter > 0)
        {
            deadcounter -= Time.deltaTime;
            if (deadcounter <= 0)
            {
                Destroy(gameObject);
            }
        }

        if(deadcounter < startFlickerTime)   
        {
            currentFlickerTime += Time.deltaTime;
            float remainingFlickerTime = startFlickerTime - currentFlickerTime;
            flickerInterval = 0.1f + (remainingFlickerTime / startFlickerTime) * 0.4f;
            if (Time.time >= nextFlickerTime)
            {
                SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, (SR.color.a == 1f) ? 0.1f : 1f); // �л�͸����ʵ����˸
                nextFlickerTime = Time.time + flickerInterval;
            }
        }
    }


}
