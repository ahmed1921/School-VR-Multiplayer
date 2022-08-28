using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class LoginManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField playerName;


    #region UnityMentionds
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region UICallbackMethods

    public void ConnectAnaymously()
    {
        PhotonNetwork.NickName = "Player_ " +Random.Range(1,100);
        PhotonNetwork.ConnectUsingSettings();

    }

    public void ConnectToPhotonServer()
    {
        if (playerName != null)
        {
            PhotonNetwork.NickName = playerName.text;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    #endregion

    #region Photon callback methods

    public override void OnConnected()
    {
        Debug.Log("onConnected is called. The Server is available");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster is called. The MasterServer is connected with Player name  "+PhotonNetwork.NickName );
        PhotonNetwork.LoadLevel("HomeScene");
    }





    #endregion

}
