using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rio : MonoBehaviour
{
    //��ҿ��ƽű�
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void AtkOver()
    {
        //���״̬תΪ�������㼶Ȩ�ظ�Ϊ0
        player.state = statePlayer.Idle;
        player.animator.SetLayerWeight(1, 0);
    }

    /// <summary>
    /// �������μ��
    /// </summary>
    public void AtkEvent()
    {
        //����������
        if (Physics.SphereCast(player.gameObject.transform.position + Vector3.up * 1.3f + player.gameObject.transform.right * 0.5f, 
                                0.1f, 
                                player.gameObject.transform.forward, 
                                out RaycastHit hit, 
                                0.8f, 
                                1 << LayerMask.NameToLayer("Monster"), 
                                QueryTriggerInteraction.Ignore)) 
        {
            Monster m = hit.collider.gameObject.GetComponent<Monster>();
            //�򵽹�������Ѫ
            if (m != null)
            {
                m.Wound();
                print("�����ɹ�");
            }
                
        }
    }
}
