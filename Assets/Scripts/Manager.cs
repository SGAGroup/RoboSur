using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine.UI;

namespace Com.sgagdr.BlackSky
{

    public class PlayerInfo
    {
        public ProfileData profile;
        public int id;
        public int deathCount = 0;
        public int killCount = 0;

        public PlayerInfo(ProfileData p, int i, int d, int k)
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

        public List<PlayerInfo> playerInfo = new List<PlayerInfo>();
        public int myind = 0;

        private Text ui_mykills;
        private Text ui_mydeaths;
        private Transform ui_leaderboard;

        public enum EventCodes : byte
        {
            NewPlayer,
            UpdatePlayers,
            ChangeStat,
            PlayerDisconnected
        }

        #region MonoBehaviour Callbacks

        void Start()
        {
            ValidateConnection();
            NewPlayer_S(Launcher.myProfile);
            Spawn();
            InitializeUI();

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (ui_leaderboard.gameObject.activeSelf) ui_leaderboard.gameObject.SetActive(false);
                else Leaderboard(ui_leaderboard);
            }
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }
        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
            //PlayerDisconnected_S(myind);
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

        private void InitializeUI()
        {
            ui_mykills = GameObject.Find("HUD/Stats/Kills").GetComponent<Text>();
            ui_mydeaths = GameObject.Find("HUD/Stats/Deaths").GetComponent<Text>();
            ui_leaderboard = GameObject.Find("HUD").transform.Find("LeaderBoard").transform;

            //ui_leaderboard.gameObject.SetActive(false);
            RefreshMyStats();
        }

        private void RefreshMyStats()
        {
            if (playerInfo.Count > myind)
            {
                ui_mykills.text = $"Kills: {playerInfo[myind].killCount}";
                ui_mydeaths.text = $"Deaths: {playerInfo[myind].deathCount}";
            }
            else
            {
                ui_mykills.text = "Kills: 0";
                ui_mydeaths.text = "Deaths: 0";
            }
            if (ui_leaderboard.gameObject.activeSelf) Leaderboard(ui_leaderboard);
            ui_leaderboard.gameObject.SetActive(false);
        }

        private void Leaderboard(Transform p_leaderboard)
        {

            for (int i = 2; i < p_leaderboard.childCount; i++)
            {
                Destroy(p_leaderboard.GetChild(i).gameObject);
            }

            /* Задает надписи header'y
            p_leaderboard.Find("Mode").GetComponent<Text>().text = "DEATHMATCH";
            p_leaderboard.Find("Map").GetComponent<Text>().text = "QUAKE3 DM";
            */
            p_leaderboard.Find("Header/HeaderText").GetComponent<Text>().text = "DEATHMATCH";

            GameObject playercard = p_leaderboard.GetChild(1).gameObject;
            playercard.SetActive(false);

            List<PlayerInfo> sorted = SortList(playerInfo);

            foreach (PlayerInfo info in sorted)
            {
                GameObject newCard = Instantiate(playercard, p_leaderboard) as GameObject;

                /*
                if (isAlternateColors) newCard.GetComponent<Image>().color = new Color(0, 0, 0, 50);
                isAlternateColors = !isAlternateColors;
                */

                newCard.transform.Find("ID_value").GetComponent<Text>().text = (info.id).ToString();
                newCard.transform.Find("Name_value").GetComponent<Text>().text = info.profile.username;
                newCard.transform.Find("Score_value").GetComponent<Text>().text = (info.killCount * 100).ToString();
                newCard.transform.Find("Kills_value").GetComponent<Text>().text = (info.killCount).ToString();
                newCard.transform.Find("Death_value").GetComponent<Text>().text = (info.deathCount).ToString();

                newCard.SetActive(true);
            }

            p_leaderboard.gameObject.SetActive(true);
        }

        private List<PlayerInfo> SortList(List<PlayerInfo> p_list)
        {
            List<PlayerInfo> sorted = new List<PlayerInfo>();

            while (sorted.Count < p_list.Count)
            {
                int highest = -1;
                PlayerInfo selection = p_list[0];

                foreach (PlayerInfo info in p_list)
                {
                    if (sorted.Contains(info)) continue;
                    if (info.killCount > highest)
                    {
                        selection = info;
                        highest = info.killCount;
                    }
                }

                sorted.Add(selection);
            }
            return sorted;
        }

        #endregion

