using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
                TipPanel.Instance.UpdateTip("¹§Ï²¹ý¹Ø");
                TipPanel.Instance.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
}
