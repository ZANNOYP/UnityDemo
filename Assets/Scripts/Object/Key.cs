using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Կ��
/// </summary>
public class Key : MonoBehaviour
{
    //��ת�ٶ�
    public float roundSpeed = 5f;
    //���Դ򿪵��Ŷ���
    public Door door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //��ת
        transform.Rotate(Vector3.up, roundSpeed * Time.deltaTime);
    }
}
