using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 武器类
/// </summary>
public class Weapon : MonoBehaviour
{
    //旋转速度
    public float roundSpeed = 100f;
    //玩家手部武器位置
    public Transform weaponPos;
    //玩家
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //自转
        this.transform.Rotate(Vector3.up, roundSpeed * Time.deltaTime);
    }
    /// <summary>
    /// 装备武器
    /// </summary>
    public void WearWeapon()
    {
        this.transform.SetParent(weaponPos);
        this.transform.localPosition = Vector3.zero;
        this.transform.localEulerAngles = Vector3.zero;
        //改变玩家攻击类型
        player.atkType = AtkType.ShortSword;
        //销毁武器碰撞器
        Destroy(GetComponent<CapsuleCollider>());
        //销毁自己
        Destroy(this);
    }
}
