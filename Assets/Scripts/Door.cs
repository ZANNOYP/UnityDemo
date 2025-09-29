using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleDoor()
    {
        if (isOpen)
        {

            transform.parent.Rotate(0, 90, 0);
            isOpen = false;
        }
        else
        {
            transform.parent.Rotate(0, -90, 0);
            isOpen = true;
        }
    }
}
