using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loginUiManager : MonoBehaviour
{
    public GameObject ConnectoptionPanel;
    public GameObject ConnectWithNamePanelGameobject;


    // Start is called before the first frame update
    void Start()
    {
        ConnectoptionPanel.SetActive(true);
        ConnectWithNamePanelGameobject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
