using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp : MonoBehaviour
{
    //血包旋转速度
    public float roundSpeed = 5f;
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

    private void OnTriggerEnter(Collider other)
    {
        //玩家触碰回血
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player p = other.gameObject.GetComponent<Player>();
            if (p != null)
            {
                //玩家回血
                p.AddHp();
                //销毁自己
                Destroy(this.gameObject);
            }
        }
    }
}
