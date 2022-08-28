using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Message : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI greetMessage;

    public void greet(string message)
    {
      
        greetMessage.text = message;
    }


    // Update is called once per frame

}
