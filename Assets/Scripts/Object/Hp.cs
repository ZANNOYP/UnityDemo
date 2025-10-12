using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp : MonoBehaviour
{
    //Ѫ����ת�ٶ�
    public float roundSpeed = 5f;
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

    private void OnTriggerEnter(Collider other)
    {
        //��Ҵ�����Ѫ
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player p = other.gameObject.GetComponent<Player>();
            if (p != null)
            {
                //��һ�Ѫ
                p.AddHp();
                //�����Լ�
                Destroy(this.gameObject);
            }
        }
    }
}
