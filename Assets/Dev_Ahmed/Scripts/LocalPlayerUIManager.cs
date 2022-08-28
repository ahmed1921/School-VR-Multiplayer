using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LocalPlayerUIManager : MonoBehaviour
{
    [SerializeField] GameObject GoHome_button;

    // Start is called before the first frame update
    void Start()
    {
        GoHome_button.GetComponent<Button>().onClick.AddListener(VirtualWorldManager.instance.LeaveRoomAndLoadHome);
    }

   
}
