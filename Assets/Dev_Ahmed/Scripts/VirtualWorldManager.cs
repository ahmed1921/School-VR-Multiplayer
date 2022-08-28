using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class VirtualWorldManager : MonoBehaviourPunCallbacks
{
    public static VirtualWorldManager instance;

    private void Awake()
    {
        if(instance!=null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }


    public void LeaveRoomAndLoadHome()
    {
        PhotonNetwork.LeaveRoom();
    }


    #region PhotonCallbackMethods

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " player " + PhotonNetwork.CurrentRoom.PlayerCount);

    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {

        PhotonNetwork.LoadLevel("HomeScene");

    }


    public void OnApplicationQuit()
    {
        PhotonNetwork.LeaveRoom();
    }


    #endregion


}
