using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 摄像机控制类
/// </summary>
public class CameraController : MonoBehaviour
{
    //摄像机看向目标
    public GameObject target;
    //摄像机y轴、z轴偏移
    public float offsetY = 5f;
    public float offsetZ = -5f;
    //摄像机移动速度
    public float cameraMoveSpeed = 10f;
    //摄像机旋转速度
    public float cameraRoundSpeed = 10f;

    private Vector3 position;
    private Quaternion rotation;
    //摄像机上下视角旋转角度
    private float pitch;
    //摄像头左右视角旋转角度
    [HideInInspector]
    public static float yaw;
    //鼠标灵敏度
    private float mouseSensitivity = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void LateUpdate()
    {
        //左右视角旋转角度
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //上下视角旋转角度
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        //限制上下视角旋转角度
        pitch = Mathf.Clamp(pitch, -60f, 30f);
        //摄像机目标旋转方向
        rotation = Quaternion.Euler(pitch, yaw, 0);
        //摄像机看向位置
        position = target.transform.position + rotation * new Vector3(0, offsetY, offsetZ);
        //设置摄像机位置
        transform.position = position;
        transform.LookAt(target.transform.position);

    }
}
