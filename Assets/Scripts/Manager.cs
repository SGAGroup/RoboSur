using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

namespace Com.sgagdr.BlackSky
{
    public class ProfileData
    {
        public string username;
        public int xp;
        public int level;
    }

    public class ProfileInfo : MonoBehaviourPun
    {
        public ProfileData profile;
        public int id;
        public int deathCount = 0;
        public int killCount = 0;

        ProfileInfo(ProfileData p, int i, int d, int k)
        {
            this.profile = p;
            this.id = i;
            this.deathCount = d;
            this.killCount = k;
        }
    }


    public class Manager : MonoBehaviour, IOnEventCallback
    {
        public string playerPrefab;
        public Transform[] spawnPoints;

        public List<ProfileInfo> playerStats = new List<ProfileInfo>();

        public enum EventCodes : byte
        {
            NewPlayer,
            UpdatePlayers,
            ChangeStat
        }

        #region MonoBehaviour Callbacks

        void Start()
        {
            ValidateConnection();
            //NewPlayer_S(Launcher.myProfile);
            Spawn();
        }
        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }
        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        #endregion

        #region Methods
        public void Spawn()
        {
            Transform t_spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            PhotonNetwork.Instantiate(playerPrefab, t_spawn.position, t_spawn.rotation);
        }

        private void ValidateConnection()
        {
            if (PhotonNetwork.IsConnected) return;
            SceneManager.LoadScene(1);
        }


        #endregion

        #region Photon
        
        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code >= 200) return;

            EventCodes e = (EventCodes)photonEvent.Code;
            object[] o = (object[])photonEvent.CustomData;
            /*
            switch (e)
            {
                case EventCodes.NewPlayer:
                    NewPlayer_R(o);
                    break;
                case EventCodes.UpdatePlayers:
                    UpdatePlayers_R(o);
                    break;
                case EventCodes.ChangeStat:
                    ChangeStat_R(o);
                    break;
            }*/
        }

        #endregion


        #region Events
        public void NewPlayer_S(ProfileData p)
        {
            object[] package = new object[6];
            package[0] = p.username;
            package[1] = p.level;
            package[2] = p.xp;
            package[3] = PhotonNetwork.LocalPlayer.ActorNumber;
            package[4] = (short) 0;
            package[5] = (short) 0;

            PhotonNetwork.RaiseEvent(
                (byte) EventCodes.NewPlayer,
                package,
                new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient},
                new SendOptions { Reliability = true}
                );
        }





        #endregion

    }
}