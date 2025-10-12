using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 门
/// </summary>
public class Door : MonoBehaviour
{
    //门开关状态
    public bool isOpen;
    //门锁状态
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
    /// 开门
    /// </summary>
    public void OpenDoor()
    {
        //开门，改变门状态
        transform.parent.Rotate(0, -90, 0);
        isOpen = true;
    }
    /// <summary>
    /// 解锁
    /// </summary>
    public void UnLock()
    {
        isLock = false;
    }
}
