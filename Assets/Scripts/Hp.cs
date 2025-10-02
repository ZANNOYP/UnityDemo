using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp : MonoBehaviour
{
    public float roundSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, roundSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            print("Íæ¼Ò´¥Åö");
            Player p = other.gameObject.GetComponent<Player>();
            if (p != null)
            {
                p.AddHp();
                Destroy(this.gameObject);
            }
        }
    }
}
