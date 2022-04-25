using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportationMove : MonoBehaviour
{
    // 前進速度
    public float forwardSpeed = 0.3f;
    // 後退速度
    public float backwardSpeed = 0.1f;
    // 旋回速度
    public float rotateSpeed = 2.0f;
    private Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //前进
        if (Input.GetKey(KeyCode.W))
        {
            float translation = forwardSpeed;
            transform.Translate(0, 0, translation);
        }
        //后退
        if (Input.GetKey(KeyCode.S))
        {
            float translation = -backwardSpeed;
            transform.Translate(0, 0, translation);
        }
        //左转
        if (Input.GetKey(KeyCode.A))
        {
            float rotation = -rotateSpeed;
            transform.Rotate(0, 0, rotation);        
        }
        //右转
        if (Input.GetKey(KeyCode.D))
        {
            float rotation = rotateSpeed;
            transform.Rotate(0, 0, rotation);
        }
    }
}