        #region Photon

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code >= 200) return;
            EventCodes e = (EventCodes)photonEvent.Code;
            object[] o = (object[])photonEvent.CustomData;

            Debug.Log($"New event, code = {photonEvent.Code}");
            Debug.Log(photonEvent);

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
                case EventCodes.PlayerDisconnected:
                    PlayerDisconnected_R(o);
                    break;
            }

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
            package[4] = (int)0;
            package[5] = (int)0;

            PhotonNetwork.RaiseEvent(
                (byte)EventCodes.NewPlayer,
                package,
                new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient },
                new SendOptions { Reliability = true }
                );
        }

        public void NewPlayer_R(object[] data)
        {
            PlayerInfo p = new PlayerInfo(
                new ProfileData(
                    (string)data[0],
                    (int)data[1],
                    (int)data[2]),
                (int)data[3],
                (int)data[4],
                (int)data[5]
            );

            playerInfo.Add(p);

            UpdatePlayers_S(playerInfo);
        }

        public void UpdatePlayers_S(List<PlayerInfo> info)
        {
            object[] package = new object[info.Count];

            for (int i = 0; i < info.Count; i++)
            {
                object[] piece = new object[6];

                piece[0] = info[i].profile.username;
                piece[1] = info[i].profile.level;
                piece[2] = info[i].profile.xp;
                piece[3] = info[i].id;
                piece[4] = info[i].killCount;
                piece[5] = info[i].deathCount;

                package[i] = piece;
            }

            PhotonNetwork.RaiseEvent(
                (byte)EventCodes.UpdatePlayers,
                package,
                new RaiseEventOptions { Receivers = ReceiverGroup.All },
                new SendOptions { Reliability = true }
                );
        }

        public void UpdatePlayers_R(object[] data)
        {
            playerInfo = new List<PlayerInfo>();

            for (int i = 0; i < data.Length; i++)
            {
                object[] extract = (object[])data[i];

                PlayerInfo p = new PlayerInfo(
                new ProfileData(
                    (string)extract[0],
                    (int)extract[1],
                    (int)extract[2]),
                (int)extract[3],
                (int)extract[4],
                (int)extract[5]
                );

                playerInfo.Add(p);

                if (PhotonNetwork.LocalPlayer.ActorNumber == p.id) myind = i;
            }
        }

        public void ChangeStat_S(int id, byte stat, byte amt)
        {
            object[] package = new object[] { id, stat, amt };

            PhotonNetwork.RaiseEvent(
                (byte)EventCodes.ChangeStat,
                package,
                new RaiseEventOptions { Receivers = ReceiverGroup.All },
                new SendOptions { Reliability = true }
                );
        }

        public void ChangeStat_R(object[] data)
        {
            int id = (int)data[0];
            int stat = (byte)data[1];
            int amt = (byte)data[2];

            for (int i = 0; i < playerInfo.Count; i++)
            {
                if (playerInfo[i].id == id)
                {
                    switch (stat)
                    {
                        case 0: //kills
                            playerInfo[i].killCount += amt;
                            Debug.Log($"Player {playerInfo[i].profile.username} : kills = {playerInfo[i].killCount}");
                            break;
                        case 1: //death
                            playerInfo[i].deathCount += amt;
                            Debug.Log($"Player {playerInfo[i].profile.username} : death = {playerInfo[i].deathCount}");
                            break;
                    }

                    if (id == myind)
                    {
                        RefreshMyStats();
                    }
                    if (ui_leaderboard.gameObject.activeSelf) Leaderboard(ui_leaderboard);
                    return;
                }
            }
        }
        public void PlayerDisconnected_S(int id)
        {
            object[] package = new object[] { id };

            PhotonNetwork.RaiseEvent(
                (byte)EventCodes.PlayerDisconnected,
                package,
                new RaiseEventOptions { Receivers = ReceiverGroup.All },
                new SendOptions { Reliability = true }
                );
        }

        public void PlayerDisconnected_R(object[] data)
        {
            int id = (int)data[0];

            for (int i = 0; i < playerInfo.Count; i++)
            {
                if (playerInfo[i].id == id)
                {
                    Debug.LogWarning($"Found player with id(actor number): {id}, His name: {playerInfo[i].profile.username}");
                    playerInfo.RemoveAt(i);
                }
            }


            UpdatePlayers_S(playerInfo);
        }

        #endregion

    }
}