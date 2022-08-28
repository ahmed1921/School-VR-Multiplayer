using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class windowsControl : MonoBehaviour
{
    public UI_InteractionController controller;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {  
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (controller != null)
                    controller.ActivateUI();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (controller != null)
                    controller.ActivateHostControls();
            }         
        
    }
}
