using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��
/// </summary>
public class Door : MonoBehaviour
{
    //�ſ���״̬
    public bool isOpen;
    //����״̬
    public bool isLock;
    // Start is called before the first frame update
    void Start()
    {
        isLock = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// ����
    /// </summary>
    public void OpenDoor()
    {
        //���ţ��ı���״̬
        transform.parent.Rotate(0, -90, 0);
        isOpen = true;
    }
    /// <summary>
    /// ����
    /// </summary>
    public void UnLock()
    {
        isLock = false;
    }
}
