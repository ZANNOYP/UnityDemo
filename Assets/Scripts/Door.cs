using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��
/// </summary>
public class Door : MonoBehaviour
{
    //��״̬
    public bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenDoor()
    {
        //���ţ��ı���״̬
        transform.parent.Rotate(0, -90, 0);
        isOpen = true;
    }
}
