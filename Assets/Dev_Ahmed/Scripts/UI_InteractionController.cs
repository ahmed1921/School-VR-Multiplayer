using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;
using Photon.Realtime;
using Photon.Pun;

public class UI_InteractionController : MonoBehaviour
{
    [SerializeField]
    GameObject UIController;

    [SerializeField]
    GameObject BaseController;

    [SerializeField]
    InputActionReference inputActionReference_UISwitcher;
    [SerializeField]
    InputActionReference inputActionReference_CharacterInteractor;
    [SerializeField] GameObject PlayerDetail;
    [SerializeField] Transform PlayersDetailParent;

    bool isUICanvasActive = false;
    bool isInteractorHandActive = false;

    [SerializeField]
    GameObject UIGameObject;

    [SerializeField]
    GameObject AllPlayersDetailUI;

    [SerializeField] PlayerNetworkSetup Playernetwork;




    private void OnEnable()
    {
        inputActionReference_UISwitcher.action.performed += ActivateUIMode;
        inputActionReference_CharacterInteractor.action.performed += ActivateInteractorHand;
    }
    private void OnDisable()
    {
        inputActionReference_UISwitcher.action.performed -= ActivateUIMode;
        inputActionReference_CharacterInteractor.action.performed -= ActivateInteractorHand;

    }

    private void Start()
    {
        //Deactivating UI Canvas Gameobject by default
        if (UIGameObject != null)
        {
            UIGameObject.SetActive(false);

        }

        if (AllPlayersDetailUI != null)
        {
            AllPlayersDetailUI.SetActive(false);

        }

        //Deactivating UI Controller by default
        UIController.GetComponent<XRRayInteractor>().enabled = false;
        UIController.GetComponent<XRInteractorLineVisual>().enabled = false;
    }

    /// <summary>
    /// This method is called when the player presses UI Switcher Button which is the input action defined in Default Input Actions.
    /// When it is called, UI interaction mode is switched on and off according to the previous state of the UI Canvas.
    /// </summary>
    /// <param name="obj"></param>
    /// 

    public void InitializePlayers()
    {
        foreach(Player player in PhotonNetwork.PlayerList) {

            PhotonView photonView= null;
            if (player.NickName == PhotonNetwork.LocalPlayer.NickName)
            {
                continue;
            }
            foreach (var gamebject in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (gamebject.GetComponent<PhotonView>().Owner.NickName == player.NickName)
                {
                    photonView = gamebject.GetComponent<PhotonView>();
                }
            }
            var playerDetail = Instantiate(PlayerDetail);
            playerDetail.transform.SetParent(PlayersDetailParent, false);
           
            playerDetail.GetComponent<PlayerDetailUI>().init(player, Playernetwork.Designation.ToLower() == "student"?true:false , photonView);
            playerDetail.SetActive(true);
        }
    }
    public void ClearplayerData()
    {
        foreach (Transform player in PlayersDetailParent)
        {
            Destroy(player.gameObject);
        }
    }


    public void ActivateHostControls()
    {
        if (isUICanvasActive)
            return;

        if (!isInteractorHandActive)
        {
            InitializePlayers();
            AllPlayersDetailUI.SetActive(true);
            UIController.GetComponent<XRRayInteractor>().enabled = true;
            UIController.GetComponent<XRInteractorLineVisual>().enabled = true;
            BaseController.GetComponent<XRDirectInteractor>().enabled = false;
            isInteractorHandActive = true;

        }
        else
        {
            ClearplayerData();
            AllPlayersDetailUI.SetActive(false);
            UIController.GetComponent<XRRayInteractor>().enabled = false;
            UIController.GetComponent<XRInteractorLineVisual>().enabled = false;
            BaseController.GetComponent<XRDirectInteractor>().enabled = true;
            isInteractorHandActive = false;
        }
    }

    public void ActivateInteractorHand(InputAction.CallbackContext obj)
    {
        ActivateHostControls();
    }


    private void ActivateUIMode(InputAction.CallbackContext obj)
    {
        ActivateUI();
    }

    public void ActivateUI()
    {
        if (isInteractorHandActive)
            return;
        if (!isUICanvasActive)
        {
            isUICanvasActive = true;

            //Activating UI Controller by enabling its XR Ray Interactor and XR Interactor Line Visual
            UIController.GetComponent<XRRayInteractor>().enabled = true;
            UIController.GetComponent<XRInteractorLineVisual>().enabled = true;

            //Deactivating Base Controller by disabling its XR Direct Interactor
            BaseController.GetComponent<XRDirectInteractor>().enabled = false;



            //Activating the UI Canvas Gameobject
            UIGameObject.SetActive(true);
        }
        else
        {
            isUICanvasActive = false;

            //De-Activating UI Controller by enabling its XR Ray Interactor and XR Interactor Line Visual
            UIController.GetComponent<XRRayInteractor>().enabled = false;
            UIController.GetComponent<XRInteractorLineVisual>().enabled = false;

            //Activating Base Controller by disabling its XR Direct Interactor
            BaseController.GetComponent<XRDirectInteractor>().enabled = true;

            //De-Activating the UI Canvas Gameobject
            UIGameObject.SetActive(false);
        }
    }
}
