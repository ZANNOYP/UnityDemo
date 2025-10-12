using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ������
/// </summary>
public class Weapon : MonoBehaviour
{
    //��ת�ٶ�
    public float roundSpeed = 100f;
    //����ֲ�����λ��
    public Transform weaponPos;
    //���
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //��ת
        this.transform.Rotate(Vector3.up, roundSpeed * Time.deltaTime);
    }
    /// <summary>
    /// װ������
    /// </summary>
    public void WearWeapon()
    {
        this.transform.SetParent(weaponPos);
        this.transform.localPosition = Vector3.zero;
        this.transform.localEulerAngles = Vector3.zero;
        //�ı���ҹ�������
        player.atkType = AtkType.ShortSword;
        //����������ײ��
        Destroy(GetComponent<CapsuleCollider>());
        //�����Լ�
        Destroy(this);
    }
}
