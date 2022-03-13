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
        public static ProfileData myProfile = new ProfileData();

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
            PhotonNetwork.CreateRoom("");
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
    }
}
