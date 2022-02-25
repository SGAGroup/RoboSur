using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.sgagdr.BlackSky
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        //��������� ���� ���������
        //����������� ������ ������
        //��� ��� ������� - ����������� OnEnable()
        //������� � ���� ������� �������� ������������
        //������� ����������� ��������� �������� ����� � Photon'�� � ��� ���������
        //���� �� ������ - ��������� � OnConnectedToMaster()
        //������� ����� ���������� ��� � ��������� ������� ��� ���� - Join()
        //� �� ���� �� ����� ���� �������, �� ��� ����� ������ ���� - StartGame();

        //���� ����������� � ��������� ������� �� ������� - ������� ����

        public void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            Connect();
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
