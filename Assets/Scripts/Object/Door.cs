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
    //开门速度
    public float roundSpeed = 100f;
    // 原始角度和目标角度
    private Quaternion closedRot;
    private Quaternion openRot;
    // 旋转角度差（开门旋转多少度）
    public float openAngle = -90f;
    public int doorID;
    // Start is called before the first frame update
    void Start()
    {
        isLock = true;
        // 记录门初始旋转（父物体）
        closedRot = transform.parent.rotation;
        // 目标旋转为在原角度基础上再旋转 openAngle
        openRot = closedRot * Quaternion.Euler(0, openAngle, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //开门
        if (isOpen) 
        {
            transform.parent.rotation = Quaternion.RotateTowards(transform.parent.rotation, openRot, roundSpeed * Time.deltaTime);
        }
    }
    /// <summary>
    /// 开门
    /// </summary>
    public void OpenDoor()
    {
        //开门音效
        MusicMgr.Instance.PlaySound("OpenDoor");
        //改变门状态
        isOpen = true;
    }
    /// <summary>
    /// 解锁
    /// </summary>
    public void UnLock(int doorID)
    {
        if (this.doorID == doorID) 
            isLock = false;
    }
}
