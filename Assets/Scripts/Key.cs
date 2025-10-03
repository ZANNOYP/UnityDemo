using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public float roundSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(Vector3.up, roundSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player p = other.gameObject.GetComponent<Player>();
            if (p != null)
            {
                p.doors[0].UnLock();
                Destroy(this.gameObject);
            }
        }
    }
}
