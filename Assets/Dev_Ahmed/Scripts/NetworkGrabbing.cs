using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkGrabbing : MonoBehaviour, IPunOwnershipCallbacks
{
    private PhotonView photonView;
    Rigidbody rb;
    bool isbeingHeld = false;
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            OnSelectEnter();
        }


        if (isbeingHeld)
        {
            rb.isKinematic = true;
            gameObject.layer = 11;
        }
        else
        {
            rb.isKinematic = false;
            gameObject.layer = 9;
        }

    }

    void TransferOwnership()
    {

        photonView.RequestOwnership();
    }


    public void OnSelectEnter()
    {

        photonView.RPC("StartNetworkGrabbing", RpcTarget.AllBuffered);
        if (photonView.Owner == PhotonNetwork.LocalPlayer)
        {

        }
        else
        {
            TransferOwnership();
        }

    }

    public void OnSelectExited()
    {
        photonView.RPC("StopNetworkGrabbing", RpcTarget.AllBuffered);

    }

    [PunRPC]
    public void StartNetworkGrabbing()
    {
        isbeingHeld = true;
    }

    [PunRPC]
    public void StopNetworkGrabbing()
    {
        isbeingHeld = false;

    }


    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != photonView)
        {
            return;
        }
        Debug.Log("Ownership Requested for: " +targetView.name + "from "+ requestingPlayer.NickName);
        photonView.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("Ownership Transfered");

    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
       

    }
}
