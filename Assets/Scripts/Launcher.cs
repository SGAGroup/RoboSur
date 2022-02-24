using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.sgagdr.BlackSky
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        //Примерная цепь процессов
        //Запускается данный скрипт
        //При его запуске - запускается OnEnable()
        //Который в свою очередь пытается подключиться
        //Функция подключения старается наладить связь с Photon'ом и его серверами
        //Если всё удачно - переходим к OnConnectedToMaster()
        //Который хочет подключить нас в случайную комнату для игры - Join()
        //А уж если мы нашли себе комнату, то уже можно начать игру - StartGame();

        //Если подключение к случайной комнате не удалось - создаем свою

        public void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            Connect();
        }

        public override void OnConnectedToMaster()
        {
            Join();

            base.OnConnectedToMaster();
        }

        public override void OnJoinedRoom()
        {
            StartGame();

            base.OnJoinedRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Create(); //Создает комнату

            base.OnJoinRandomFailed(returnCode, message);
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
            PhotonNetwork.JoinRandomRoom();
        }
        public void StartGame()
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.LoadLevel(1);
            }
        }
    }
}
