using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    public float offsetY = 5f;
    public float offsetZ = -5f;
    public float cameraMoveSpeed = 10f;
    public float cameraRoundSpeed = 10f;
    private Vector3 position;
    private Quaternion rotation;

    private float pitch;
    [HideInInspector]
    public static float yaw;
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
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -60f, 30f);

        rotation = Quaternion.Euler(pitch, yaw, 0);
        position = target.transform.position + rotation * new Vector3(0, offsetY, offsetZ);

        transform.position = position;
        transform.LookAt(target.transform.position);

    }
}
