using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �����������
/// </summary>
public class CameraController : MonoBehaviour
{
    //���������Ŀ��
    public GameObject target;
    //�����y�ᡢz��ƫ��
    public float offsetY = 5f;
    public float offsetZ = -5f;
    //������ƶ��ٶ�
    public float cameraMoveSpeed = 10f;
    //�������ת�ٶ�
    public float cameraRoundSpeed = 10f;

    private Vector3 position;
    private Quaternion rotation;
    //����������ӽ���ת�Ƕ�
    private float pitch;
    //����ͷ�����ӽ���ת�Ƕ�
    [HideInInspector]
    public static float yaw;
    //���������
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
        //�����ӽ���ת�Ƕ�
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //�����ӽ���ת�Ƕ�
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        //���������ӽ���ת�Ƕ�
        pitch = Mathf.Clamp(pitch, -60f, 30f);
        //�����Ŀ����ת����
        rotation = Quaternion.Euler(pitch, yaw, 0);
        //���������λ��
        position = target.transform.position + rotation * new Vector3(0, offsetY, offsetZ);
        //���������λ��
        transform.position = position;
        transform.LookAt(target.transform.position);

    }
}
