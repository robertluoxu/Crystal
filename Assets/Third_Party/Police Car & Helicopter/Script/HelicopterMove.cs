using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterMove : MonoBehaviour
{
    public float speed = 1.5f;//控制移动速度
    public float rotspeed = 1f; // 旋转速度

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
            float translation = speed;
            transform.Translate(0, 0, translation);
        }
        //后退
        if (Input.GetKey(KeyCode.S))
        {
            float translation = -speed;
            transform.Translate(0, 0, translation);
        }
        //左转
        if (Input.GetKey(KeyCode.A))
        {
            float rotation = -rotspeed;
            transform.Rotate(0, 0, rotation);        
        }
        //右转
        if (Input.GetKey(KeyCode.D))
        {
            float rotation = rotspeed;
            transform.Rotate(0, 0, rotation);
        }
        //抬头
        if (Input.GetKey(KeyCode.I))
        {
            float rotation = rotspeed;
            transform.Rotate(rotation, 0, 0);
        }
        //俯身
        if (Input.GetKey(KeyCode.K))
        {
            float rotation = -rotspeed;
            transform.Rotate(rotation, 0, 0);
        }
        //左侧翻身
        if (Input.GetKey(KeyCode.J))
        {
            float rotation = rotspeed;
            transform.Rotate(0, rotation, 0);
        }//右侧翻身
        if (Input.GetKey(KeyCode.L))
        {
            float rotation = -rotspeed;
            transform.Rotate(0, rotation, 0);
        }
    }
}
