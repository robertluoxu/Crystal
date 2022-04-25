using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WowCamera : MonoBehaviour
{
    ///镜头的目标-player
    /// 
    private Transform _target;

    [SerializeField]
    private Vector3 focus = Vector3.zero;
    private Vector3 oldPos;
    [SerializeField]
    private GameObject focusObj = null;
    
    ///镜头离目标的距离 
    /// 
    public float Distance = 17.0f;
    
    ///最小镜头距离
    /// 
    public float MinDistance = 3.0f;

    ///最大镜头距离
    /// 
    public float MaxDistance = 100.0f;
    
    //鼠标滚轮拉近拉远速度系数
    public float ScrollFactor = 10.0f;

    ///左右旋转速度可以快一点
    ///
    public float RotateFactorX = 10.0f;

    ///上下旋转速度可以慢一点
    /// 
    public float RotateFactorY = 0.5f;

    //镜头水平环绕角度
    public float HorizontalAngle = 0;
    
    //镜头竖直环绕角度 
    public float VerticalAngle = 60;
    
    public float MaxVerticalAngle = 90;
    
    public float MinVerticalAngle = 6;

    private Transform _cameraTransform;

    private bool _isOpenCamera = true;
    
    void Start()
    {
        _cameraTransform = transform;

        if (GameObject.Find ("CamPos") != null) {
            _target = GameObject.Find ("CamPos").transform;
            if ( _target == null)
            {
                Debug.Log ( "Target Object Not Exist");
            }
        }

        if (this.focusObj == null)
            this.setupFocusObject("CameraFocusObject");

        Transform trans = this.transform;
        transform.parent = this.focusObj.transform;
        trans.LookAt(this.focus);
    }

    public void SetTarget(Transform transform) {
        _target = transform;
    }

    public void OpenCameraDrop(bool result) {
        this._isOpenCamera = result;
    }

    void Update()
    {
        Distance -= Input.GetAxis ("Mouse ScrollWheel") * ScrollFactor;
        Distance = ( Distance < MinDistance ) ? MinDistance : Distance;
        Distance = ( Distance > MaxDistance ) ? MaxDistance : Distance;

        //按住鼠标左右键移动，镜头随之旋转
        var isMouseRightButtonDown = Input.GetMouseButton(1);
        var isMouseLeftButtonDown = Input.GetMouseButton(0);
        if (isMouseRightButtonDown)
        {
            //水平旋转和上下旋转
            Screen.lockCursor = true;
            var axisX = Input.GetAxis( "Mouse X" );
            var axisY = Input.GetAxis( "Mouse Y" );
            HorizontalAngle += axisX * RotateFactorX;
            VerticalAngle += -axisY * RotateFactorY;
            VerticalAngle = ( VerticalAngle < MinVerticalAngle ) ? MinVerticalAngle : VerticalAngle;
            VerticalAngle = ( VerticalAngle > MaxVerticalAngle ) ? MaxVerticalAngle : VerticalAngle;

            if (isMouseRightButtonDown)
            {
                //如果是鼠标右键移动，则旋转人物在水平面上与镜头方向一致
                if (_target != null) _target.rotation = Quaternion.Euler(0, HorizontalAngle, 0);
            }
        }
        else
        {
            Screen.lockCursor = false;
        }

        ///按镜头距离调整位置和方向
        var rotation = Quaternion.Euler(VerticalAngle, HorizontalAngle, 0);
        var offset = rotation * Vector3.back * Distance;
        _cameraTransform.rotation = rotation;
        if (_target != null) { 
            _cameraTransform.position = _target.position + offset; 
        }
        else {
            _cameraTransform.position = this.focus + offset;
        }

        if (this._isOpenCamera) {
            if (Input.GetMouseButtonDown(0))
                this.oldPos = Input.mousePosition;
            this.mouseDragEvent(Input.mousePosition);
        }
    }

    void mouseDragEvent(Vector3 mousePos)
    {
        Vector3 diff = mousePos - oldPos;

        if(Input.GetMouseButton(0))
        {
            if (diff.magnitude > Vector3.kEpsilon)
                this.cameraTranslate(-diff / 100.0f);
        }
        this.oldPos = mousePos;	
        return;
    }


    void cameraTranslate(Vector3 vec)
    {
        Transform focusTrans = this.focusObj.transform;
        vec.x *= -1;
        focusTrans.Translate(Vector3.right * vec.x);
        focusTrans.Translate(Vector3.up * vec.y);

        this.focus = focusTrans.position;

        return;
    }

    void setupFocusObject(string name)
    {
        GameObject obj = this.focusObj = new GameObject(name);
        obj.transform.position = this.focus;
        obj.transform.LookAt(this.transform.position);

        return;
    }
}
