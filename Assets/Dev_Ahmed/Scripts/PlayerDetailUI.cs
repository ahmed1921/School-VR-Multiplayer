using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
public class PlayerDetailUI : MonoBehaviourPun
{
    [SerializeField] Image Avatar;
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] Button MicOn;
    [SerializeField] Button MicOff;
    [SerializeField] Button Greet;
    PhotonView photonView1;

    public void init(Player player, bool isStudent, PhotonView photonView)
    {
        Name.text = player.NickName;
        photonView1 = photonView;
        if (isStudent)
        {
            MicOn.gameObject.SetActive(false);
            MicOff.gameObject.SetActive(false);
            Greet.gameObject.SetActive(true);
            Greet.onClick.AddListener(() =>
            {
                photonView1.RPC("SendMessage", player, "You received greetings from " + PhotonNetwork.LocalPlayer.NickName);              

            });
        }



    }
}
