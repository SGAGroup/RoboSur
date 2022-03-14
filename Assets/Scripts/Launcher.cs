using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

namespace Com.sgagdr.BlackSky
{
    [System.Serializable]
    public class ProfileData
    {
        public string username;
        public int xp;
        public int level;
        public ProfileData()
        {
            username = "";
            xp = 0;
            level = 0;
        }
        public ProfileData(string p_username, int p_xp, int p_level)
        {
            username = p_username;
            xp = p_xp;
            level = p_level;
        }
    }

    public class Launcher : MonoBehaviourPunCallbacks
    {
        public string defaultName = "defaultName";
        public InputField usernameInput;
        public InputField roomnameField;
        public static ProfileData myProfile = new ProfileData();

        public GameObject tabMain;
        public GameObject tabRooms;
        public GameObject tabCreate;

        public GameObject buttonRoom;

        private List<RoomInfo> roomList;

        public byte maxPlayers = 20;

        public void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            myProfile = Data.LoadProfile();
            usernameInput.text = myProfile.username;
            Connect();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);


        }
        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected!");

            PhotonNetwork.JoinLobby();
            base.OnConnectedToMaster();
        }

        public override void OnJoinedRoom()
        {
            StartGame();
            
            base.OnJoinedRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Create(); //������� �������

            base.OnJoinRandomFailed(returnCode, message);
        }

        private void VerifyUsername()
        {
            if (string.IsNullOrEmpty(usernameInput.text))
            {
                myProfile.username = defaultName;
            }
            else
            {
                myProfile.username = usernameInput.text;
            }
        }

        


        public void Create()
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = maxPlayers;

            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add("map", 0);

            options.CustomRoomProperties = properties;

            PhotonNetwork.CreateRoom(roomnameField.text, options);
        }

        public void Connect()
        {
            PhotonNetwork.GameVersion = "0";
            PhotonNetwork.ConnectUsingSettings();
        }
        public void Join()
        {
            VerifyUsername();
            PhotonNetwork.JoinRandomRoom();
        }
        public void StartGame()
        {
            VerifyUsername();

            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Data.SaveProfile(myProfile);
                PhotonNetwork.LoadLevel(2);
            }
        }

        public void TabCloseAll()
        {
            tabMain.SetActive(false);
            tabRooms.SetActive(false);
            tabCreate.SetActive(false);
        }

        public void TabOpenMain()
        {
            TabCloseAll();
            tabMain.SetActive(true);
        }
        public void TabOpenRooms()
        {
            TabCloseAll();
            tabRooms.SetActive(true);
        }
        public void TabOpenCreate()
        {
            TabCloseAll();
            tabCreate.SetActive(true);
        }

        private void ClearRoomList()
        {
            Transform content = tabRooms.transform.Find("Scroll View/Viewport/Content");
            foreach (Transform a in content) Destroy(a.gameObject);
        }

        public override void OnRoomListUpdate(List<RoomInfo> p_list)
        {
            roomList = p_list;
            ClearRoomList();

            Transform content = tabRooms.transform.Find("Scroll View/Viewport/Content");

            foreach (RoomInfo a in roomList) {
                GameObject newRoomButton = Instantiate(buttonRoom, content) as GameObject;
                newRoomButton.transform.Find("Name").GetComponent<Text>().text = a.Name;
                newRoomButton.transform.Find("Players").GetComponent<Text>().text = a.PlayerCount + "/" + a.MaxPlayers+"";

                newRoomButton.GetComponent<Button>().onClick.AddListener(delegate { JoinRoom(newRoomButton.transform); });
            }

            base.OnRoomListUpdate(roomList);
        }

        public void JoinRoom(Transform p_button)
        {
            string t_roomName = p_button.transform.Find("Name").GetComponent<Text>().text;
            PhotonNetwork.JoinRoom(t_roomName);
        }
    }
}
