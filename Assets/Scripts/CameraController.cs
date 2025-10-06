using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector2 lastPos;
    
    public Transform followTarget;

    public Transform farBackground;//middleBackground;
    /*
    public float smoothSpeed;
    */
    public float maxHeight;
    public float minHeight;
    public float maxX;
    public float minX;

    public bool stopFollow = false;

    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopFollow)
        {
            //�����������Target�ƶ�
            FollowTarget(followTarget);

            //���Ʊ�����������ƶ�
            Vector2 ammountToMove = new Vector2(transform.position.x - lastPos.x, transform.position.y - lastPos.y);
            farBackground.position += new Vector3(ammountToMove.x, ammountToMove.y, 0f);
            //middleBackground.position += new Vector3(ammountToMove.x, ammountToMove.y, 0f) * 0.5f;
            lastPos = transform.position;
            
        }

    }




    private void FollowTarget(Transform followtarget)
    {
        if (followTarget != null)
        {
            Vector3 position = followTarget.position;
            position.y = Mathf.Clamp(position.y, minHeight, maxHeight);
            position.x = Mathf.Clamp(position.x, minX, maxX);
            position.z = transform.position.z;

            //transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * smoothSpeed);
            //Lerpʵ�ֲ�ֵ���ӳٸ���,��Ҫ����FixedUpdate��

            transform.position = new Vector3(position.x, position.y, transform.position.z);
            //���ӳٸ��棬�ɷ���Update��
        }
    }
}
