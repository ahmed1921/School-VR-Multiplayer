using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class RoomDetail : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalPeople;
    [SerializeField] TextMeshProUGUI RoomName;
    [SerializeField] Button EnterButton;
    bool hasTeacher = false;
    int currentPlayer;
    
    public void initRoomDetail(string currentPlayers, string maxPlayers, string roomName,bool hasTeacher, Action<string> OnButtonClick)
    {
        RoomName.text = roomName;
        this.hasTeacher = hasTeacher;
        currentPlayer = int.Parse(currentPlayers);
        EnterButton.onClick.AddListener(() =>
        {
            OnButtonClick(roomName);
        });
        totalPeople.text = currentPlayers + "/" + maxPlayers;
    }


    public bool getHasTeacher()
    {
        return hasTeacher;
    }

    public int getCurrentPlayer()
    {
        return currentPlayer;
    }


}
