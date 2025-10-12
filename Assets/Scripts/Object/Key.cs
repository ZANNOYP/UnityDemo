using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 钥匙
/// </summary>
public class Key : MonoBehaviour
{
    //旋转速度
    public float roundSpeed = 5f;
    //可以打开的门对象
    public Door door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //自转
        transform.Rotate(Vector3.up, roundSpeed * Time.deltaTime);
    }
}
