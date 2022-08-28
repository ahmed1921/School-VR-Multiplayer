using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject GenericVRPlayerPrefab;

    public Transform Teacherposition;
    public Transform[] studentsposition; 
    // Start is called before the first frame update
    public void Start()
    {
        object designation;
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerVRConstants.Designation, out designation))
            {             
                object has_teacher;
                int index = 0; 
                if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(MultiplayerVRConstants.HAS_TEACHER, out has_teacher))
                {
                    index = (bool)has_teacher ? 1 : 0;
                }
                Transform spot = (string)designation.ToString().ToLower() == "teacher" ? Teacherposition : studentsposition[PhotonNetwork.CurrentRoom.PlayerCount - (1+ index)].transform;
                var Player =  PhotonNetwork.Instantiate(GenericVRPlayerPrefab.name, spot.position, Quaternion.identity);
            }
        }

    }
}
