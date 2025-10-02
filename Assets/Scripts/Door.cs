using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 门
/// </summary>
public class Door : MonoBehaviour
{
    //门状态
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
        //开门，改变门状态
        transform.parent.Rotate(0, -90, 0);
        isOpen = true;
    }
}
