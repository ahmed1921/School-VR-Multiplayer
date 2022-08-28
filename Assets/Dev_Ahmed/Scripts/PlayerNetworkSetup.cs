using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using Photon.Realtime;
using Photon.Voice.PUN;
public class PlayerNetworkSetup : MonoBehaviourPunCallbacks
{
    public GameObject LocalXRRightGameObject;
    public GameObject MainAvatarGameObject;
    public GameObject AvatarBodyGameobject;
    public GameObject AvatarHeadGameobject;
    public GameObject[] AvatarModelsPrefab;
    public GameObject Greetings;
    public string Designation;
    [SerializeField] XRDirectInteractor directInteractor;
    [SerializeField] XRBaseController xrcontroller;

    public TextMeshProUGUI PlayerNameText;
    private void Start()
    {
        if (photonView.IsMine)
        {

            LocalXRRightGameObject.SetActive(true);
            object AvatarSelectionNo;
            if(PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerVRConstants.AVATAR_SELECTION_NUMBER, out AvatarSelectionNo))
            {
                Debug.Log("Avatar Selection Number "+ (int)AvatarSelectionNo);
                photonView.RPC("initializeSelectedAvatarModel",RpcTarget.AllBuffered,(int)AvatarSelectionNo);
            }
            object designation;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerVRConstants.Designation, out designation))
            {
                
               initializedDesignation((string)designation);
            }

            SetLayerRecursively(AvatarHeadGameobject,6);
            SetLayerRecursively(AvatarBodyGameobject,7);
            TeleportationArea[] teleportationAreas = GameObject.FindObjectsOfType<TeleportationArea>();
            if (teleportationAreas.Length > 0)
            {
                foreach(var item in teleportationAreas)
                {
                    item.teleportationProvider = LocalXRRightGameObject.GetComponent<TeleportationProvider>();
                }
            }
            MainAvatarGameObject.AddComponent<AudioListener>();

        }
        else
        {
            LocalXRRightGameObject.SetActive(false);
            SetLayerRecursively(AvatarHeadGameobject, 0);
            SetLayerRecursively(AvatarBodyGameobject, 0);
        }

        if (PlayerNameText != null)
        {
            PlayerNameText.text = photonView.Owner.NickName;
        }
    }

    void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }

    [PunRPC]

    public void initializeSelectedAvatarModel(int AvatarSelectedNo)
    {
        GameObject selectedAvatarGameobject = Instantiate(AvatarModelsPrefab[AvatarSelectedNo].gameObject, LocalXRRightGameObject.transform);
        AvatarController avatarController = LocalXRRightGameObject.GetComponent<AvatarController>();
        AvatarHolder avatarHolder = selectedAvatarGameobject.GetComponent<AvatarHolder>();
        SetupAvatarGameObject(avatarHolder.HeadTransform, avatarController.AvatarHead);
        SetupAvatarGameObject(avatarHolder.BodyTransform, avatarController.Avatarbody);
        SetupAvatarGameObject(avatarHolder.HandLeftTransform, avatarController.Avatar_Hand_Left);
        SetupAvatarGameObject(avatarHolder.HandRightTransform, avatarController.Avatar_hand_Right);



    }
    public void initializedDesignation(string designation)
    {
        Designation = designation;
        switch (designation.ToLower())
        {
            case "student":
                directInteractor.enabled = false;
                xrcontroller.enabled = false;
                break;
        }
    }



    [PunRPC]
    public void SendMessage(string message)
    {
        if (photonView.IsMine)
        {
            Greetings.GetComponent<Message>().greet(message);
            Greetings.GetComponent<Canvas>().enabled = true;
            Greetings.GetComponent<Animator>().SetTrigger("greet");
        }
    }




    public void SetupAvatarGameObject(Transform avatarTransformModel, Transform MainAvatarTransformModel)
    {
        avatarTransformModel.SetParent(MainAvatarTransformModel);
        avatarTransformModel.localPosition = Vector3.zero;
        avatarTransformModel.localRotation = Quaternion.identity;

    }



    public void MutePlayer(TextMeshProUGUI name)
    {
        print("Muting");
        var photonViews = UnityEngine.Object.FindObjectsOfType<PhotonView>();
        foreach (PhotonView player in photonViews)
        {

            if (player.Owner.NickName == name.text)
            {
                player.gameObject.GetComponent<PhotonVoiceView>().SpeakerInUse.gameObject.SetActive(false);
            }
        }

    }
    public void Unmute(TextMeshProUGUI name)
    {
        var photonViews = UnityEngine.Object.FindObjectsOfType<PhotonView>();
        foreach (PhotonView player in photonViews)
        {

            if (player.Owner.NickName == name.text)
            {
                player.gameObject.GetComponent<PhotonVoiceView>().SpeakerInUse.gameObject.SetActive(true);
            }
        }

    }

}
