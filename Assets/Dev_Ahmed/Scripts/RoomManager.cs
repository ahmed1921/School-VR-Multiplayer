using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
public class RoomManager : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public enum Represent
    {
        student,
        teacher
    }


    public Represent represent = Represent.student;
    private string MapType;
    private bool IsTeacher;
    public GameObject RoomObject;
    public Transform RoomParent;
    [SerializeField] Button StudenButton;
    [SerializeField] Button TeacherButton;

    [SerializeField] TextMeshProUGUI availableRooms;
    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    // Start is called before the first frame update
    void Start()
    {
        StudenButton.Select();
        PhotonNetwork.AutomaticallySyncScene = true;
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    #region UICallBackMethods


    public void OnEnteredRoom_School()
    {
        AddlocalPlayerCustomproperties();
        MapType = MultiplayerVRConstants.MAP_TYPE_VALUE_SCHOOL;
        ExitGames.Client.Photon.Hashtable expectedCustomProperties;
        switch (represent)
        {
            case Represent.student:
                PhotonNetwork.JoinRandomRoom();
                break;
            case Represent.teacher:
                expectedCustomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.HAS_TEACHER, false } };
                PhotonNetwork.JoinRandomRoom(expectedCustomProperties, 0);
                break;
        }

    }

    public void SetPersonType(string Type)
    {
        switch (Type.ToLower())
        {
            case "teacher":
                represent = Represent.teacher;
                updateRoomList();
                break;
            case "student":
                represent = Represent.student;
                updateRoomList();
                break;

        }

    }
    // This function is responsible for managing the rooms that are created and removed
    public void updateRoomList()
    {
        if (represent == Represent.teacher)
        {
            foreach (RoomDetail rooms in RoomParent.GetComponentsInChildren(typeof(RoomDetail), true))
            {
                print("disabling");

                if (rooms.getHasTeacher() || rooms.getCurrentPlayer() >= MultiplayerVRConstants.ROOM_LIMIT)
                {
                    print("disabling");
                    rooms.gameObject.SetActive(false);
                }
                else
                {
                    rooms.gameObject.SetActive(true);

                }
            }
        }
        if (represent == Represent.student)
        {
            foreach (RoomDetail rooms in RoomParent.GetComponentsInChildren(typeof(RoomDetail), true))
            {
                if (rooms.getCurrentPlayer() >= MultiplayerVRConstants.ROOM_LIMIT)
                {
                    rooms.gameObject.SetActive(false);
                }
                else
                {
                    rooms.gameObject.SetActive(true);
                }
            }
        }

    }

    #endregion


    #region PhotonCallback

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        CreateAndJoinRoom();

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        cachedRoomList = roomList;
        if (roomList.Count == 0)
        {
            availableRooms.text = "Currently no room available";
            return;
        }

        foreach (Transform child in RoomParent)
        {
            DestroyImmediate(child.gameObject);
        }
        Debug.Log("Total no of rooms Present are : " + roomList.Count);
        foreach (RoomInfo room in roomList)
        {
            if (room.PlayerCount <= 0)
            {
                return;
            }
            if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_SCHOOL))
            {
                object teacherResult;
                if (room.CustomProperties.TryGetValue(MultiplayerVRConstants.HAS_TEACHER, out teacherResult))
                {
                    teacherResult = (bool)teacherResult;
                    Debug.Log("This system has " + (bool)teacherResult + MultiplayerVRConstants.Teacher);
                }

                GameObject roomDetailPrefab = Instantiate(RoomObject, RoomObject.transform.position, Quaternion.identity);
                roomDetailPrefab.transform.SetParent(RoomParent, false);


                roomDetailPrefab.GetComponent<RoomDetail>().initRoomDetail(room.PlayerCount.ToString(), room.MaxPlayers.ToString(), room.Name, (bool)teacherResult, joinSpecifiedRoom);
            }
        }

    }


    public override void OnJoinedLobby()
    {
        object IsTeacher = FindLocalPlayerproperty(MultiplayerVRConstants.Designation);
        if (IsTeacher == null) return;
        Debug.Log("Have joined successfully");

        if (IsTeacher != null && (string)IsTeacher.ToString().ToLower() == MultiplayerVRConstants.Teacher)
        {
            represent = Represent.teacher;
            UpdateLocalPlayerCustomProperty(MultiplayerVRConstants.Designation, MultiplayerVRConstants.Teacher);
            TeacherButton.Select();
        }
        else
        {
            represent = Represent.student;
            UpdateLocalPlayerCustomProperty(MultiplayerVRConstants.Designation, MultiplayerVRConstants.Student);
            StudenButton.Select();

        }
    }

    public override void OnConnectedToMaster()
    {
        availableRooms.text = "Loading Rooms";
        PhotonNetwork.JoinLobby();
    }

    public void joinSpecifiedRoom(string roomName)
    {
        CreateAndJoinRoom(roomName);
    }


    public override void OnCreatedRoom()
    {
        Debug.Log("A room has been created  " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("A Local player " + PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " Player count " + PhotonNetwork.CurrentRoom.PlayerCount);

        object mapType = FindCustomRoomProperties(MultiplayerVRConstants.MAP_TYPE_KEY);
        if (mapType != null)
        {
            Debug.Log("Joined room with the map:" + (string)mapType);
            if (MapType == MultiplayerVRConstants.MAP_TYPE_VALUE_SCHOOL)
            {
                PhotonNetwork.LoadLevel("World_School");
            }
            object IsTeacher = FindCustomRoomProperties(MultiplayerVRConstants.HAS_TEACHER);
            if (IsTeacher != null)
            {
                Debug.Log("Joined Teacher with the map:" + (bool)IsTeacher);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " player " + PhotonNetwork.CurrentRoom.PlayerCount);

    }


    #endregion

    #region PrivateMethods
    void CreateAndJoinRoom(string RoomName = null)
    {

        string RandomRoomName = string.IsNullOrWhiteSpace(RoomName) ? "Room_" + MapType + Random.Range(1, 1000) : RoomName;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = MultiplayerVRConstants.ROOM_LIMIT;
        string[] roomPropsInLobby = { MultiplayerVRConstants.HAS_TEACHER, MultiplayerVRConstants.MAP_TYPE_KEY };

        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable();
        customRoomProperties.Add(MultiplayerVRConstants.HAS_TEACHER, represent == Represent.student ? false : true);
        customRoomProperties.Add(MultiplayerVRConstants.MAP_TYPE_KEY, MapType);
        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
        if (FindLocalPlayerproperty(MultiplayerVRConstants.Designation) == null)
            AddlocalPlayerCustomproperties();
        else
        {
            UpdateLocalPlayerCustomProperty(MultiplayerVRConstants.Designation, represent == Represent.student ? MultiplayerVRConstants.Student : MultiplayerVRConstants.Teacher);
        }
        roomOptions.CustomRoomProperties = customRoomProperties;
        PhotonNetwork.JoinOrCreateRoom(RandomRoomName, roomOptions, default);
    }

    public void AddlocalPlayerCustomproperties()
    {
        if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(MultiplayerVRConstants.Designation))
            PhotonNetwork.LocalPlayer.CustomProperties.Add(MultiplayerVRConstants.Designation, represent == Represent.student ? MultiplayerVRConstants.Student : MultiplayerVRConstants.Teacher);

    }

    public object FindLocalPlayerproperty(string key)
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(key))
        {
            object temp;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(key, out temp))
            {
                return temp;
            }
        }
        return null;
    }

    public void UpdateLocalPlayerCustomProperty(string key, string value)
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(key))
        {
            var hash = PhotonNetwork.LocalPlayer.CustomProperties;
            hash[key] = value;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public object FindCustomRoomProperties(string key)
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(key))
        {
            object temp;
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(key, out temp))
            {
                return temp;
            }
        }
        return null;
    }
    public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {

    }

    #endregion

}
